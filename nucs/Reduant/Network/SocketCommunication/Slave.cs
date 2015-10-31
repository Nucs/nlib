/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using nucs.Network.SocketCommunication;
using nucs.SocketCommunication.Commons;
using nucs.SystemCore.String;
using nucs.Utils;

namespace nucs.SocketCommunication {
    public class Slave : IDisposable {
        #region Init And Constructors
        public readonly Guid GUID = Guid.NewGuid();
        public ServerSideSlave CreateServerSideModel { get { var s = new ServerSideSlave(Name, GUID, serverSocket); if (Connected == false) s.Approved = false; return s; } }
        public Guid ServerGUID { get; private set; }
        public bool Connected { get {
            if (serverSocket == null) return false;
            var result = false;
            try {
                result = serverSocket.Poll(5, SelectMode.SelectWrite);
            }
            catch {}
            return result;
        } }
        public string Address { get; private set; }
        public int Port { get; private set; }
        public string Name { get; private set; }
        public event OnMessageArrivalHandler OnMessageArrival;
        public event OnTextArrivalHandler OnTextArrival;
        public event OnMasterDisconnectedHandler OnDisconnect;
        public event OnReconnectionSuccessfull OnConnectedSuccessfuly;
        public event OnMasterDisconnectedHandler OnReconnectionFail;
        public event OnMasterForcedDisconnect OnConnectionDisproved;
        private Socket serverSocket;
        private readonly object lock_send = new object();
        private readonly object lock_read = new object();
        
        public Slave(string name, string address, int port) {
            OnMessageArrival += AutoResponder;
            OnTextArrival += args => { };
            OnDisconnect += (at, reconnect) => { };
            OnConnectedSuccessfuly += (at) => { };
            OnReconnectionFail += (at, reconnect) => { };
            OnConnectionDisproved += (at, reas) => { };
            Name = name;
            Address = address;
            Port = port;
        }
        public Slave(string name, string address, int port, bool AttemptConnectNowOnce) : this(name, address, port) {
            if (AttemptConnectNowOnce)
                connect(address, port);
        }
        public Slave(string name, string address, int port, bool AttemptConnectNowFor, int milliseconds) : this(name, address, port) {
            if (AttemptConnectNowFor) {
                AttemptConnect(milliseconds);
            }
        }
        public Slave(string name, IPEndPoint eip) : this(name, eip.Address.ToString(), eip.Port) {}
        public Slave(string name, IPEndPoint eip, bool AttemptConnectNow) : this(name, eip) {
            if (AttemptConnectNow)
                connect(Address, Port);
        }
        #endregion

        #region Functions and Commands
        public bool Connect() {
            return Connected || connect(Address, Port);
        }

        private readonly AutoResetEvent approvalreset = new AutoResetEvent(false);
        /// <summary>
        /// Attempts to connect to the target as long as timeoutMilliseconds didn't pass.
        /// </summary>
        /// <returns>Connection Successfuly or not.</returns>
        public bool AttemptConnect(int timeoutMilliseconds) {
            if (serverSocket != null && Connected)
                return true;
            
            var token = new CancellationTokenSource();
            var task = Task.Factory.StartNew(() => {
                token.CancelAfter(timeoutMilliseconds);     
                while (connect(Address, Port) == false) 
                    if (token.IsCancellationRequested) return false;
                return true;
                }, token.Token);
            Task.WaitAny(new Task[] { task });
            if (task.Result == false)
                return false;
            if (approvalreset.WaitOne(0))
                goto _skip;
            if (!approvalreset.WaitOne(10000))
                return false;
            _skip:
            return true;
        }

        private bool connect(string address, int port) {
            try {
                if (address.CompareAny("127.0.0.1", "::1")) address = "localhost";
                //if (string.Compare(address, "127.0.0.1"))
                // Create one SocketPermission for socket access restrictions 
                var permission = new SocketPermission(
                    NetworkAccess.Connect, // Connection permission 
                    TransportType.Tcp, // Defines transport types 
                    address, // Gets the IP addresses 
                    port // All ports 
                    );

                // Ensures the code to have permission to access a Socket 
                permission.Demand();
                var ipHost = Dns.GetHostEntry(address);
                foreach (var ip in ipHost.AddressList) {
                    try {
                        var ipEndPoint = new IPEndPoint(ip, port);
                        // Create one Socket object to setup Tcp connection 
                        serverSocket = new Socket(
                            ip.AddressFamily, // Specifies the addressing scheme 
                            SocketType.Stream, // The type of socket  
                            ProtocolType.Tcp // Specifies the protocols  
                            ) {NoDelay = false};

                        // Establishes a connection to a remote host 
                        serverSocket.Connect(ipEndPoint);
                        var args = AsyncEventArgs.Create(serverSocket);

                        Console.WriteLine("Socket connected to " + serverSocket.RemoteEndPoint.ToString() +
                                          ". Ready to communicate");

                        serverSocket.BeginReceive(args.Buffer, 0, args.Length, 0, ReceiveCallback, args);
                        return true;
                    } catch (Exception e) {
                        continue;
                    }
                }
            } catch (Exception e) {}
            return false;
        }

