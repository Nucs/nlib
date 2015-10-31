/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using nucs.Annotations;
using nucs.SystemCore;
using nucs.SystemCore.String;

namespace nucs.Network {
    public delegate void OnConnectionRequestedHandler(ServerSideClient slave, Bool approve, StringBuilder reason);
    public delegate void OnConnectionApproved(ServerSideClient slave);
    public delegate void SSMessageArrivedHandler(ServerSideClient cli, Message msg);
    public delegate void SSRawMessageArrivedHandler(ServerSideClient cli, string msg);
    /// <summary>
    /// A server based TCP Communication with a custom handshake implementation and event based approval system
    /// </summary>
    public class Server {
        #region Events
        public event OnConnectionRequestedHandler OnConnectionRequested;
        public event OnConnectionApproved OnConnectionApproved;

        public event SSRawMessageArrivedHandler RawMessageArrived;
        public event SSMessageArrivedHandler MessageArrived;
        #endregion

        #region Constructor and Properties
        
        /// <summary>
        /// Returns the listener socket of this server.
        /// </summary>
        public Socket Listener { get; private set; }
        /// <summary>
        /// Clients that are currently connected to the server.
        /// </summary>
        public readonly List<ServerSideClient> Clients = new List<ServerSideClient>();
        /// <summary>
        /// Returns the currectly active port for this server.
        /// </summary>
        public ushort Port { get; private set; }
        /// <summary>
        /// A representive name for the server.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Unique Id for the server
        /// </summary>
        public Guid Guid { get { return _guid; } }

        private readonly Guid _guid = Guid.NewGuid();

        /// <summary>
        /// A server based TCP Communication with a custom handshake implementation and event based approval system
        /// </summary>
        public Server(string serverName, ushort port, bool autoStart = false) {
            Port = port;
            Name = serverName;
            if (autoStart) Listen(Port);
        }

        #endregion

        protected virtual void Listen(int port) {
            try {
                new SocketPermission(NetworkAccess.Accept, //Allowed to accept connections
                    TransportType.Tcp, //Defines transport types
                    "", port // Specifies all ports 
                    ).Demand();
            } catch (Exception e) {
                Console.Out.WriteLine("Error in requesting permission to listen for port "+port+"\n"+e);
                throw;
            }
            
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //todo consider if Listener.Bind is needed
            Listener.Bind(new IPEndPoint(IPAddress.Any, port));
            Listener.Listen(10);

            BindEvents();

        }
        
        protected virtual void BindEvents() {
            Listener.BeginAccept(_BeginAccept_Callback, null);
        }

        /// <summary>
        /// Handles a connection request, handshake performing and client approval.
        /// </summary>
        private void _BeginAccept_Callback(IAsyncResult ar) {
            var socket_client= Listener.EndAccept(ar); //socket of client
            
            var args = new AsyncEventArgs(socket_client);
            
            var rec = socket_client.Receive(args.Buffer, args.Length, 0);
            if (rec == 0)
                return;
            var details = Encoding.UTF8.GetString(args.Buffer).BetweenClause("hs").AsStrings();
            var ss_client = new ServerSideClient(socket_client) { ClientName = details[0], ClientGuid = Guid.Parse(details[1]) };
            var approved = (Bool)true;
            var reason = new StringBuilder();
            if (OnConnectionRequested != null)
                OnConnectionRequested(ss_client, approved, reason);
            if (approved == false) {
                ss_client.Disconnect(reason.ToString());
                return;
            } //todo more delecate disconnect with reason and so on.
            SendTo(ss_client, "<hs>" + Name + Constants.ShortSplitString + Guid + "</hs>");
            Clients.Add(ss_client);
            if (OnConnectionApproved != null)
                OnConnectionApproved(ss_client);

            Listener.BeginAccept(_BeginAccept_Callback, null);
        }
        
        //todo receive bits center, with mapping of the socket and so on

