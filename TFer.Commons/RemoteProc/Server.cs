// Decompiled with JetBrains decompiler
// Type: RemoteProcedureCalls.Server
// Assembly: NetworkCommsDotNetComplete, Version=3.0.0.0, Culture=neutral, PublicKeyToken=f58108eb6480f6ec
// MVID: D81C90D5-119C-4F53-86B6-4A32F7B5925E
// Assembly location: F:\__Development\C#\TFer\Libs\NetworkComms\DLLs\Net40\Merged\NetworkCommsDotNetComplete.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using nucs.Mono.System.Threading;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;

namespace RemoteProcedureCalls {
    /// <summary>
    ///     Contains methods for managing objects server side which allow Remote Procedure Calls
    /// </summary>
    public static class Server {
        private static readonly object locker = new object();

        private static readonly Dictionary<string, RPCRemoteObject> RPCObjectsById =
            new Dictionary<string, RPCRemoteObject>();

        private static readonly Dictionary<Type, int> timeoutByInterfaceType = new Dictionary<Type, int>();
        private static Dictionary<Type, Delegate> newConnectionByNewInstanceHandlers = new Dictionary<Type, Delegate>();
        private static Dictionary<Type, Delegate> newConnectionByNameHandlers = new Dictionary<Type, Delegate>();
        private static Dictionary<Type, Delegate> newConnectionByIdHandlers = new Dictionary<Type, Delegate>();
        private static readonly byte[] salt;
        private static readonly HashAlgorithm hash;

        static Server() {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            salt = new byte[32];
            randomNumberGenerator.GetBytes(salt);
            hash = HMAC.Create();
            var watcherWaitEvent = new AutoResetEvent(false);
            var watcher = Task.Run((() => {
                do {
                    lock (locker) {
                        var local_2 = new List<string>();
                        foreach (var item_0 in RPCObjectsById)
                            if (item_0.Value.Type == RPCRemoteObject.RPCObjectType.Private &&
                                item_0.Value.SubscribedClients.Count == 0 &&
                                (DateTime.Now - item_0.Value.LastAccess).TotalMilliseconds > item_0.Value.TimeOut)
                                local_2.Add(item_0.Key);
                        if (local_2.Count != 0)
                            RemoveRPCObjects(local_2);
                    }
                } while (!watcherWaitEvent.WaitOne(5000));
                ShutdownAllRPC();
            }));
            NetworkComms.OnCommsShutdown += (EventHandler<EventArgs>) ((sender, args) => {
                watcherWaitEvent.Set();
                watcher.Wait();
            });
        }

