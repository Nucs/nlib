// Decompiled with JetBrains decompiler
// Type: RemoteProcedureCalls.RemoteCallWrapper
// Assembly: NetworkCommsDotNetComplete, Version=3.0.0.0, Culture=neutral, PublicKeyToken=f58108eb6480f6ec
// MVID: D81C90D5-119C-4F53-86B6-4A32F7B5925E
// Assembly location: F:\__Development\C#\TFer\Libs\NetworkComms\DLLs\Net40\Merged\NetworkCommsDotNetComplete.dll

using System.Collections.Generic;
using ProtoBuf;

namespace RemoteProcedureCalls {
    /// <summary>
    ///     Wrapper class used for serialisation when running functions remotely
    /// </summary>
    [ProtoContract]
    internal class RemoteCallWrapper {
        [ProtoMember(3, DynamicType = true)] public List<RPCArgumentBase> args;

        [ProtoMember(5)] public string Exception;

        [ProtoMember(1)] public string instanceId;

        [ProtoMember(2)] public string name;

        [ProtoMember(4, DynamicType = true)] public RPCArgumentBase result;
    }
}