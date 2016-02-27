/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using nucs.Network.SocketCommunication;
using nucs.SocketCommunication.Commons;
using nucs.SystemCore.String;
using nucs.Utils;
using nucs.Collections.Extensions;

namespace nucs.SocketCommunication {

    /// <summary>
    /// The master in RRC (Request Responde Communication)
    /// </summary>
    public class Master {

        #region Inits & Constructors

        private SocketPermission permission;
        private Socket sListener;
        private IPEndPoint ipEndPoint;
        private bool started;
        public event OnMasterMessageArrivalHandler OnMessageArrival;
        public event OnTextArrivalHandler OnTextArrival;
        public event OnConnectionArrivalHandler OnConnectionArrival;
        public event OnConnectionApprovalHandler OnConnectionApproval;
        public event OnSlaveDisconnectedHandler OnDisconnection;
        public readonly Guid GUID = Guid.NewGuid();
        public readonly List<ServerSideSlave> Slaves = new List<ServerSideSlave>();
        public int Port { get; set; }
        private readonly object lock_send = new object();
        private object lock_read = new object();
        private readonly object lock_accept = new object();

        public Master(int port) {
            OnMessageArrival += AutoResponder;
            OnConnectionApproval += (sl, allow) => { };
            OnConnectionArrival += slave => { };
            OnTextArrival += args => { };
            OnDisconnection += (slave, at) => { }; 
            Port = port;
            Init(port);
            InitResponders();
        }

        public Master(int port, bool StartNow) : this(port) {
            if (StartNow)
                StartListening();
        }

        #endregion