        [DebuggerStepThrough]
        public bool SendMessage(string msg) {
            if (_handshaked == false && msg.Contains("hs"+Constants.Split_ClassText) == false) {
                toSendList.Add(msg);
                return true;
            }
            if (!Connected)
                return false;
            lock (lock_send) {
                try {
                    var args = AsyncEventArgs.Create(serverSocket, Encoding.UTF8.GetBytes(msg + Constants.Delimiter));
                    serverSocket.BeginSend(args.Buffer, 0, args.Length, 0, SendCallback, args);
                    return true;
                }
                catch (Exception exc) { Console.WriteLine(exc.ToString()); return false; }
            }
        }

        [DebuggerStepThrough]
        public void SendMessage(Message msg) {
            SendMessage(msg.ToString());
        }
        
        internal void SendCallback(IAsyncResult ar) {
            try {
                var args = (AsyncEventArgs) ar.AsyncState;
                args.Socket.EndSend(ar);
            } catch (Exception exc) {
                Console.WriteLine(exc.ToString());
            }
        }

        private int antispamerror = 0;
        internal void ReceiveCallback(IAsyncResult ar) {
            try {
                var args = (AsyncEventArgs) ar.AsyncState;
                string content = string.Empty;
                var bytesRead = args.Socket.EndReceive(ar);
                if (bytesRead > 0) {
                    content += Encoding.UTF8.GetString(args.Buffer, 0, bytesRead);
                    var a = content.StringsTill(Constants.Delimiter);
                    foreach (var i in a) {
                        OnTextArrival(OnTextArrivalArgs.Create(args.Socket, i));
                        var b = i.StringsBetween(Constants.StartMessage, Constants.EndMessage);
                        foreach (var msg in b) {
                            OnMessageArrivalArgs margs;
                            if ((margs = OnMessageArrivalArgs.TryCreate(args.Socket, Constants.StartMessage + msg + Constants.EndMessage)) != null) {
                                Task.Run(() => OnMessageArrivalInvoke(margs));
                            }
                        }
                    }
                } else {
                    antispamerror++;
                    if (antispamerror == 5) {
                        Disconnect("Was not approved by master!");
                        return;
                    }
                }
                var obj = AsyncEventArgs.Create(args.Socket);
                args.Socket.BeginReceive(obj.Buffer, 0, args.Length,
                                            SocketFlags.None,
                                            ReceiveCallback, obj);

            } catch (Exception exc) {
                if (exc.Message == "An existing connection was forcibly closed by the remote host") {
                    Console.WriteLine("***Server has been shutdown, closing...*** reason:\n   "+exc.Message);
                    this.Disconnect();
                    return;
                }
                if (serverSocket != null)
                    Console.WriteLine("Error at receive ReceiveCallback: "+exc);
            }
        }

        internal void OnMessageArrivalInvoke(OnMessageArrivalArgs args) {
            //lock (lock_read) { //todo decide wether to leave it or add it
                OnMessageArrival(args);
            //}
        }

        public void Disconnect(string reason) {
            var time = DateTime.Now;
            Console.WriteLine("***Master forced this slave to disconnect, Reason: ***\n    " + reason);
            #region Disconnect Manually
            try {
                // Disables sends and receives on a Socket. 
                serverSocket.Shutdown(SocketShutdown.Both);
                //Closes the Socket connection and releases all resources 
                serverSocket.Close();
                serverSocket = null;
            } catch (Exception exc) { Console.WriteLine(exc.ToString()); }
            #endregion
            if (reason == "Was not approved by master!") {
                OnConnectionDisproved(time, reason);
            }
        }

        public void Disconnect() {
            try {
                // Disables sends and receives on a Socket. 
                serverSocket.Shutdown(SocketShutdown.Both);
                //Closes the Socket connection and releases all resources 
                serverSocket.Close();
            } catch (Exception exc) { Console.WriteLine(exc.ToString()); }

            var holder = new BooleanHolder(false);
            OnDisconnect(DateTime.Now, holder);
            if (holder) {
                while (true) {
                    bool result = AttemptConnect(1000);
                    if (result == false) {
                        var boolean = new BooleanHolder(false);
                        OnReconnectionFail(DateTime.Now, boolean);
                        if (boolean == false)
                            break;
                        else 
                            continue;
                    }
                    OnConnectedSuccessfuly(DateTime.Now);
                    break;
                }
            }
        }
        #endregion 

        #region Approval Waitress
        private readonly List<string> toSendList = new List<string>();
        private bool Handshaked { get { return _handshaked; } set { if ((_handshaked = value) == true) InvokeSendList(); } }
        private bool _handshaked = false;
        
