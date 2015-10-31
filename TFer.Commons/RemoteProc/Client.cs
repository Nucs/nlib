// Decompiled with JetBrains decompiler
// Type: RemoteProcedureCalls.Client
// Assembly: NetworkCommsDotNetComplete, Version=3.0.0.0, Culture=neutral, PublicKeyToken=f58108eb6480f6ec
// MVID: D81C90D5-119C-4F53-86B6-4A32F7B5925E
// Assembly location: F:\__Development\C#\TFer\Libs\NetworkComms\DLLs\Net40\Merged\NetworkCommsDotNetComplete.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;

namespace RemoteProcedureCalls {
    /// <summary>
    ///     Provides functions for managing proxy classes to remote objects client side
    /// </summary>
    public static class Client {
        private static readonly object cacheLocker = new object();

        private static readonly Dictionary<CachedRPCKey, object> cachedInstances =
            new Dictionary<CachedRPCKey, object>();

        private static string fullyQualifiedClassName = typeof (Client).AssemblyQualifiedName;

        static Client() {
            DefaultRPCTimeout = 1000;
            RPCInitialisationTimeout = 1000;
        }

        /// <summary>
        ///     The default timeout period in ms for new RPC proxies. Default value is 1000ms
        /// </summary>
        public static int DefaultRPCTimeout { get; set; }

        /// <summary>
        ///     The timeout period allowed for creating new RPC proxies
        /// </summary>
        public static int RPCInitialisationTimeout { get; set; }

        /// <summary>
        ///     Creates a remote proxy instance for the desired interface with the specified server and object identifier.
        ///     Instance is private to this client in the sense that no one else can
        ///     use the instance on the server unless they have the instanceId returned by this method
        /// </summary>
        /// <typeparam name="I">The interface to use for the proxy</typeparam>
        /// <param name="connection">The connection over which to perform remote procedure calls</param>
        /// <param name="instanceName">The object identifier to use for this proxy</param>
        /// <param name="instanceId">
        ///     Outputs the instance Id uniquely identifying this object on the server.  Can be used to
        ///     re-establish connection to object if connection is dropped
        /// </param>
        /// <param name="options">SendRecieve options to use</param>
        /// <returns>
        ///     A proxy class for the interface I allowing remote procedure calls
        /// </returns>
        public static I CreateProxyToPrivateInstance<I>(Connection connection, string instanceName,
            out string instanceId, SendReceiveOptions options = null) where I : class {
            if (!typeof (I).IsInterface)
                throw new InvalidOperationException(typeof (I).Name + " is not an interface");
            var sendingPacketTypeStr = typeof (I).Name + "-NEW-INSTANCE-RPC-CONNECTION";
            var expectedReturnPacketTypeStr = sendingPacketTypeStr + "-RESPONSE";
            instanceId = connection.SendReceiveObject<string, string>(sendingPacketTypeStr, expectedReturnPacketTypeStr,
                RPCInitialisationTimeout, instanceName);
            if (instanceId == string.Empty)
                throw new RPCException("Server not listening for new instances of type " + typeof (I));
            return Cache<I>.CreateInstance(instanceId, connection, options);
        }

        /// <summary>
        ///     Creates a remote proxy instance for the desired interface with the specified server and object identifier.
        ///     Instance is public in sense that any client can use specified name to make
        ///     calls on the same server side object
        /// </summary>
        /// <typeparam name="I">The interface to use for the proxy</typeparam>
        /// <param name="connection">The connection over which to perform remote procedure calls</param>
        /// <param name="instanceName">The name specified server side to identify object to create proxy to</param>
        /// <param name="instanceId">
        ///     Outputs the instance Id uniquely identifying this object on the server.  Can be used to
        ///     re-establish connection to object if connection is dropped
        /// </param>
        /// <param name="options">SendRecieve options to use</param>
        /// <returns>
        ///     A proxy class for the interface I allowing remote procedure calls
        /// </returns>
        public static I CreateProxyToPublicNamedInstance<I>(Connection connection, string instanceName, out string instanceId, SendReceiveOptions options = null) where I : class
        {
            if (!typeof(I).IsInterface)
                throw new InvalidOperationException(typeof(I).Name + " is not an interface");
            string sendingPacketTypeStr = typeof(I).Name + "-NEW-RPC-CONNECTION-BY-NAME";
            string expectedReturnPacketTypeStr = sendingPacketTypeStr + "-RESPONSE";
            instanceId = connection.SendReceiveObject<string, string>(sendingPacketTypeStr, expectedReturnPacketTypeStr, Client.RPCInitialisationTimeout, instanceName);
            if (instanceId == string.Empty)
                throw new RPCException("Named instance does not exist");
            return Client.Cache<I>.CreateInstance(instanceId, connection, options);
        }