        #region Messaging
        /*public void Broadcast(string msg) {//todo fix broadcast and test
            if (started == false)
                throw new InvalidOperationException("Cannot send a message because server wasn't started yet.");
            if (Slaves.Count == 0) { Console.WriteLine("Attempted to broadcast message: \"" + msg + "\" but there are no slaves connected"); return; }
            lock (lock_send) {
                try {
                    msg += Constants.Delimiter;
                    var bytes = Encoding.UTF8.GetBytes(msg);

                    Slaves.ToList().AsParallel().ForAll(s =>
                    {
                        if (!s.CommonSocket.Poll(-1, SelectMode.SelectWrite))
                            DisconnectSocket(s.CommonSocket, "Poll of SelectWrite has failed.");
                        var args = AsyncEventArgs.Create(s.CommonSocket, bytes.Clone() as byte[]);
                        if (s.CommonSocket.Connected)
                            s.CommonSocket.BeginSend(bytes, 0, bytes.Length, 0, SendCallback, AsyncEventArgs.Create(s.CommonSocket, bytes));
                    });
                    
                    Console.WriteLine("Message was broadcasted: "+msg);
                } catch (Exception exc) {
                    Console.WriteLine(exc.ToString());
                }
            }
        }

        public void Broadcast(Message msg) {
            Broadcast(msg.ToString());
        }#1#

        /// <summary>
        /// Sends asynchronously a string using the given socket which reprensents a socket of a client. 
        /// <remarks>The string is encoded using UTF8</remarks>
        /// </summary>
        /// <param name="towards">Client's socket</param>
        /// <param name="text">The message to be sent</param>
        /// <returns>Was sent successfully</returns>
        public bool SendTo(Socket towards, string text) {
            if (towards.Connected == false) return false;
            if (!towards.Poll(-1, SelectMode.SelectWrite)) {
                /*DisconnectSocket(towards, "Poll of SelectWrite has failed.");#1# return false;
            } //todo allow disconnect
            try {
                var bytes = Encoding.UTF8.GetBytes(text + Constants.Delimiter);
                towards.BeginSend(bytes, 0, bytes.Length, 0, SendCallback, AsyncEventArgs.Create(towards, bytes));
            }
            catch { Console.WriteLine("Failed sending " + text); return false; }
            return true;
        }
        /// <summary>
        /// Sends asynchronously a string using the given socket which reprensents a socket of a client. 
        /// <remarks>The string is encoded using UTF8</remarks>
        /// </summary>
        /// <param name="towards">Client's socket</param>
        /// <param name="text">The message to be sent</param>
        /// <returns>Was sent successfully</returns>
        public void SendTo(ServerSideClient towards, string text) {
            SendTo(towards.Socket, text);
        }
        /// <summary>
        /// Sends asynchronously a string using the given socket which reprensents a socket of a client. 
        /// <remarks>The string is encoded using UTF8</remarks>
        /// </summary>
        /// <param name="towards">Client's socket</param>
        /// <param name="msg">The message to be sent</param>
        /// <returns>Was sent successfully</returns>
        public void SendTo(ServerSideClient towards, Message msg) {
            SendTo(towards, msg.ToString());
        }
        /// <summary>
        /// Sends asynchronously a string using the given socket which reprensents a socket of a client. 
        /// <remarks>The string is encoded using UTF8</remarks>
        /// </summary>
        /// <param name="towards">Client's socket</param>
        /// <param name="msg">The message to be sent</param>
        /// <returns>Was sent successfully</returns>
        public void SendTo(Socket towards, Message msg) {
            SendTo(towards, msg.ToString());
        }

        private void SendCallback(IAsyncResult ar) {
            try {
                var args = (AsyncEventArgs) ar.AsyncState;
                if (args.Socket != null)
                    args.Socket.EndSend(ar);
            } catch (ObjectDisposedException exc) { //incase of disposed socket.
            } catch (Exception exc) {
                Console.WriteLine(exc.ToString()); //todo proper exception, if socket is disposed or null then skip end.
            }
        }
        #endregion
        
        #region Object Overrides

        public override int GetHashCode() {
            return Port ^ Guid.ToString().Sum(c => (int) c) ^ Name.Sum(c => (int) c) ^ Clients.Count;
        }

        public override string ToString() {
            return "server:" + Name + "://" + "127.0.0.1:" + Port+"/?Connected="+Clients.Count;
        }

        public bool Equals(Server obj) {
            return obj.GetHashCode().Equals(GetHashCode());
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is Server) return Equals(obj as Server);
            return base.Equals(obj);
        }

        #endregion
    }
}

*/
