using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient.Memcached;
using NetworkCommsDotNet;

namespace nucs.Network {
    public delegate void ClientApprovalRequestedHandler(Connection conn, string clientName, BooleanHolder ToApprove);
    public delegate void ClientApprovedHandler(Connection conn, string clientName);
    public delegate void ClientDisconnectedHandler(Connection conn, string clientName, DateTime time);
    public delegate string NameRequesterHandler();

    /// <summary>
    /// An extension to NetComms, every app has default methods to identify by set of rules.
    /// </summary>
    public static class NetCommsExt {

        #region Setuppers

        public static string Name {
            get {
                if (_nameGetter == null)
                    return _name;
                return _nameGetter();
            }
        }

        private static NameRequesterHandler _nameGetter;
        private static string _name;

        /// <summary>
        /// This should be enabled in case of multiple Application.Run(form) calls, make sure you disable it when it is no longer required.
        /// </summary>
        public static bool EnableCrossApplication {
            set {
                if (value == false)
                    Application.ApplicationExit += ShutDownUponApplicationExit;
                else 
                    Application.ApplicationExit -= ShutDownUponApplicationExit;

            }
        }

        #region Server

        public static event ClientDisconnectedHandler ClientDisconnected;
        public static event ClientApprovalRequestedHandler ConnectionApprovalRequested;
        public static event ClientApprovedHandler ClientApproved;
        /// <summary>
        /// The clients that were connected to this server application.
        /// </summary>
        public static Dictionary<string, Connection> Clients;  
        public static void SetupServer(string serverName) {
            if (!(_name == null && _nameGetter == null))
                throw new InvalidOperationException("Server or client was already established on this application");
            _name = serverName;
            Clients = new Dictionary<string, Connection>();
            NetworkComms.AppendGlobalIncomingPacketHandler("Identify", new NetworkComms.PacketHandlerCallBackDelegate<string>((header, connection1, incomingObject) => connection1.SendObject("Identity", _name)));
            Application.ApplicationExit += ShutDownUponApplicationExit;
            NetworkComms.AppendGlobalConnectionEstablishHandler(ConnectionEstablishDelegate);
            NetworkComms.AppendGlobalConnectionCloseHandler(ConnectionShutdownDelegate);
        }

        public static void SetupServer(NameRequesterHandler nameGetter) {
            if (!(_name == null && _nameGetter == null))
                throw new InvalidOperationException("Server or client was already established on this application");
            _nameGetter = nameGetter;
            Clients = new Dictionary<string, Connection>();
            NetworkComms.AppendGlobalIncomingPacketHandler("Identify", new NetworkComms.PacketHandlerCallBackDelegate<string>((header, connection1, incomingObject) => connection1.SendObject("Identity", _nameGetter())));
            Application.ApplicationExit += ShutDownUponApplicationExit;
            NetworkComms.AppendGlobalConnectionEstablishHandler(ConnectionEstablishDelegate);
            NetworkComms.AppendGlobalConnectionCloseHandler(ConnectionShutdownDelegate);
        }

        private static void ConnectionEstablishDelegate(Connection connection) {
            var id = connection.RequestIdentity();
            if (string.IsNullOrEmpty(id) || Clients.ContainsKey(id))
                return;
            
            var holder = new BooleanHolder(true);
            
            if (ConnectionApprovalRequested != null)
                ConnectionApprovalRequested(connection, id, holder);

            if (holder == false) { //if disproved
                connection.CloseConnection(false);
                return;
            }
            
            if (Clients.ContainsKey(id) == false) //if already contains
                Clients.Add(id, connection);

            if (ClientApproved != null)
                ClientApproved(connection, id);
        }

        private static void ConnectionShutdownDelegate(Connection connection) {
            KeyValuePair<string, Connection> found;
            if ((found = Clients.FirstOrDefault(kv => kv.Value.Equals(connection))).Key != null) {
                Clients.Remove(found.Key);
                if (ClientDisconnected != null)
                    ClientDisconnected(connection, found.Key, DateTime.Now);
            }
        }
        #endregion

        #region Client