        /// <summary>
        ///     Creates a remote proxy to an object with a specific identifier implementing the supplied interface with the
        ///     specified server
        /// </summary>
        /// <typeparam name="I">The interface to use for the proxy</typeparam>
        /// <param name="connection">The connection over which to perform remote procedure calls</param>
        /// <param name="instanceId">Unique identifier for the instance on the server</param>
        /// <param name="options">SendRecieve options to use</param>
        /// <returns>
        ///     A proxy class for the interface T allowing remote procedure calls
        /// </returns>
        public static I CreateProxyToIdInstance<I>(Connection connection, string instanceId,
            SendReceiveOptions options = null) where I : class {
            if (!typeof (I).IsInterface)
                throw new InvalidOperationException(typeof (I).Name + " is not an interface");
            var sendingPacketTypeStr = typeof (I).Name + "-NEW-RPC-CONNECTION-BY-ID";
            var expectedReturnPacketTypeStr = sendingPacketTypeStr + "-RESPONSE";
            instanceId = connection.SendReceiveObject<string, string>(sendingPacketTypeStr, expectedReturnPacketTypeStr,
                RPCInitialisationTimeout, instanceId);
            if (instanceId == string.Empty)
                throw new RPCException("Instance with given Id not found");
            return Cache<I>.CreateInstance(instanceId, connection, options);
        }

        /// <summary>
        ///     Private method for simplifying the remote procedure call.  I don't want to write this in IL!!
        /// </summary>
        /// <param name="clientObject" />
        /// <param name="functionToCall" />
        /// <param name="args" />
        /// <returns />
        public static object RemoteCallClient(IRPCProxy clientObject, string functionToCall, object[] args) {
            if (clientObject.IsDisposed)
                throw new ObjectDisposedException("clientObject",
                    "RPC object has already been disposed of and cannot be reused");
            var serverConnection = clientObject.ServerConnection;
            var sendObject = new RemoteCallWrapper();
            sendObject.args = args.Select(arg => RPCArgumentBase.CreateDynamic(arg)).ToList();
            sendObject.name = functionToCall;
            sendObject.instanceId = clientObject.ServerInstanceID;
            var shortGuid = ShortGuid.NewGuid();
            var sendingPacketTypeStr = clientObject.ImplementedInterface.Name + "-RPC-CALL-" + sendObject.instanceId;
            var expectedReturnPacketTypeStr = sendingPacketTypeStr + "-" + shortGuid;
            var sendReceiveOptions = clientObject.SendReceiveOptions;
            var remoteCallWrapper = sendReceiveOptions == null
                ? serverConnection.SendReceiveObject<RemoteCallWrapper, RemoteCallWrapper>(sendingPacketTypeStr,
                    expectedReturnPacketTypeStr, clientObject.RPCTimeout, sendObject)
                : serverConnection.SendReceiveObject<RemoteCallWrapper, RemoteCallWrapper>(sendingPacketTypeStr,
                    expectedReturnPacketTypeStr, clientObject.RPCTimeout, sendObject, sendReceiveOptions,
                    sendReceiveOptions);
            if (remoteCallWrapper.Exception != null)
                throw new RPCException(remoteCallWrapper.Exception);
            for (var index = 0; index < args.Length; ++index)
                args[index] = remoteCallWrapper.args[index].UntypedValue;
            if (remoteCallWrapper.result != null)
                return remoteCallWrapper.result.UntypedValue;
            return null;
        }

