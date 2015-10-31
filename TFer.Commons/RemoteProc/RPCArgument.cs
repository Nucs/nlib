// Decompiled with JetBrains decompiler
// Type: RemoteProcedureCalls.RPCArgument`1
// Assembly: NetworkCommsDotNetComplete, Version=3.0.0.0, Culture=neutral, PublicKeyToken=f58108eb6480f6ec
// MVID: D81C90D5-119C-4F53-86B6-4A32F7B5925E
// Assembly location: F:\__Development\C#\TFer\Libs\NetworkComms\DLLs\Net40\Merged\NetworkCommsDotNetComplete.dll

using ProtoBuf;

namespace RemoteProcedureCalls {
    /// <summary>
    ///     Cheeky derived class used in order to allow us to send an array of objects using Protobuf-net
    /// </summary>
    [ProtoContract]
    internal sealed class RPCArgument<T> : RPCArgumentBase {
        [ProtoMember(1)]
        public T Value { get; set; }

        public override object UntypedValue
        {
            get { return Value; }
            set { Value = (T) value; }
        }
    }
}