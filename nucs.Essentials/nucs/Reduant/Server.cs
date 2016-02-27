/*
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace nucs.Network {
    /// <summary>
    /// Represents the big ass father, who can adopt children and tell them what to do.
    /// </summary>
    public class Server : NetworkBase {

        #region Initialization and Async event handling

        public event NewClientConnectionHandler OnConnectionRequest;
        public event NewClientConnectionApprovedHandler OnConnectionApproved;
        public event DataSentFromServer OnMessageSent;
        public event ClientDisconnected OnClientDisconnection;
        public List<TcpClient> Clients { get; private set; }
        public TcpListener Listener { get; private set; }
        public int Port { get; private set; }
        public string Address { get; private set; }

        public Server(string identifier, string address, int port) {
            initiallize(identifier, address, port);
        }

        public Server(string address, int port) {
            initiallize("Server-" + new Random().Next(1, 1000000), address, port);
        }

        private void initiallize(string identifier, string address, int port) {
            if (!identifier.Contains("Server"))
                throw new InvalidOperationException("Identifier must contain the following exact string: \"Server\"");
            Address = address;
            Port = port;
            Type = NetworkType.Server;
            Identity = identifier;
            OnConnectionRequest += (cli, args) => {  };
            OnReadBytes += (cli, received, lenght) => { };
            OnReadString += (cli, msg, encoding) => { };
            OnMessageSent += (broadcast, args) => { };
            OnClientDisconnection += (cli, time) => { Clients.Remove(cli); Console.WriteLine(time + ": Client has disconnected"); };
            OnMessageArrival += args => Console.WriteLine("Server Received: " + args.msg.ToString());
            OnConnectionApproved += (cli, networkStream, identity) => { };
            Host = new IPEndPoint(Dns.GetHostEntry(address).AddressList[0], port);
            Clients = new List<TcpClient>();
            Listener = new TcpListener(Host);
            Listener.Start();
            Listener.BeginAcceptTcpClient(AcceptConnection, null);
            InitPinger();
        }

        private void SetupReceiver(TcpClient cli) {
            try {
                var buffer = new byte[1024];
                if (cli.Connected && cli.GetStream().CanRead)
                {
                    cli.GetStream().BeginRead(buffer, 0, buffer.Length,
                                              ar => DataReceiver(ar, cli, buffer, 0, buffer.Length), null);
                }
            } catch (Exception e) {Console.WriteLine("SetupReceiver failed initializing: "+e);}
        }

        public override bool SendMessage(Socket socket, string msg) {
            try {
                if (socket.Connected && socket.Poll(3000, SelectMode.SelectWrite)) {
                    OnMessageSent(false, msg);
                    var bytes = Encoding.UTF8.GetBytes(msg);
                    socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
                    return true;
                }
                return false;
            } catch {
                return false;
            }
        }

        public override bool SendMessage(string msg) {
            try {
                Clients.ForEach(c => {
                    if (c.Connected && c.Client.Poll(3000, SelectMode.SelectWrite)) {
                        OnMessageSent(true, msg);
                        var bytes = Encoding.UTF8.GetBytes(msg);
                        c.Client.Send(bytes, 0, bytes.Length, SocketFlags.None);
                    }
                });
                return true;
            } catch {
                return false;
            }
        }

        private void InitPinger() {
            Task.Factory.StartNew(async () => {
                while (true) {
                    if (Clients.Count == 0)
                        await Task.Delay(5000);
                    foreach (var cli in Clients.ToArray()) {
                        if (cli.Connected == false) {
                            OnClientDisconnection(cli, DateTime.Now);
                        }
                        await Task.Delay(1000);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void AddClient(TcpClient cli, NetworkStream stream, string identity) {
            if (cli.Connected) {
                Clients.Add(cli);
                SetupReceiver(cli);
            }
        }

        private void RemoveClient(TcpClient cli) {
            Clients.Remove(cli);
            cli.GetStream().Close();
            cli.Close();
        }


        private void DataReceiver(IAsyncResult ar, TcpClient cli, byte[] buffer, int offset, int lenght) {
            try {
                if (cli.Connected) {
                    cli.GetStream().EndRead(ar);
                    OnReadBytesInvoke(cli, buffer, lenght);
                    OnReadStringInvoke(cli, Encoding.UTF8.GetString(buffer).Replace("\0", ""), Encoding.UTF8);
                    OnMessageArrivalInvoke(cli, buffer);
                    SetupReceiver(cli);
                }
            } catch {}
        }

        private async void AcceptConnection(IAsyncResult ar) {
            var cli = Listener.EndAcceptTcpClient(ar);
            if (!Clients.Contains(cli)) {
                var args = new NewClientConnectionArgs();
                OnConnectionRequest(cli, args);
                if (args.Approved) {
                    //Task.Run(async () => {
                        AddClient(cli, cli.GetStream(), string.Empty);
                        //var msg = await Responder_GetResponde(this, cli.Client, "identify", "OnApprovalPending", Identity);
                        var msg = await Responder_GetResponde(this, cli.Client, "identify", "OnApprovalPending", Identity);
                        if (msg.Stage == 1 && msg.Header == "identify" && msg.Failed == false && msg.Method == "OnApprovalPending" && msg.Text.Contains("Client"))
                            OnConnectionApproved(cli, cli.GetStream(), msg.Text.Split('-')[1]);
                        else {
                            RemoveClient(cli);
                        }
                    //});
                }
            }
            Listener.BeginAcceptTcpClient(AcceptConnection, null);
        }


        public class NewClientConnectionArgs {
            /// <summary>
            /// Accept the new incoming connection, default is true.
            /// </summary>
            public bool Approved = true;
        }

        #endregion

        public bool BroadcastString(string message) {
            if (Clients.Count == 0)
                return false;
            try {
                Clients.ForEach(c => {
                    if (c.Connected && c.Client.Poll(3000, SelectMode.SelectWrite)) {
                        var msg = Encoding.UTF8.GetBytes(message);
                        c.Client.Send(msg, 0, msg.Length, SocketFlags.None);
                    }
                });
                return true;
            }
            catch {
                Console.WriteLine("Failed sending a broadcast to: " + Host.Address + ":" + Host.Port + " with message: " +
                                  message);
                return false;
            }
        }


        public void Dispose() {
            this.Clients.ForEach(c => c.Client.Disconnect(false));
            this.Listener.Server.Dispose();
        }

        public void CommunicationBasic(OnMessageArrivalArgs args) {
            if (args.msg.Header == "hs" && args.msg.Method == "identify")
                        args.MessageToSend = "true";
        }
    }
}
*/