        /// <summary>
        ///     Causes the provided <see cref="T:RemoteProcedureCalls.IRPCProxy" /> instance to be disposed
        /// </summary>
        /// <param name="clientObject">The <see cref="T:RemoteProcedureCalls.IRPCProxy" /> to dispose</param>
        public static void DestroyRPCClient(IRPCProxy clientObject) {
            if (clientObject.IsDisposed)
                return;
            var serverConnection = clientObject.ServerConnection;
            var sendingPacketType = clientObject.ImplementedInterface.Name + "-REMOVE-REFERENCE-" +
                                    clientObject.ServerInstanceID;
            var objectToSend = new RemoteCallWrapper();
            objectToSend.args = new List<RPCArgumentBase>();
            objectToSend.name = null;
            objectToSend.instanceId = clientObject.ServerInstanceID;
            try {
                serverConnection.SendObject(sendingPacketType, objectToSend);
            }
            catch (Exception) {}
            try {
                serverConnection.RemoveIncomingPacketHandler(clientObject.ImplementedInterface.Name + "-RPC-LISTENER-" +
                                                             clientObject.ServerInstanceID);
            }
            catch (Exception) {}
            try {
                serverConnection.RemoveIncomingPacketHandler(clientObject.ImplementedInterface.Name + "-RPC-DISPOSE-" +
                                                             clientObject.ServerInstanceID);
            }
            catch (Exception) {}
            lock (cacheLocker) {
                var local_5 = new CachedRPCKey(clientObject.ServerInstanceID, clientObject.ServerConnection,
                    clientObject.ImplementedInterface);
                try {
                    cachedInstances.Remove(local_5);
                }
                catch (Exception) {}
            }
        }

        /// <summary>
        ///     Struct that helps store the cached RPC objects
        /// </summary>
        private struct CachedRPCKey {
            public CachedRPCKey(string instanceId, Connection connection, Type implementedInterface) {
                this = new CachedRPCKey();
                InstanceId = instanceId;
                Connection = connection;
                ImplementedInterface = implementedInterface;
            }

            public string InstanceId { get; }
            public Connection Connection { get; }
            public Type ImplementedInterface { get; }

            public override bool Equals(object obj) {
                if (obj == null || !(obj is CachedRPCKey))
                    return false;
                var cachedRpcKey = (CachedRPCKey) obj;
                if (InstanceId == cachedRpcKey.InstanceId && Connection == cachedRpcKey.Connection)
                    return ImplementedInterface == cachedRpcKey.ImplementedInterface;
                return false;
            }

            public override int GetHashCode() {
                return InstanceId.GetHashCode() ^ Connection.GetHashCode() ^ ImplementedInterface.GetHashCode();
            }
        }

        /// <summary>
        ///     Funky class used for dynamically creating the proxy
        /// </summary>
        /// <typeparam name="I" />
        private static class Cache<I> where I : class {
            private static readonly Type Type;

