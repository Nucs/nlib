using System;
using nucs.Network.Discovery;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;

namespace nucs.Network.RPC {
    public static class NodeExtension {

        public static void RemoteExecute(this Node to, string code) {
            if (Node.This == to)
                throw new InvalidOperationException("Can't send data to self.");

            TCPConnection.GetConnection(new ConnectionInfo(to.IP, Ports.DiscoveryPort)).Execute(code);
        }
    }
}