        private void InvokeSendList() {
            if (_handshaked && toSendList.Count > 0)
                Task.Run(() => {
                    foreach (var item in toSendList) {
                        SendMessage(item);
                    }
                });
        }

        #endregion

        #region Async Communication

        private readonly List<RequestCollector> requestCollectors = new List<RequestCollector>();
        public ReadOnlyCollection<RequestCollector> RequestCollectors { get { return requestCollectors.AsReadOnly(); } } 
        private readonly List<RequestRegisteration> Requests = new List<RequestRegisteration>();
        public async Task<ArrayList> Request(string RequestName, int timeout = 0, params string[] parameters) {
            if (Connected == false)
                return null;
            var req = RequestRegisteration.Create(RequestName, serverSocket, timeout, parameters);
            Requests.Add(req);
            SendMessage(Message.Create("req", req.RequestName + Constants.ShortSplitLetter + req.Guid, parameters));
            return await req.Waiter ?? await Task.Run(() => new ArrayList());
        }

        public bool AddRequestResponder(string RequestName, RequestCollecterAction Collector, RequestCollectorOptions mode) {
            return AddRequestResponder(new RequestCollector(RequestName, Collector, mode));
        }

        public bool AddRequestResponder(RequestCollector collector) {
            if (collector == null || string.IsNullOrEmpty(collector.RequestName) || requestCollectors.Any(s => s.RequestName == collector.RequestName))
                return false;
            requestCollectors.Add(collector);
            return true;
        }

        public bool RemoveRequestResponder(string RequestName) {
            return requestCollectors.RemoveAll(c => c.RequestName == RequestName) > 0;
        }
        
        private IEnumerable<string> CollectRespond(string RequestName, params string[] parameters) {
            var i = requestCollectors.FirstOrDefault(c => c.RequestName == RequestName);
            return i == null ? null : i.Request(parameters ?? new string[] {});
        }

        #endregion

        #region Communication Basics

        public bool Ping() {
            try {
                if (!Connected)
                    return false;
                SendMessage(Message.Create("ping", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString()));
                return true;
            }
            catch {
                return false;
            }
        }

        public virtual void AutoResponder(OnMessageArrivalArgs args) {
            try {
                if (args.Message.Failed)
                    return;
                switch (args.Class) {
                    case "hs":
                        if (args.Message.Text.Contains("request"+Constants.ShortSplitLetter)) {
                            var msg = Message.Create(args.Class, args.Message.Text.AsStrings()[1],
                                                     new[] {Name, GUID.ToString()});
                            SendMessage(msg);
                        } else if (args.Message.Text.Contains("approved"+Constants.ShortSplitLetter)) {
                            ServerGUID = Guid.Parse(args.Message.Text.AsStrings()[1]);
                            approvalreset.Set();
                            Handshaked = true;
                        }
                        break;
                    case "ping":
                        SendMessage(Message.Create("pinga", new[] { args.Message.Text, (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString() }).ToString());
                        break;
                    case "pinga":
                        Console.WriteLine("Ping: " + (Convert.ToInt64(args.Variables[1]) - Convert.ToInt64(args.Variables[0])) + "ms");
                        break;
                    case "dc":
                        this.Disconnect(args.Message.Text);
                        return;
                    case "req":
                        IEnumerable<string> l;
                        var varis = args.Variables.ToArray();
                        var txt = args.Message.Text.AsStrings();
                        if (varis.Length == 0) { varis = null; }
                        if ((l = CollectRespond(txt[0], varis)) == null) return;
                        SendMessage(Message.Create("reqa", args.Message.Text, l));
                        return;
                    case "reqa":
                        var txt2 = args.Message.Text.AsStrings();
                        var req = Requests.FirstOrDefault(k => k.Guid == Guid.Parse(txt2[1]) && k.RequestName == txt2[0]);
                        if (req == null) {
                            #if DEBUG
                            Console.WriteLine("reqa received and none matching register for it: " + args.Message.Text);
                            #endif
                            return;
                        }
                        req.SetResult = new ArrayList((args.Message.Variables ?? new List<string>()).ToArray());
                        return;
                }
            } catch {}
        }

        #endregion

        public void Dispose() {
            Disconnect("Slave object has been disposed");
        }
    }

    #region SSS
    public class ServerSideSlave {
        public Socket CommonSocket { get; set; }
        public bool Approved { get; set; }
        public string Name { get { return _name; } set { if (string.IsNullOrEmpty(_name)) return; _name = value; } }
        private string _name;
        public Guid GUID { get { return _guid; } private set { if (_guid != null) return; _guid = value; } }
        private Guid _guid;



        public ServerSideSlave(string Name, Guid guid, Socket commonSocket) {
            CommonSocket = commonSocket;
            _name = Name;
            _guid = guid;
            Approved = true;

        }

        public bool Equals(ServerSideSlave target) {
            return _guid == target._guid;
        }

    }
    #endregion
}*/