        #region Functions and Commands
        private void Init(int port) {
            try {
                // Creates one SocketPermission object for access restrictions
                permission = new SocketPermission(PermissionState.Unrestricted);
                /*permission = new SocketPermission(
                    NetworkAccess.Accept /*Allowed to accept connections#3#,
                    TransportType.Tcp, /*Defines transport types#3#
                    "",
                    port // Specifies all ports 
                    );#1#//todo see this permission, maybe reenable it and disable top 1
                sListener = null;
                permission.Demand();
                //var ipAddr = Dns.GetHostEntry("localhost").AddressList.FirstOrDefault(ad => ad.AddressFamily != AddressFamily.InterNetworkV6);
                //if (ipAddr == null)
                //throw new Exception("couldn't identify ip address");
                ipEndPoint = new IPEndPoint(IPAddress.Any, port); //TODO
                sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                sListener.Bind(ipEndPoint);

                Console.WriteLine("Server started, ready to start listening.");
            } catch (Exception exc) {
                throw new Exception("Could not initiallize listener; "+ exc);
            }
        }

        protected virtual void InitResponders() {
            
        }

        public void StartListening() {
            if (started)
                return;
            try {
                // Places a Socket in a listening state and specifies the maximum 
                // Length of the pending connections queue 
                sListener.Listen(10);

                // Begins an asynchronous operation to accept an attempt
                sListener.BeginAccept(AcceptCallback, sListener);

                Console.WriteLine("Server is now listening on " + ipEndPoint.Address + " port: " + ipEndPoint.Port + ". Ready to receive messages.");
                started = true;
            }
            catch (Exception exc) {
                Console.WriteLine(exc.ToString());
            }
        }

        internal void AcceptCallback(IAsyncResult ar) { //ar.AsyncState - socket in which it is listening to new connections (handler)
            lock (lock_accept) {
                try {
                    var listener = (Socket) ar.AsyncState;
                    var socket = listener.EndAccept(ar);

                    var args = AsyncEventArgs.Create(socket);
                    // Creates one object array for passing data
                    var reset = new ManualResetEvent(false);
                    var naArgs = new SlaveApprovalArgs {Reset = reset};
                    var i = new Random().Next(0, 9999999);
                    naArgs.ID = i;
                    ApprovalAppending.Add(naArgs);
                    socket.BeginReceive(
                        args.Buffer, // An array of type Byt for received data 
                        0, // The zero-based position in the buffer  
                        args.Length, // The number of bytes to receive 
                        SocketFlags.None, // Specifies send and receive behaviors 
                        ReceiveCallback, //An AsyncCallback delegate 
                        args // Specifies infomation for receive operation 
                        );
                    listener.BeginAccept(AcceptCallback, listener);
                    handshake(socket, ref naArgs);
                    if (!reset.WaitOne(10000)) {
                        socket.Close();
                        ApprovalAppending.Remove(ApprovalAppending.ToArray().First(j => j.ID == naArgs.ID));
                        return;
                    }

                    var holder = new BooleanHolder(true);
                
                    var slave = ApprovalAppending.ToArray().First(pair => pair.ID == naArgs.ID).Result;
                    OnConnectionApproval(slave, holder);
                    if (holder) {
                        Slaves.Add(slave);
                        OnConnectionArrival(slave);
                    } else {
                        slave.CommonSocket.Disconnect(false);
                        slave.CommonSocket.Close();
                    }
                    ApprovalAppending.Remove(ApprovalAppending.ToArray().First(j => j.ID == naArgs.ID));
                }
                catch (Exception exc) {
                    Console.WriteLine(exc.ToString());
                }
            }
        }

        internal void ReceiveCallback(IAsyncResult ar) { //State is an array, 1. buffer 2. socket (
            var currentArgs = new AsyncEventArgs();
            try {
                var args = currentArgs = (AsyncEventArgs) ar.AsyncState;
                var content = string.Empty;
                int bytesRead = 0;
                try {bytesRead = args.Socket.EndReceive(ar);}
                catch (Exception e) {
                    if (e.Message.Contains("Cannot access a disposed object."))
                        throw new Exception("Connection has been rejeced.");
                }

                if (bytesRead > 0) {
                    content += Encoding.UTF8.GetString(args.Buffer, 0, bytesRead);
                    var a = content.StringsTill(Constants.Delimiter);

                    foreach (var i in a) {
                        OnTextArrival(OnTextArrivalArgs.Create(args.Socket, i));
                        var b = i.StringsBetween(Constants.StartMessage, Constants.EndMessage);
                        foreach (var msg in b) {
                            OnMessageArrivalArgs margs;
                            if ((margs = OnMessageArrivalArgs.TryCreate(args.Socket, Constants.StartMessage + msg + Constants.EndMessage)) != null) {
                                Task.Run(() => MessageInvoke(Slaves.FirstOrDefault(j=>j.CommonSocket == args.Socket) ,margs));
                            }
                        }
                    }
                }
                var obj = AsyncEventArgs.Create(args.Socket);
                args.Socket.BeginReceive(obj.Buffer, 0, args.Length,
                                        SocketFlags.None,
                                        ReceiveCallback, obj);
               // #if DEBUG
                //Console.WriteLine("All Received: "+content);
               // #endif
            } catch (Exception exc) {
                switch (exc.Message) {
                    case "An existing connection was forcibly closed by the remote host":
                        DisconnectSocket(currentArgs.Socket, exc.Message);
                        return;
                    case "Connection has been rejeced.":
                        return;
                }
                Console.WriteLine("Error at receive ReceiveCallback: "+exc);
            }
        }
        
        internal void MessageInvoke(ServerSideSlave slave, OnMessageArrivalArgs args) {
            //lock (lock_read) { //todo decide wether to perform lock or not.
                OnMessageArrival(slave, args);
            //}
        }

        #region Messaging
        public void Broadcast(string msg) {
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
        }

        public bool SendTo(Socket towards, string text) {
            if (towards.Connected == false) return false;
            if (!towards.Poll(-1, SelectMode.SelectWrite)) { DisconnectSocket(towards, "Poll of SelectWrite has failed."); return false; }
            lock (lock_send) {
                try {
                    var bytes = Encoding.UTF8.GetBytes(text + Constants.Delimiter);
                    towards.BeginSend(bytes, 0, bytes.Length, 0, SendCallback, AsyncEventArgs.Create(towards, bytes));
                }
                catch { Console.WriteLine("Failed sending " + text); return false; }
            }
            return true;
        }

        public void SendTo(ServerSideSlave towards, string text) {
            SendTo(towards.CommonSocket, text);
        }

        public void SendTo(ServerSideSlave towards, Message msg) {
            SendTo(towards, msg.ToString());
        }

        public void SendTo(Socket towards, Message msg) {
            SendTo(towards, msg.ToString());
        }

        internal void SendCallback(IAsyncResult ar) {
            try {
                var args = (AsyncEventArgs) ar.AsyncState;
                args.Socket.EndSend(ar);
            } catch (Exception exc) {
                Console.WriteLine(exc.ToString());
            }
        }
        #endregion

        public void Shutdown() {
            try {
                if (sListener.Connected) {
                    sListener.Shutdown(SocketShutdown.Receive);
                    sListener.Close();
                }
                Slaves.AsParallel().ForAll(s=>s.CommonSocket.Disconnect(false));

                Console.WriteLine("Server has been shut down.");
            } catch (Exception exc) {
                Console.WriteLine(exc.ToString());
            }
        }

        private void DisconnectSocket(Socket socket, string reason) {
            try {
                ServerSideSlave slave = Slaves.ToArray().FirstOrDefault(sl => Sockets.Compare(sl.CommonSocket, socket));
            if (slave == null && Slaves.Count > 0)
                slave = Slaves.ToArray().FirstOrDefault(sl=> {
                    try {
                        if (sl.CommonSocket.Available>0){}
                    } catch {
                        return true;
                    }
                    return false;
                });
            if (slave == null)
                throw new Exception("Socket that has been disconnected a moment ago, cannot be found in Slaves list to pass it in disconnect event.");
            
            Slaves.Remove(slave);
            OnDisconnection(slave, DateTime.Now);
            Console.WriteLine("***Slave "+slave.Name+" has been disconnected " + slave.CommonSocket.LocalEndPoint + "***" + "\n    Reason: " + reason);

            socket.Close();
            } catch (Exception e) {Console.WriteLine("Error at DisconnectSocket on Master\n"+e);}
        }
        #endregion

        #region Async Communication

        private readonly List<RequestCollector> requestCollectors = new List<RequestCollector>();
        public ReadOnlyCollection<RequestCollector> RequestCollectors { get { return requestCollectors.AsReadOnly(); } }
        private readonly List<RequestRegisteration> Requests = new List<RequestRegisteration>();


        //public async Task<ArrayList> Request(string SlaveName, string RequestName, int timeout = 0) {RequestName, timeout)}
        //todo figure out a way to broadcast a request from all/many targets
        //public async Task<ArrayList> Request(Guid SlaveGUID, string RequestName, int timeout = 0) {}

        public async Task<ArrayList> Request(ServerSideSlave target, string RequestName, int timeout = 0, params string[] parameters) {
            var req = RequestRegisteration.Create(RequestName, target.CommonSocket, timeout, parameters);
            Requests.Add(req);
            SendTo(target, Message.Create("req", req.RequestName + Constants.ShortSplitLetter + req.Guid, parameters));
            return await req.Waiter ?? await Task.Run(() => new ArrayList());
        }

        public Task<ArrayList> Request(string slaveName, string RequestName, int timeout = 0, params string[] parameters) {
            var sss = Slaves.FirstOrDefault(s => s.Name == slaveName);
            if (sss == null) throw new Exception("Slave under name "+ slaveName + " could not be found");
            return Request(sss, RequestName, timeout, parameters);
        }

        public Task<ArrayList> Request(Guid slaveGuid, string RequestName, int timeout = 0, params string[] parameters) {
            var sss = Slaves.FirstOrDefault(s => s.GUID == slaveGuid);
            if (sss == null) throw new Exception("Slave under guid "+ slaveGuid + " could not be found");
            return Request(sss, RequestName, timeout, parameters);
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

        public virtual void AutoResponder(ServerSideSlave slave, OnMessageArrivalArgs args) {
            if (args.Message.Failed)
                return;
            switch (args.Class) {
                case "ping":
                    SendTo(args.CommonSocket, (Message.Create("pinga", new[] { args.Message.Text, (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString()}).ToString()));
                    return;
                case "pinga":
                    Console.WriteLine("Ping: " + (Convert.ToInt64(args.Variables[1]) - Convert.ToInt64(args.Variables[0])) + "ms");
                    return;
                case "hs":
                    var vars = args.Variables;
                    if (vars != null && vars.Count==2) {
                        SlaveApprovalArgs argssssl;
                        try {argssssl = ApprovalAppending.ToArray().First(sssl => sssl.ID.ToString() == args.Message.Text);} catch (Exception exx) {Console.WriteLine(exx);return;}
                        ApprovalAppending.Remove(argssssl);
                        argssssl.Result = new ServerSideSlave(vars[0], Guid.Parse(vars[1]), args.CommonSocket) { CommonSocket = args.CommonSocket };
                        argssssl.Reset.Set();
                        ApprovalAppending.Add(argssssl);
                        SendTo(args.CommonSocket, Message.Create("hs", "approved" + Constants.ShortSplitLetter + GUID));
                    }
                    return;
                case "req":
                    IEnumerable<string> l;
                    var varis = args.Variables.ToArray();
                    var txt = args.Message.Text.AsStrings();
                    if (varis.Length == 0) { varis = null; }
                    if ((l = CollectRespond(txt[0], varis)) == null) return;
                    SendTo(args.CommonSocket, Message.Create("reqa", args.Message.Text, l));
                    return;
                case "reqa":
                    var txt2 = args.Message.Text.AsStrings();
                    var req = Requests.FirstOrDefault(k => k.Guid == Guid.Parse(txt2[1]) && k.RequestName == txt2[0]);
                    if (req == null) {
                        #if DEBUG
                        Console.WriteLine("reqa received and none matching register for it: "+args.Message.Text);
                        #endif
                        return;
                    }
                    req.SetResult = new ArrayList(args.Message.Variables.ToArray());
                    return;
            }
        }

        protected readonly List<SlaveApprovalArgs> ApprovalAppending = new List<SlaveApprovalArgs>();
        private void handshake(Socket target, ref SlaveApprovalArgs args){
            try {
                SendTo(target, Message.Create("hs", "request" + Constants.ShortSplitLetter + args.ID));
            } catch {
                
            }
        }

        public void BroadcastPing() {
            Broadcast(Message.Create("ping", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString(CultureInfo.InvariantCulture)));
        }
        #endregion
    } 
}

/*   Custom disprove,
        Add this code before base.AutoResponder(slave, args);
             switch (args.Class) {
                case "hs":
                    var vars = args.Variables;
                    if (vars != null && vars.Count==2) {
                        SlaveApprovalArgs argssssl;
                        try {argssssl = ApprovalAppending.ToArray().First(sssl => sssl.ID.ToString() == args.Message.Text);} catch (Exception exx) {Console.WriteLine(exx);return;}
                        ApprovalAppending.Remove(argssssl);
                        argssssl.Result = new ServerSideSlave(vars[0], Guid.Parse(vars[1]), args.CommonSocket) { CommonSocket = args.CommonSocket };
                        argssssl.Reset.Set();
                        //PLACE CONDITION IF TRUE, Send a message by class "dc" and return; (see example down below)
                        ApprovalAppending.Add(argssssl);
                        SendTo(args.CommonSocket, Message.Create("hs", "approved:" + GUID));
                    }
                    return;
            }
        
        replace the //PLACE CONDITION IF TRUE, DO SendTo(args.CommonSocket, Message.Create("dc", "disproved: <reason>")); return;
        with the desired condition
        Example:
       
        if (slave != null && FormMain.MainInstance.CurrentConnectionStatus != FormMain.ConnectionStatus.Connected) {
            SendTo(slave.CommonSocket, Message.Create("dc", "disproved:db is not connected yet."));
            return;
        }
  
 

#1#*/