            static Cache() {
                if (!typeof (I).IsInterface)
                    throw new InvalidOperationException(typeof (I).Name + " is not an interface");
                var name = new AssemblyName("tmp_" + typeof (I).Name);
                var moduleBuilder =
                    AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run)
                        .DefineDynamicModule(Path.ChangeExtension(name.Name, "dll"), false);
                var @namespace = typeof (I).Namespace;
                if (!string.IsNullOrEmpty(@namespace))
                    @namespace += ".";
                var typeBuilder = moduleBuilder.DefineType(@namespace + "grp_" + typeof (I).Name, TypeAttributes.Sealed);
                typeBuilder.AddInterfaceImplementation(typeof (I));
                typeBuilder.AddInterfaceImplementation(typeof (IRPCProxy));
                var fieldBuilder1 = typeBuilder.DefineField("serverInstanceID", typeof (string), FieldAttributes.Private);
                var fieldBuilder2 = typeBuilder.DefineField("serverConnection", typeof (Connection),
                    FieldAttributes.Private);
                var fieldBuilder3 = typeBuilder.DefineField("sendReceiveOptions", typeof (SendReceiveOptions),
                    FieldAttributes.Private);
                var fieldBuilder4 = typeBuilder.DefineField("rpcTimeout", typeof (int), FieldAttributes.Private);
                var fieldBuilder5 = typeBuilder.DefineField("implementedInterface", typeof (Type),
                    FieldAttributes.Private);
                var fieldBuilder6 = typeBuilder.DefineField("isDisposed", typeof (bool), FieldAttributes.Private);
                var method1 = typeof (Client).GetMethod("RemoteCallClient", BindingFlags.Static | BindingFlags.Public);
                var method2 = typeof (Client).GetMethod("DestroyRPCClient", BindingFlags.Static | BindingFlags.Public);
                var ilGenerator1 =
                    typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[5] {
                        typeof (string),
                        typeof (Connection),
                        typeof (SendReceiveOptions),
                        typeof (Type),
                        typeof (int)
                    }).GetILGenerator();
                ilGenerator1.Emit(OpCodes.Ldarg_0);
                ilGenerator1.Emit(OpCodes.Ldarg_1);
                ilGenerator1.Emit(OpCodes.Stfld, fieldBuilder1);
                ilGenerator1.Emit(OpCodes.Ldarg_0);
                ilGenerator1.Emit(OpCodes.Ldarg_2);
                ilGenerator1.Emit(OpCodes.Stfld, fieldBuilder2);
                ilGenerator1.Emit(OpCodes.Ldarg_0);
                ilGenerator1.Emit(OpCodes.Ldarg_3);
                ilGenerator1.Emit(OpCodes.Stfld, fieldBuilder3);
                ilGenerator1.Emit(OpCodes.Ldarg_0);
                ilGenerator1.Emit(OpCodes.Ldarg_S, 4);
                ilGenerator1.Emit(OpCodes.Stfld, fieldBuilder5);
                ilGenerator1.Emit(OpCodes.Ldarg_0);
                ilGenerator1.Emit(OpCodes.Ldarg_S, 5);
                ilGenerator1.Emit(OpCodes.Stfld, fieldBuilder4);
                ilGenerator1.Emit(OpCodes.Ldarg_0);
                ilGenerator1.Emit(OpCodes.Ldc_I4_0);
                ilGenerator1.Emit(OpCodes.Stfld, fieldBuilder6);
                ilGenerator1.Emit(OpCodes.Ret);
                var attributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig |
                                 MethodAttributes.SpecialName;
                foreach (var propertyInfo in typeof (IRPCProxy).GetProperties()) {
                    var indexParameters = propertyInfo.GetIndexParameters();
                    var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, propertyInfo.Attributes,
                        propertyInfo.PropertyType, Array.ConvertAll(indexParameters, p => p.ParameterType));
                    FieldBuilder fieldBuilder7;
                    switch (propertyInfo.Name) {
                        case "ServerInstanceID":
                            fieldBuilder7 = fieldBuilder1;
                            break;
                        case "ServerConnection":
                            fieldBuilder7 = fieldBuilder2;
                            break;
                        case "RPCTimeout":
                            fieldBuilder7 = fieldBuilder4;
                            break;
                        case "ImplementedInterface":
                            fieldBuilder7 = fieldBuilder5;
                            break;
                        case "SendReceiveOptions":
                            fieldBuilder7 = fieldBuilder3;
                            break;
                        case "IsDisposed":
                            fieldBuilder7 = fieldBuilder6;
                            break;
                        default:
                            throw new RPCException("Error initialising IRPCClient property");
                    }
                    if (propertyInfo.CanRead) {
                        var mdBuilder = typeBuilder.DefineMethod("get_" + propertyInfo.Name, attributes,
                            propertyInfo.PropertyType, Array.ConvertAll(indexParameters, p => p.ParameterType));
                        var ilGenerator2 = mdBuilder.GetILGenerator();
                        ilGenerator2.Emit(OpCodes.Ldarg_0);
                        ilGenerator2.Emit(OpCodes.Ldfld, fieldBuilder7);
                        ilGenerator2.Emit(OpCodes.Ret);
                        propertyBuilder.SetGetMethod(mdBuilder);
                    }
                    if (propertyInfo.CanWrite) {
                        var list = indexParameters.Select(a => a.ParameterType).ToList();
                        list.Add(propertyInfo.PropertyType);
                        var mdBuilder = typeBuilder.DefineMethod("set_" + propertyInfo.Name, attributes, typeof (void),
                            list.ToArray());
                        var ilGenerator2 = mdBuilder.GetILGenerator();
                        ilGenerator2.Emit(OpCodes.Ldarg_0);
                        ilGenerator2.Emit(OpCodes.Ldarg_1);
                        ilGenerator2.Emit(OpCodes.Stfld, fieldBuilder7);
                        ilGenerator2.Emit(OpCodes.Ret);
                        propertyBuilder.SetSetMethod(mdBuilder);
                    }
                }
                foreach (var methodInfoDeclaration in typeof (IDisposable).GetMethods()) {
                    var parameters = methodInfoDeclaration.GetParameters();
                    var methodBuilder = typeBuilder.DefineMethod(methodInfoDeclaration.Name,
                        MethodAttributes.Public | MethodAttributes.Virtual, methodInfoDeclaration.ReturnType,
                        Array.ConvertAll(parameters, arg => arg.ParameterType));
                    typeBuilder.DefineMethodOverride(methodBuilder, methodInfoDeclaration);
                    var ilGenerator2 = methodBuilder.GetILGenerator();
                    ilGenerator2.Emit(OpCodes.Ldarg_0);
                    ilGenerator2.EmitCall(OpCodes.Call, method2, null);
                    ilGenerator2.Emit(OpCodes.Ldarg_0);
                    ilGenerator2.Emit(OpCodes.Ldc_I4_1);
                    ilGenerator2.Emit(OpCodes.Stfld, fieldBuilder6);
                    ilGenerator2.Emit(OpCodes.Ret);
                }
                foreach (
                    var methodInfoDeclaration in
                        typeof (I).GetMethods()
                            .Where(m => (m.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope)) {
                    var parameters = methodInfoDeclaration.GetParameters();
                    var methodBuilder = typeBuilder.DefineMethod(methodInfoDeclaration.Name,
                        MethodAttributes.Public | MethodAttributes.Virtual, methodInfoDeclaration.ReturnType,
                        Array.ConvertAll(parameters, arg => arg.ParameterType));
                    typeBuilder.DefineMethodOverride(methodBuilder, methodInfoDeclaration);
                    var ilGenerator2 = methodBuilder.GetILGenerator();
                    var local1 = ilGenerator2.DeclareLocal(typeof (object[]));
                    ilGenerator2.Emit(OpCodes.Ldc_I4_S, parameters.Length);
                    ilGenerator2.Emit(OpCodes.Newarr, typeof (object));
                    ilGenerator2.Emit(OpCodes.Stloc, local1);
                    var local2 = ilGenerator2.DeclareLocal(typeof (object));
                    for (var index = 0; index < parameters.Length; ++index) {
                        ilGenerator2.Emit(OpCodes.Ldarg, index + 1);
                        if (parameters[index].ParameterType.IsByRef) {
                            ilGenerator2.Emit(OpCodes.Ldind_Ref);
                            if (parameters[index].ParameterType.GetElementType().IsValueType)
                                ilGenerator2.Emit(OpCodes.Box, parameters[index].ParameterType.GetElementType());
                        }
                        if (parameters[index].ParameterType.IsValueType)
                            ilGenerator2.Emit(OpCodes.Box, parameters[index].ParameterType);
                        ilGenerator2.Emit(OpCodes.Castclass, typeof (object));
                        ilGenerator2.Emit(OpCodes.Stloc, local2);
                        ilGenerator2.Emit(OpCodes.Ldloc, local1);
                        ilGenerator2.Emit(OpCodes.Ldc_I4_S, index);
                        ilGenerator2.Emit(OpCodes.Ldloc, local2);
                        ilGenerator2.Emit(OpCodes.Stelem_Ref);
                    }
                    ilGenerator2.Emit(OpCodes.Ldarg_0);
                    ilGenerator2.Emit(OpCodes.Ldstr, methodInfoDeclaration.Name);
                    ilGenerator2.Emit(OpCodes.Ldloc, local1);
                    ilGenerator2.EmitCall(OpCodes.Call, method1, null);
                    if (methodInfoDeclaration.ReturnType.IsValueType &&
                        methodInfoDeclaration.ReturnType != typeof (void))
                        ilGenerator2.Emit(OpCodes.Unbox_Any, methodInfoDeclaration.ReturnType);
                    if (methodInfoDeclaration.ReturnType == typeof (void))
                        ilGenerator2.Emit(OpCodes.Pop);
                    if (!methodInfoDeclaration.ReturnType.IsValueType)
                        ilGenerator2.Emit(OpCodes.Castclass, methodInfoDeclaration.ReturnType);
                    for (var index = 0; index < parameters.Length; ++index)
                        if (parameters[index].ParameterType.IsByRef) {
                            ilGenerator2.Emit(OpCodes.Ldarg, index + 1);
                            ilGenerator2.Emit(OpCodes.Ldloc, local1);
                            ilGenerator2.Emit(OpCodes.Ldc_I4_S, index);
                            ilGenerator2.Emit(OpCodes.Ldelem, typeof (object));
                            ilGenerator2.Emit(OpCodes.Castclass, parameters[index].ParameterType.GetElementType());
                            if (parameters[index].ParameterType.GetElementType().IsValueType)
                                ilGenerator2.Emit(OpCodes.Unbox_Any, parameters[index].ParameterType.GetElementType());
                            ilGenerator2.Emit(OpCodes.Stobj, parameters[index].ParameterType.GetElementType());
                        }
                    ilGenerator2.Emit(OpCodes.Ret);
                }
                foreach (var propertyInfo in typeof (I).GetProperties()) {
                    var indexParameters = propertyInfo.GetIndexParameters();
                    var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, propertyInfo.Attributes,
                        propertyInfo.PropertyType, Array.ConvertAll(indexParameters, p => p.ParameterType));
                    if (propertyInfo.CanRead) {
                        var mdBuilder = typeBuilder.DefineMethod("get_" + propertyInfo.Name, attributes,
                            propertyInfo.PropertyType, Array.ConvertAll(indexParameters, p => p.ParameterType));
                        var ilGenerator2 = mdBuilder.GetILGenerator();
                        var local1 = ilGenerator2.DeclareLocal(typeof (object[]));
                        ilGenerator2.Emit(OpCodes.Ldc_I4_S, indexParameters.Length);
                        ilGenerator2.Emit(OpCodes.Newarr, typeof (object));
                        ilGenerator2.Emit(OpCodes.Stloc, local1);
                        var local2 = ilGenerator2.DeclareLocal(typeof (object));
                        for (var index = 0; index < indexParameters.Length; ++index) {
                            ilGenerator2.Emit(OpCodes.Ldarg, index + 1);
                            if (indexParameters[index].ParameterType.IsValueType)
                                ilGenerator2.Emit(OpCodes.Box, indexParameters[index].ParameterType);
                            ilGenerator2.Emit(OpCodes.Castclass, typeof (object));
                            ilGenerator2.Emit(OpCodes.Stloc, local2);
                            ilGenerator2.Emit(OpCodes.Ldloc, local1);
                            ilGenerator2.Emit(OpCodes.Ldc_I4_S, index);
                            ilGenerator2.Emit(OpCodes.Ldloc, local2);
                            ilGenerator2.Emit(OpCodes.Stelem_Ref);
                        }
                        ilGenerator2.Emit(OpCodes.Ldarg_0);
                        ilGenerator2.Emit(OpCodes.Ldstr, mdBuilder.Name);
                        ilGenerator2.Emit(OpCodes.Ldloc, local1);
                        ilGenerator2.EmitCall(OpCodes.Call, method1, null);
                        if (mdBuilder.ReturnType.IsValueType)
                            ilGenerator2.Emit(OpCodes.Unbox_Any, mdBuilder.ReturnType);
                        if (!mdBuilder.ReturnType.IsValueType)
                            ilGenerator2.Emit(OpCodes.Castclass, mdBuilder.ReturnType);
                        ilGenerator2.Emit(OpCodes.Ret);
                        propertyBuilder.SetGetMethod(mdBuilder);
                    }
                    if (propertyInfo.CanWrite) {
                        var list = indexParameters.Select(a => a.ParameterType).ToList();
                        list.Add(propertyInfo.PropertyType);
                        var mdBuilder = typeBuilder.DefineMethod("set_" + propertyInfo.Name, attributes, typeof (void),
                            list.ToArray());
                        var ilGenerator2 = mdBuilder.GetILGenerator();
                        var local1 = ilGenerator2.DeclareLocal(typeof (object[]));
                        ilGenerator2.Emit(OpCodes.Ldc_I4_S, list.Count);
                        ilGenerator2.Emit(OpCodes.Newarr, typeof (object));
                        ilGenerator2.Emit(OpCodes.Stloc, local1);
                        var local2 = ilGenerator2.DeclareLocal(typeof (object));
                        for (var index = 0; index < list.Count; ++index) {
                            ilGenerator2.Emit(OpCodes.Ldarg, index + 1);
                            if (list[index].IsValueType)
                                ilGenerator2.Emit(OpCodes.Box, list[index]);
                            ilGenerator2.Emit(OpCodes.Castclass, typeof (object));
                            ilGenerator2.Emit(OpCodes.Stloc, local2);
                            ilGenerator2.Emit(OpCodes.Ldloc, local1);
                            ilGenerator2.Emit(OpCodes.Ldc_I4_S, index);
                            ilGenerator2.Emit(OpCodes.Ldloc, local2);
                            ilGenerator2.Emit(OpCodes.Stelem_Ref);
                        }
                        ilGenerator2.Emit(OpCodes.Ldarg_0);
                        ilGenerator2.Emit(OpCodes.Ldstr, mdBuilder.Name);
                        ilGenerator2.Emit(OpCodes.Ldloc, local1);
                        ilGenerator2.EmitCall(OpCodes.Call, method1, null);
                        ilGenerator2.Emit(OpCodes.Pop);
                        ilGenerator2.Emit(OpCodes.Ret);
                        propertyBuilder.SetSetMethod(mdBuilder);
                    }
                }
                if (typeof (I).GetEvents().Count() != 0)
                    foreach (var eventInfo in typeof (I).GetEvents()) {
                        var eventBuilder = typeBuilder.DefineEvent(eventInfo.Name, eventInfo.Attributes,
                            eventInfo.EventHandlerType);
                        var fieldBuilder7 = typeBuilder.DefineField(eventInfo.Name, eventInfo.EventHandlerType,
                            FieldAttributes.Private);
                        var method3 = typeof (Delegate).GetMethod("Combine", new Type[2] {
                            typeof (Delegate),
                            typeof (Delegate)
                        });
                        typeof (Delegate).GetMethod("Remove", new Type[2] {
                            typeof (Delegate),
                            typeof (Delegate)
                        });
                        var meth = typeof (Interlocked).GetMethods().Where(info => {
                            if (info.Name == "CompareExchange")
                                return info.IsGenericMethod;
                            return false;
                        }).First().MakeGenericMethod(eventInfo.EventHandlerType);
                        var mdBuilder1 = typeBuilder.DefineMethod("add_" + eventInfo.Name, attributes, null,
                            new Type[1] {
                                eventInfo.EventHandlerType
                            });
                        mdBuilder1.DefineParameter(0, ParameterAttributes.Retval, null);
                        mdBuilder1.DefineParameter(1, ParameterAttributes.In, "value");
                        eventBuilder.SetAddOnMethod(mdBuilder1);
                        var ilGenerator2 = mdBuilder1.GetILGenerator();
                        ilGenerator2.DeclareLocal(eventInfo.EventHandlerType);
                        ilGenerator2.DeclareLocal(eventInfo.EventHandlerType);
                        ilGenerator2.DeclareLocal(eventInfo.EventHandlerType);
                        ilGenerator2.DeclareLocal(typeof (bool));
                        var label1 = ilGenerator2.DefineLabel();
                        ilGenerator2.Emit(OpCodes.Ldarg_0);
                        ilGenerator2.Emit(OpCodes.Ldfld, fieldBuilder7);
                        ilGenerator2.Emit(OpCodes.Stloc_0);
                        ilGenerator2.EmitWriteLine("Built");
                        ilGenerator2.MarkLabel(label1);
                        ilGenerator2.Emit(OpCodes.Ldloc_0);
                        ilGenerator2.Emit(OpCodes.Stloc_1);
                        ilGenerator2.Emit(OpCodes.Ldloc_1);
                        ilGenerator2.Emit(OpCodes.Ldarg_1);
                        ilGenerator2.Emit(OpCodes.Call, method3);
                        ilGenerator2.Emit(OpCodes.Castclass, eventInfo.EventHandlerType);
                        ilGenerator2.Emit(OpCodes.Stloc_2);
                        ilGenerator2.Emit(OpCodes.Ldarg_0);
                        ilGenerator2.Emit(OpCodes.Ldflda, fieldBuilder7);
                        ilGenerator2.Emit(OpCodes.Ldloc_2);
                        ilGenerator2.Emit(OpCodes.Ldloc_1);
                        ilGenerator2.Emit(OpCodes.Call, meth);
                        ilGenerator2.Emit(OpCodes.Stloc_0);
                        ilGenerator2.Emit(OpCodes.Ldloc_0);
                        ilGenerator2.Emit(OpCodes.Ldloc_1);
                        ilGenerator2.Emit(OpCodes.Ceq);
                        ilGenerator2.Emit(OpCodes.Ldc_I4_0);
                        ilGenerator2.Emit(OpCodes.Ceq);
                        ilGenerator2.Emit(OpCodes.Stloc_3);
                        ilGenerator2.Emit(OpCodes.Ldloc_3);
                        ilGenerator2.Emit(OpCodes.Brtrue_S, label1);
                        ilGenerator2.Emit(OpCodes.Ret);
                        var mdBuilder2 = typeBuilder.DefineMethod("remove_" + eventInfo.Name, attributes, null,
                            new Type[1] {
                                eventInfo.EventHandlerType
                            });
                        mdBuilder2.DefineParameter(0, ParameterAttributes.Retval, null);
                        mdBuilder2.DefineParameter(1, ParameterAttributes.In, "value");
                        eventBuilder.SetRemoveOnMethod(mdBuilder2);
                        var ilGenerator3 = mdBuilder2.GetILGenerator();
                        ilGenerator3.DeclareLocal(eventInfo.EventHandlerType);
                        ilGenerator3.DeclareLocal(eventInfo.EventHandlerType);
                        ilGenerator3.DeclareLocal(eventInfo.EventHandlerType);
                        ilGenerator3.DeclareLocal(typeof (bool));
                        var label2 = ilGenerator3.DefineLabel();
                        ilGenerator3.Emit(OpCodes.Ldarg_0);
                        ilGenerator3.Emit(OpCodes.Ldfld, fieldBuilder7);
                        ilGenerator3.Emit(OpCodes.Stloc_0);
                        ilGenerator3.EmitWriteLine("Built");
                        ilGenerator3.MarkLabel(label2);
                        ilGenerator3.Emit(OpCodes.Ldloc_0);
                        ilGenerator3.Emit(OpCodes.Stloc_1);
                        ilGenerator3.Emit(OpCodes.Ldloc_1);
                        ilGenerator3.Emit(OpCodes.Ldarg_1);
                        ilGenerator3.Emit(OpCodes.Call, method3);
                        ilGenerator3.Emit(OpCodes.Castclass, eventInfo.EventHandlerType);
                        ilGenerator3.Emit(OpCodes.Stloc_2);
                        ilGenerator3.Emit(OpCodes.Ldarg_0);
                        ilGenerator3.Emit(OpCodes.Ldflda, fieldBuilder7);
                        ilGenerator3.Emit(OpCodes.Ldloc_2);
                        ilGenerator3.Emit(OpCodes.Ldloc_1);
                        ilGenerator3.Emit(OpCodes.Call, meth);
                        ilGenerator3.Emit(OpCodes.Stloc_0);
                        ilGenerator3.Emit(OpCodes.Ldloc_0);
                        ilGenerator3.Emit(OpCodes.Ldloc_1);
                        ilGenerator3.Emit(OpCodes.Ceq);
                        ilGenerator3.Emit(OpCodes.Ldc_I4_0);
                        ilGenerator3.Emit(OpCodes.Ceq);
                        ilGenerator3.Emit(OpCodes.Stloc_3);
                        ilGenerator3.Emit(OpCodes.Ldloc_3);
                        ilGenerator3.Emit(OpCodes.Brtrue_S, label2);
                        ilGenerator3.Emit(OpCodes.Ret);
                    }
                Type = typeBuilder.CreateType();
            }

            public static I CreateInstance(string instanceId, Connection connection, SendReceiveOptions options) {
                lock (cacheLocker) {
                    var local_3 = new CachedRPCKey(instanceId, connection, typeof (I));
                    if (cachedInstances.ContainsKey(local_3))
                        return (I) cachedInstances[local_3];
                    var res =
                        (I)
                            Activator.CreateInstance(Type, (object) instanceId, (object) connection, (object) options,
                                (object) typeof (I), (object) DefaultRPCTimeout);
                    var eventFields = new Dictionary<string, FieldInfo>();
                    foreach (var item_0 in typeof (I).GetEvents())
                        eventFields.Add(item_0.Name,
                            Type.GetField(item_0.Name, BindingFlags.Instance | BindingFlags.NonPublic));
                    connection.AppendIncomingPacketHandler(typeof (I).Name + "-RPC-LISTENER-" + instanceId,
                        (NetworkComms.PacketHandlerCallBackDelegate<RemoteCallWrapper>)
                            ((header, internalConnection, eventCallWrapper) => {
                                try {
                                    if (eventCallWrapper == null || !eventFields.ContainsKey(eventCallWrapper.name))
                                        return;
                                    (eventFields[eventCallWrapper.name].GetValue(res) as Delegate).DynamicInvoke(
                                        eventCallWrapper.args[0].UntypedValue, eventCallWrapper.args[1].UntypedValue);
                                }
                                catch (Exception) {}
                            }));
                    connection.AppendIncomingPacketHandler(typeof (I).Name + "-RPC-DISPOSE-" + instanceId,
                        (NetworkComms.PacketHandlerCallBackDelegate<string>)
                            ((header, internalConnection, eventCallWrapper) => ((object) res as IRPCProxy).Dispose()));
                    cachedInstances[local_3] = res;
                    return res;
                }
            }
        }
    }
}