/*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using nucs.Annotations;
using nucs.SystemCore.String;

namespace nucs.Network {
    public class Client {
        public delegate void MessageArrivedHandler(Message msg);
        public delegate void RawMessageArrivedHandler(string msg);
        public delegate void DisconnectionHandler(string reason);
        public event DisconnectionHandler Disconnected;
        public event RawMessageArrivedHandler RawMessageArrived;
        public event MessageArrivedHandler MessageArrived;

        #region Constructor and Properties
        /// <summary>
        /// The socket between this client and the server
        /// </summary>
        public Socket Socket { get; set; }
        /// <summary>
        /// Hostname of the server, could be IP or address
        /// </summary>
        public string Host { get; private set; }
        /// <summary>
        /// The port of the communication between server and client
        /// </summary>
        public ushort Port { get; private set; }
        /// <summary>
        /// A representive name for the client. It will be sent to the server and can be used to identify specific system partitions.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Unique Guid for each client for comparing.
        /// </summary>
        public Guid Guid {
            get { return _guid; }
        }

        /// <summary>
        /// Tests if the socket is alive using Socket.Poll
        /// </summary>
        public bool Connected { get {
            return Socket != null && Socket.Connected;
        } }

        #region Server Details
        public Guid ServerGuid { get; private set; }
        public string ServerName { get; private set; }
        #endregion

        private byte _emptyMessagesCount = 0;
        private readonly Guid _guid = Guid.NewGuid();

        public Client([NotNull] string clientName, [NotNull] string host, ushort port, bool AutoConnect = false) {
            Name = clientName;
            Host = host;
            Port = port;
            if (AutoConnect) Connect();
        }
        #endregion

        public bool Connect() {
            #region Resolving and Connecting
            try {
                if (Host.CompareAny("127.0.0.1", "::1")) Host = "localhost"; //fix for local usage
                var permission = new SocketPermission(
                    NetworkAccess.Connect, // Connection permission 
                    TransportType.Tcp, // Defines transport types 
                    Host, // Gets the IP addresses 
                    Port // All ports 
                );

                // Ensures the code to have permission to access a Socket 
                permission.Demand();
                var addresses = Dns.GetHostAddresses(Host);
                foreach (var ip in addresses.Where(add=>add.AddressFamily != AddressFamily.InterNetworkV6)) { 
                    if (ip.AddressFamily == AddressFamily.InterNetworkV6) continue;
                    try {
                        var ipEndPoint = new IPEndPoint(ip, Port);
                        // Create one Socket object to setup Tcp connection 
                        Socket = new Socket(
                            ip.AddressFamily, // Specifies the addressing scheme 
                            SocketType.Stream, // The type of socket  
                            ProtocolType.Tcp // Specifies the protocols  
                            ) {NoDelay = false};
                        // Establishes a connection to a remote host 
                        Socket.Connect(ipEndPoint);
                        
                        goto _connected;
                    } catch (Exception e) {
                        continue;
                    }
                }
            } catch (Exception e) { }
            return false;

            #endregion
            #region Post-Connect Successful
        _connected:
            
            #region Handshake

            try {
                var args = new AsyncEventArgs(Socket);
                Socket.ReceiveTimeout = 10000;
                Socket.SendTimeout = 10000;
                Socket.Send(Encoding.UTF8.GetBytes("<hs>" + Name + Constants.ShortSplitString + Guid + "</hs>"));
                    //sends details
                int rec = -1;
                rec = Socket.Receive(args.Buffer);
                    //suppose to receive <hs>ServerName + Constants.ShortSplitString + ServerGuid</hs>
                if (rec == 0)
                    throw new ConnectionException("Failed performing handshake with server " + ToString()); //bug rec == 0 might also be disproval of client
                var msg = Encoding.UTF8.GetString(args.Buffer);
                if (msg.Contains("<disconnect>"))
                    throw new ServerDisporvalException(msg.BetweenClause("disconnect"));
                var received = msg.BetweenClause("hs").AsStrings();
                ServerName = received[0];
                ServerGuid = Guid.Parse(received[1]);
                Socket.ReceiveTimeout = 0;
                Socket.SendTimeout = 0;
            } catch (Exception e) {
                if (e is ServerDisporvalException)
                    throw;
                throw new ConnectionException("Failed performing handshake with server " + ToString() + "\n" + e);
            }
            #endregion
            var _args = new AsyncEventArgs(Socket);
            Socket.BeginReceive(_args.Buffer, 0, _args.Length, 0, _ReceiveCallback, _args);
            #endregion
            Console.WriteLine("Socket connected to " + Socket.RemoteEndPoint + ". Ready to communicate");
            return true;
        }

        private void _ReceiveCallback(IAsyncResult ar) {
            var args = (AsyncEventArgs) ar.AsyncState;
            var received = 0;
            if (Socket == null)
                received += args.Buffer.TakeWhile(b => b != 0).Count(); //figure out the lenght when socket is already disposed
            else 
                received = Socket.EndReceive(ar);

            var content = Encoding.UTF8.GetString(args.Buffer);
            args.Buffer = new byte[args.Length];
            if (Socket != null)
                Socket.BeginReceive(args.Buffer, 0, args.Length, 0, _ReceiveCallback, args); //immediatly begins to listen again

            if (received > 0) {
                var messages = content.StringsTill(Constants.Delimiter);
                foreach (var i in messages) {
                    var i1 = i; //internal copy
                    //internal catagorizing
                    if (i.IsBetweenClause("disconnect")) { //forcing disconnection
                        Disconnect(i.BetweenClause("disconnect"));
                        continue;
                    }
                    //end internal catagorizing

                    if (MessageArrived != null && i.IsBetweenClause("m"))
                        Task.Run(() => MessageArrived(Message.Translate(i1)));
                    else if (RawMessageArrived != null)
                        Task.Run(() => RawMessageArrived(i1));

                }
            } else { //possibility that the server performed a disconnection. but to make sure, wait for atleast 5 of those messages.
                if (Socket == null)
                    return;
                _emptyMessagesCount++;
                if (_emptyMessagesCount >= 5) {
                    Disconnect("Connection with server has been lost.");
                }
            }
        }
        
        public bool SendMessage(string msg) {
            if (Connected == false)
                return false;
            try {
                var args = AsyncEventArgs.Create(Socket, Encoding.UTF8.GetBytes(msg + Constants.Delimiter));
                Socket.BeginSend(args.Buffer, 0, args.Length, 0, ar => Socket.EndSend(ar), args);
                return true;
            } catch (Exception exc) { Console.WriteLine(exc.ToString()); return false; }
        }

        public void SendMessage(Message msg) {
            SendMessage(msg.ToString());
        }
        
        public void Disconnect(string reason = "") {
            // Disables sends and receives on a Socket. 
            Socket.Shutdown(SocketShutdown.Both);
            //Closes the Socket connection and releases all resources 
            Socket.Close(1000);
            Socket = null;
            if (Disconnected != null)
                Disconnected(reason == "" ? null : reason);
        }

        #region Object overrides
        public bool Equals(Client obj) {
            return Guid.Equals(obj.Guid);
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is Client) return Equals((Client) obj);
            return obj.Equals(this);
        }

        public override int GetHashCode() {
            return Guid.ToString().Sum(c => (int)c) ^ Port ^ (Name ?? "").Sum(c => (int)c) ^ (Host ?? "").Sum(c => (int)c);
        }

        public override string ToString() {
            return string.Format("client:{0}://{1}:{2}", this.Name, Host, Port);
        }
        #endregion
    }
}

*/