        /// <summary>
        /// Sets up the props and methods for a client.
        /// </summary>
        public static void SetupClient(string clientName) {
            if (!(_name == null && _nameGetter == null))
                throw new InvalidOperationException("Server or client was already established on this application");
            _name = clientName;
            NetworkComms.AppendGlobalIncomingPacketHandler("Identify", new NetworkComms.PacketHandlerCallBackDelegate<string>((header, connection1, incomingObject) => connection1.SendObject("Identity", _name)));
            Application.ApplicationExit += ShutDownUponApplicationExit;
        }
        /// <summary>
        /// Sets up the props and methods for a client.
        /// </summary>
        public static void SetupClient(NameRequesterHandler clientNameGetter) {
            if (!(_name == null && _nameGetter == null))
                throw new InvalidOperationException("Server or client was already established on this application");
            _nameGetter = clientNameGetter;
            NetworkComms.AppendGlobalIncomingPacketHandler("Identify", new NetworkComms.PacketHandlerCallBackDelegate<string>((header, connection1, incomingObject) => connection1.SendObject("Identity", _nameGetter())));
            Application.ApplicationExit += ShutDownUponApplicationExit;
        }
        #endregion

        private static void ShutDownUponApplicationExit(object sender, EventArgs eventArgs) {
            NetworkComms.Shutdown();
        }
        #endregion

        #region Extensions

        /// <summary>
        /// Create a <see cref="T:NetworkCommsDotNet.TCPConnection"/> with the provided connectionInfo. If there is an existing connection that will be returned instead.
        ///             If a new connection is created it will be registered with NetworkComms and can be retreived using <see cref="M:NetworkCommsDotNet.NetworkComms.GetExistingConnection"/> and overrides.
        /// 
        /// </summary>
        /// <returns>
        /// Returns a <see cref="T:NetworkCommsDotNet.TCPConnection"/>
        /// </returns>
        public static TCPConnection GetConnection(string ip, int port, bool establishIfRequired = true) {
            return TCPConnection.GetConnection(new ConnectionInfo(ip, port), establishIfRequired);
        }

        /// <summary>
        /// Accept new incoming TCP connections on specified <see cref="T:System.Net.IPEndPoint"/>
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="useRandomPortFailOver">NetworkComms figues out other port instead of requested if it is blocked</param>
        /// <param name="UseAllPossibleIPs">Use all IP Addresses from 'Dns.GetHostAddresses' method, else => get the first from it</param>
        public static void StartListening(string ip, int port, bool useRandomPortFailOver = true, bool UseAllPossibleIPs = false) {
            var _ip = Dns.GetHostAddresses(ip);
            var first = _ip.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork);
            if (UseAllPossibleIPs)
                TCPConnection.StartListening(_ip.Select(p => new IPEndPoint(p, port)).ToList(), useRandomPortFailOver);
            else 
                TCPConnection.StartListening(new IPEndPoint(first, port), useRandomPortFailOver);
        }

        public static string RequestIdentity(this Connection connection, int waitFor = 1500) {
            return connection.SendReceiveObject<string>("Identify", "Identity", waitFor, "");
        }

        public static string RequestIdentity(this TCPConnection connection, int waitFor = 1500) {
            return connection.SendReceiveObject<string>("Identify", "Identity", waitFor, "");
        }

        /// <summary>
        /// Disconnects a client from this server based on it's name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Was the disconnection successful</returns>
        public static bool DisconnectClient(string name) {
            if (_name == null && _nameGetter == null)
                throw new InvalidOperationException("Server or client was already established on this application");
            if (Clients == null)
                throw new InvalidOperationException("Server was never constructed on this application.");
            if (Clients.Count == 0 || Clients.ContainsKey(name))
                return false;
            try {
                Clients[name].CloseConnection(false);
            } catch {} //silent catching
            return true;

        }

        /// <summary>
        /// Disconnects all currently connected clients to this server.
        /// </summary>
        /// <returns>How many clients were disconnected</returns>
        public static int DisconnectAllClients() {
            if (_name == null && _nameGetter == null)
                throw new InvalidOperationException("Server or client was already established on this application");
            if (Clients == null)
                throw new InvalidOperationException("Server was never constructed on this application.");
            if (Clients.Count == 0)
                return 0;
            var n = Clients.Count;
            NetworkComms.CloseAllConnections(ConnectionType.TCP, Clients.Select(c=>c.Value.ConnectionInfo.LocalEndPoint).ToArray());
            return n;
        }

        #endregion
    }
}
