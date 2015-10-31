// Decompiled with JetBrains decompiler
// Type: RemoteProcedureCalls.RPCArgumentBase
// Assembly: NetworkCommsDotNetComplete, Version=3.0.0.0, Culture=neutral, PublicKeyToken=f58108eb6480f6ec
// MVID: D81C90D5-119C-4F53-86B6-4A32F7B5925E
// Assembly location: F:\__Development\C#\TFer\Libs\NetworkComms\DLLs\Net40\Merged\NetworkCommsDotNetComplete.dll

using System;
using ProtoBuf;

namespace RemoteProcedureCalls {
    /// <summary>
    ///     Cheeky base class used in order to allow us to send an array of objects using Protobuf-net
    /// </summary>
    [ProtoContract]
    internal abstract class RPCArgumentBase {
        public abstract object UntypedValue { get; set; }

        public static RPCArgument<T> Create<T>(T value) {
            return new RPCArgument<T> {
                Value = value
            };
        }

        public static RPCArgumentBase CreateDynamic(object value) {
            if (value == null)
                return null;
            var rpcArgumentBase =
                (RPCArgumentBase) Activator.CreateInstance(typeof (RPCArgument<>).MakeGenericType(value.GetType()));
            rpcArgumentBase.UntypedValue = value;
            return rpcArgumentBase;
        }
    }
}