// Decompiled with JetBrains decompiler
// Type: RemoteProcedureCalls.IRPCProxy
// Assembly: NetworkCommsDotNetComplete, Version=3.0.0.0, Culture=neutral, PublicKeyToken=f58108eb6480f6ec
// MVID: D81C90D5-119C-4F53-86B6-4A32F7B5925E
// Assembly location: F:\__Development\C#\TFer\Libs\NetworkComms\DLLs\Net40\Merged\NetworkCommsDotNetComplete.dll

using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace RemoteProcedureCalls {
    /// <summary>
    ///     Interface for the RPC proxy generated on the client side. All RPC objects returned from Client.CreateRPCProxyTo{X}
    ///     implement this interface
    /// </summary>
    public interface IRPCProxy : IDisposable {
        /// <summary>
        ///     The interface the proxy implements
        /// </summary>
        Type ImplementedInterface { get; }

        /// <summary>
        ///     The server generated object id for the remote instance
        /// </summary>
        string ServerInstanceID { get; }

        /// <summary>
        ///     The NetworkComms.Net connection associated wth the proxy
        /// </summary>
        Connection ServerConnection { get; }

        /// <summary>
        ///     The send receive options used when communicating with the server
        /// </summary>
        SendReceiveOptions SendReceiveOptions { get; set; }

        /// <summary>
        ///     The timeout for all RPC calls made with this proxy in ms
        /// </summary>
        int RPCTimeout { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:RemoteProcedureCalls.IRPCProxy" /> has been disposed of
        /// </summary>
        bool IsDisposed { get; }
    }
}