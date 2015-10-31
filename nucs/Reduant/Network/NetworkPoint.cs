/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace nucs.Network {
    public abstract class NetworkPoint {
        protected Socket _socket;
        protected Socket _listener;
        protected IPEndPoint ipEndPoint;
        public readonly Guid Guid = Guid.NewGuid();
        protected string Host { get; set; }
        protected int port { get; set; }




        public void DemandPermissions(SocketPermission permission) {
            permission.Demand();
        }

        public void DemandPermissions() {
            new SocketPermission(PermissionState.Unrestricted).Demand();
            /*permission = new SocketPermission(
                NetworkAccess.Accept /*Allowed to accept connections#2#,
                TransportType.Tcp, /*Defines transport types#2#
                "",
                port // Specifies all ports 
                );
            //todo see this permission, maybe reenable it and disable top 1
            sListener = null;
            permission.Demand();#1#
        }




    }
}
*/
