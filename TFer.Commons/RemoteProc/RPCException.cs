// Decompiled with JetBrains decompiler
// Type: RemoteProcedureCalls.RPCException
// Assembly: NetworkCommsDotNetComplete, Version=3.0.0.0, Culture=neutral, PublicKeyToken=f58108eb6480f6ec
// MVID: D81C90D5-119C-4F53-86B6-4A32F7B5925E
// Assembly location: F:\__Development\C#\TFer\Libs\NetworkComms\DLLs\Net40\Merged\NetworkCommsDotNetComplete.dll

using NetworkCommsDotNet;

namespace RemoteProcedureCalls {
    /// <summary>
    ///     An error occured during an RPC (Remote Procedure Call) exchange.
    /// </summary>
    public class RPCException : CommsException {
        /// <summary>
        ///     Create a new instance of RPCException
        /// </summary>
        /// <param name="msg">A string containing useful information regarding the error</param>
        public RPCException(string msg)
            : base(msg) {}
    }
}