        /// <summary>
        ///     Helper method for calculating instance ids
        /// </summary>
        /// <param name="input" />
        /// <returns />
        private static string GetInstanceId(string input) {
            return Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(input).Union(salt).ToArray()));
        }

        /// <summary>
        ///     Registers a type for private RPC whereby each client generates it's own private instances on the server
        /// </summary>
        /// <typeparam name="T">The type of object to create new instances of for RPC.  Must implement I</typeparam>
        /// <typeparam name="I">Interface that should be provided for RPC</typeparam>
        /// <param name="timeout">
        ///     If specified each RPC object created will be destroyed if it is unused for a time, in ms,
        ///     specified by timeout
        /// </param>
        public static void RegisterTypeForPrivateRemoteCall<T, I>(int timeout = 2147483647) where T : I, new() {
            lock (locker) {
                if (!typeof (I).IsInterface)
                    throw new InvalidOperationException(typeof (I).Name + " is not an interface");
                if (newConnectionByNewInstanceHandlers.ContainsKey(typeof (I)))
                    throw new RPCException("Interface already has a type associated with it for new instance RPC");
                NetworkComms.PacketHandlerCallBackDelegate<string> local_2 = NewInstanceRPCHandler<T, I>;
                timeoutByInterfaceType.Add(typeof (I), timeout);
                NetworkComms.AppendGlobalIncomingPacketHandler(typeof (I).Name + "-NEW-INSTANCE-RPC-CONNECTION", local_2);
                newConnectionByNewInstanceHandlers.Add(typeof (I), local_2);
            }
        }

        /// <summary>
        ///     Registers a specfic object instance, with the supplied name, for RPC
        /// </summary>
        /// <typeparam name="T">The type of the object to register. Must implement I</typeparam>
        /// <typeparam name="I">The interface to be provided for RPC</typeparam>
        /// <param name="instance">Instance to register for RPC</param>
        /// <param name="instanceName">Name of the instance to be used by clients for RPC</param>
        public static void RegisterInstanceForPublicRemoteCall<T, I>(T instance, string instanceName) where T : I {
            lock (locker) {
                if (!typeof (I).IsInterface)
                    throw new InvalidOperationException(typeof (I).Name + " is not an interface");
                var local_2 = GetInstanceId(typeof (T).Name + instanceName);
                if (!RPCObjectsById.ContainsKey(local_2))
                    RPCObjectsById.Add(local_2,
                        new RPCRemoteObject(instance, typeof (I), RPCRemoteObject.RPCObjectType.Public, local_2,
                            int.MaxValue));
                if (!newConnectionByNameHandlers.ContainsKey(typeof (I))) {
                    NetworkComms.PacketHandlerCallBackDelegate<string> local_3 = RetrieveNamedRPCHandler<T, I>;
                    NetworkComms.AppendGlobalIncomingPacketHandler(typeof (I).Name + "-NEW-RPC-CONNECTION-BY-NAME",
                        local_3);
                    newConnectionByNameHandlers.Add(typeof (I), local_3);
                }
                if (newConnectionByIdHandlers.ContainsKey(typeof (I)))
                    return;
                NetworkComms.PacketHandlerCallBackDelegate<string> local_4 = RetrieveByIDRPCHandler<T, I>;
                NetworkComms.AppendGlobalIncomingPacketHandler(typeof (I).Name + "-NEW-RPC-CONNECTION-BY-ID", local_4);
                newConnectionByIdHandlers.Add(typeof (I), local_4);
            }
        }

        /// <summary>
        ///     Removes all private RPC objects for the specified interface type.  Stops listenning for new RPC instance
        ///     connections
        /// </summary>
        /// <typeparam name="T">Object type that implements the specified interface I</typeparam>
        /// <typeparam name="I">Interface that is being implemented for RPC calls</typeparam>
        public static void RemovePrivateRPCObjectType<T, I>() where T : I, new() {
            lock (locker) {
                if (timeoutByInterfaceType.ContainsKey(typeof (I)))
                    timeoutByInterfaceType.Remove(typeof (I));
                RemoveRPCObjects(RPCObjectsById.Where(obj => {
                    if (obj.Value.InterfaceType == typeof (I))
                        return obj.Value.Type == RPCRemoteObject.RPCObjectType.Private;
                    return false;
                }).Select(obj => obj.Key).ToList());
                newConnectionByNewInstanceHandlers.Remove(typeof (I));
            }
        }

        /// <summary>
        ///     Disables RPC calls for the supplied named public object supplied
        /// </summary>
        /// <param name="instance">Instance to disable RPC for</param>
        public static void RemovePublicRPCObject(object instance) {
            lock (locker) {
                var local_4 = RPCObjectsById.Where(obj => {
                    if (obj.Value.RPCObject == instance)
                        return obj.Value.Type == RPCRemoteObject.RPCObjectType.Public;
                    return false;
                }).ToList();
                RemoveRPCObjects(local_4.Select(obj => obj.Key).ToList());
                var local_6 = local_4.Select(obj => obj.Value.InterfaceType).Distinct().ToList();
                foreach (
                    var item_0 in
                        local_6.Except(
                            RPCObjectsById.Where(rpcObj => rpcObj.Value.Type == RPCRemoteObject.RPCObjectType.Public)
                                .Select(rpcObj => rpcObj.Value.InterfaceType)).ToList())
                    newConnectionByNameHandlers.Remove(item_0);
                foreach (
                    var item_1 in
                        local_6.Except(RPCObjectsById.Select(rpcObj => rpcObj.Value.InterfaceType).Distinct()).ToList())
                    newConnectionByIdHandlers.Remove(item_1);
            }
        }

        /// <summary>
        ///     Removes all public and private RPC objects and removes all related packet handlers from NetworkComms
        /// </summary>
        public static void ShutdownAllRPC() {
            lock (locker) {
                RemoveRPCObjects(RPCObjectsById.Keys.ToList());
                foreach (var item_0 in newConnectionByNewInstanceHandlers)
                    NetworkComms.RemoveGlobalIncomingPacketHandler(item_0.Key.Name + "-NEW-INSTANCE-RPC-CONNECTION",
                        item_0.Value as NetworkComms.PacketHandlerCallBackDelegate<string>);
                foreach (var item_1 in newConnectionByNameHandlers)
                    NetworkComms.RemoveGlobalIncomingPacketHandler(item_1.Key.Name + "-NEW-RPC-CONNECTION-BY-NAME",
                        item_1.Value as NetworkComms.PacketHandlerCallBackDelegate<string>);
                foreach (var item_2 in newConnectionByIdHandlers)
                    NetworkComms.RemoveGlobalIncomingPacketHandler(item_2.Key.Name + "-NEW-RPC-CONNECTION-BY-ID",
                        item_2.Value as NetworkComms.PacketHandlerCallBackDelegate<string>);
                newConnectionByNewInstanceHandlers = new Dictionary<Type, Delegate>();
                newConnectionByNameHandlers = new Dictionary<Type, Delegate>();
                newConnectionByIdHandlers = new Dictionary<Type, Delegate>();
            }
        }

        private static void RemoveRPCObjects(List<string> keysToRemove) {
            lock (locker) {
                for (var local_2 = 0; local_2 < keysToRemove.Count; ++local_2) {
                    RPCObjectsById[keysToRemove[local_2]].RemoveAllClientSubscriptions();
                    RPCObjectsById.Remove(keysToRemove[local_2]);
                }
            }
        }

        private static void NewInstanceRPCHandler<T, I>(PacketHeader header, Connection connection, string instanceName)
            where T : I, new() {
            lock (locker) {
                var local_3 = GetInstanceId(typeof (T).Name + instanceName + connection.ConnectionInfo.NetworkIdentifier);
                if (!RPCObjectsById.ContainsKey(local_3)) {
                    var local_7 = new RPCRemoteObject(new T(), typeof (I), RPCRemoteObject.RPCObjectType.Private,
                        local_3, timeoutByInterfaceType[typeof (I)]);
                    local_7.AddClientSubscription<T, I>(connection);
                    RPCObjectsById.Add(local_3, local_7);
                }
                if (!newConnectionByIdHandlers.ContainsKey(typeof (I))) {
                    NetworkComms.PacketHandlerCallBackDelegate<string> local_8 = RetrieveByIDRPCHandler<T, I>;
                    NetworkComms.AppendGlobalIncomingPacketHandler(typeof (I).Name + "-NEW-RPC-CONNECTION-BY-ID",
                        local_8);
                    newConnectionByIdHandlers.Add(typeof (I), local_8);
                }
                var local_9 = header.GetOption(PacketHeaderStringItems.RequestedReturnPacketType);
                connection.SendObject(local_9, local_3);
            }
        }

        private static void RetrieveNamedRPCHandler<T, I>(PacketHeader header, Connection connection,
            string instanceName) where T : I {
            lock (locker) {
                var local_2 = GetInstanceId(typeof (T).Name + instanceName);
                if (!RPCObjectsById.ContainsKey(local_2))
                    local_2 = string.Empty;
                else
                    RPCObjectsById[local_2].AddClientSubscription<T, I>(connection);
                var local_4 = header.GetOption(PacketHeaderStringItems.RequestedReturnPacketType);
                connection.SendObject(local_4, local_2);
            }
        }

        private static void RetrieveByIDRPCHandler<T, I>(PacketHeader header, Connection connection, string instanceId)
            where T : I {
            lock (locker) {
                if (!RPCObjectsById.ContainsKey(instanceId) || RPCObjectsById[instanceId].InterfaceType != typeof (I))
                    instanceId = string.Empty;
                else
                    RPCObjectsById[instanceId].AddClientSubscription<T, I>(connection);
                var local_3 = header.GetOption(PacketHeaderStringItems.RequestedReturnPacketType);
                connection.SendObject(local_3, instanceId);
            }
        }

        private static void RunRPCFunctionHandler<T, I>(PacketHeader header, Connection connection,
            RemoteCallWrapper wrapper) where T : I {
            var i = default(I);
            var method = typeof (I).GetMethod(wrapper.name);
            try {
                lock (locker)
                    i = (I) RPCObjectsById[wrapper.instanceId].RPCObject;
            }
            catch (Exception ex) {
                wrapper.result = null;
                wrapper.Exception = "SERVER SIDE EXCEPTION\n\nInvalid instanceID\n\nEND SERVER SIDE EXCEPTION\n\n";
                connection.SendObject(header.PacketType, wrapper);
                return;
            }
            var parameters = wrapper.args != null
                ? wrapper.args.Select(arg => arg.UntypedValue).ToArray()
                : new object[0];
            try {
                wrapper.result = RPCArgumentBase.CreateDynamic(method.Invoke(i, parameters));
                wrapper.args = parameters.Select(arg => RPCArgumentBase.CreateDynamic(arg)).ToList();
            }
            catch (Exception ex) {
                var exception = ex;
                wrapper.result = null;
                if (exception.InnerException != null)
                    exception = exception.InnerException;
                wrapper.Exception = "SERVER SIDE EXCEPTION\n\n" + exception + "\n\nEND SERVER SIDE EXCEPTION\n\n";
            }
            var option = header.GetOption(PacketHeaderStringItems.RequestedReturnPacketType);
            connection.SendObject(option, wrapper);
        }

        private static void RPCDisconnectHandler<T, I>(PacketHeader header, Connection connection,
            RemoteCallWrapper wrapper) where T : I {
            lock (locker) {
                if (!RPCObjectsById.ContainsKey(wrapper.instanceId))
                    return;
                RPCObjectsById[wrapper.instanceId].RemoveClientSubscription(connection);
            }
        }

        private class RPCClientSubscription {
            public RPCClientSubscription(Connection connection,
                NetworkComms.PacketHandlerCallBackDelegate<RemoteCallWrapper> callFunctionDelegate,
                NetworkComms.PacketHandlerCallBackDelegate<RemoteCallWrapper> removeDelegate) {
                Connection = connection;
                SubscribedEvents = new Dictionary<EventInfo, Delegate>();
                CallFunctionDelegate = callFunctionDelegate;
                RemoveDelegate = removeDelegate;
            }

            public Connection Connection { get; }
            public Dictionary<EventInfo, Delegate> SubscribedEvents { get; }
            public NetworkComms.PacketHandlerCallBackDelegate<RemoteCallWrapper> CallFunctionDelegate { get; }
            public NetworkComms.PacketHandlerCallBackDelegate<RemoteCallWrapper> RemoveDelegate { get; }
        }

        private class RPCRemoteObject {
            public enum RPCObjectType {
                Public,
                Private
            }

            private readonly object obj;

            public RPCRemoteObject(object RPCObject, Type interfaceType, RPCObjectType Type, string instanceId,
                int timeout = 2147483647) {
                TimeOut = timeout;
                obj = RPCObject;
                LastAccess = DateTime.Now;
                InterfaceType = interfaceType;
                this.Type = Type;
                InstanceId = instanceId;
                SubscribedClients = new Dictionary<ShortGuid, RPCClientSubscription>();
            }

            public object RPCObject
            {
                get
                {
                    LastAccess = DateTime.Now;
                    return obj;
                }
            }

            public string InstanceId { get; }
            public Type InterfaceType { get; }
            public DateTime LastAccess { get; private set; }
            public int TimeOut { get; }
            public RPCObjectType Type { get; }
            public Dictionary<ShortGuid, RPCClientSubscription> SubscribedClients { get; }

            public void AddClientSubscription<T, I>(Connection connection) where T : I {
                if (SubscribedClients.ContainsKey(connection.ConnectionInfo.NetworkIdentifier))
                    return;
                var events = InterfaceType.GetEvents();
                var dictionary = new Dictionary<EventInfo, Delegate>();
                foreach (var key in events) {
                    var addMethod = key.GetAddMethod();
                    var obj =
                        typeof (RPCRemoteObject).GetMethod("GenerateEvent", BindingFlags.Static | BindingFlags.NonPublic)
                            .MakeGenericMethod(key.EventHandlerType.GetGenericArguments())
                            .Invoke(null, new object[4] {
                                connection,
                                InstanceId,
                                InterfaceType,
                                key.Name
                            });
                    addMethod.Invoke(this.obj, new object[1] {
                        obj
                    });
                    dictionary.Add(key, obj as Delegate);
                }
                NetworkComms.PacketHandlerCallBackDelegate<RemoteCallWrapper> callBackDelegate1 =
                    RunRPCFunctionHandler<T, I>;
                NetworkComms.PacketHandlerCallBackDelegate<RemoteCallWrapper> callBackDelegate2 =
                    RPCDisconnectHandler<T, I>;
                var clientSubscription = new RPCClientSubscription(connection, callBackDelegate1, callBackDelegate2);
                foreach (var keyValuePair in dictionary)
                    clientSubscription.SubscribedEvents.Add(keyValuePair.Key, keyValuePair.Value);
                SubscribedClients.Add(connection.ConnectionInfo.NetworkIdentifier, clientSubscription);
                connection.AppendIncomingPacketHandler(InterfaceType.Name + "-RPC-CALL-" + InstanceId, callBackDelegate1);
                connection.AppendIncomingPacketHandler(InterfaceType.Name + "-REMOVE-REFERENCE-" + InstanceId,
                    callBackDelegate2);
                LastAccess = DateTime.Now;
                connection.AppendShutdownHandler(clientConnection => RemoveClientSubscription(clientConnection));
            }

            public void RemoveClientSubscription(Connection connection) {
                if (!SubscribedClients.ContainsKey(connection.ConnectionInfo.NetworkIdentifier))
                    return;
                var clientSubscription = SubscribedClients[connection.ConnectionInfo.NetworkIdentifier];
                foreach (var keyValuePair in clientSubscription.SubscribedEvents)
                    keyValuePair.Key.GetRemoveMethod().Invoke(obj, new object[1] {
                        keyValuePair.Value
                    });
                try {
                    clientSubscription.Connection.SendObject(InterfaceType.Name + "-RPC-DISPOSE-" + InstanceId, "");
                }
                catch (Exception) {}
                finally {
                    LastAccess = DateTime.Now;
                    SubscribedClients.Remove(connection.ConnectionInfo.NetworkIdentifier);
                    connection.RemoveIncomingPacketHandler(InterfaceType.Name + "-RPC-CALL-" + InstanceId,
                        clientSubscription.CallFunctionDelegate);
                    connection.RemoveIncomingPacketHandler(InterfaceType.Name + "-REMOVE-REFERENCE-" + InstanceId,
                        clientSubscription.RemoveDelegate);
                }
            }

            public void RemoveAllClientSubscriptions() {
                foreach (var connection in SubscribedClients.Values.Select(client => client.Connection).ToArray())
                    RemoveClientSubscription(connection);
            }

            private static EventHandler<A> GenerateEvent<A>(Connection clientConnection, string instanceId,
                Type interfaceType, string eventName) where A : EventArgs {
                return (sender, args) => {
                    var sendingPacketType = interfaceType.Name + "-RPC-LISTENER-" + instanceId;
                    clientConnection.SendObject(sendingPacketType, new RemoteCallWrapper {
                        name = eventName,
                        instanceId = instanceId,
                        args = new List<RPCArgumentBase> {
                            RPCArgumentBase.CreateDynamic(sender),
                            RPCArgumentBase.CreateDynamic(args)
                        }
                    });
                };
            }
        }
    }
}