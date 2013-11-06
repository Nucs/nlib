/*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using nucs.Threading;

namespace nucs.Network {

    /// <summary>
    /// Represents the small children that is told what to do, but can request from father and tell him things.
    /// </summary>
    public class Client : NetworkBase {
        public event DataSentFromClient OnMessageSent;

        public TcpClient tcpClient {
            get { return client; }
        }
        public bool Connected { get; set; }
        public bool Reconnect() {
            if (tcpClient.Connected)
                return true;
            try {
                foreach (var ip in Dns.GetHostEntry(Host.Address).AddressList) {
                    Host = new IPEndPoint(ip, Host.Port);
                    try {
                        client = new TcpClient(Host);
                        client.Connect(Host);
                        if (client.Connected) {
                            Connected = true;
                        }
                            break;
                        
                    } catch (Exception e) {}
                }
                
                Stream = client.GetStream();
                SetupReceiver();
                return true;
            } catch {
                Connected = false;
                return false;
            }
        }

        public string ServerIdentity { get; set; }
        private TcpClient client;

        public Client(string Identifier, string address, int port) {
            initiallize(Identifier, address, port);
        }

        public Client(string address, int port) {
            initiallize("Client-" + new Random().Next(1, 1000000), address, port);
        }

        private async void initiallize(string Identifier, string address, int port) {
            if (!Identifier.Contains("Client"))
                throw new InvalidOperationException("Identifier must contain the following exact string: \"Client\"");
            Identity = Identifier;
            Type = NetworkType.Client;
            OnReadBytes += (cli, received, lenght) => { };
            OnReadString += (cli, msg, encoding) => { };
            OnMessageSent += args => Console.WriteLine("Client Sent: " + args);
            OnMessageArrival += args => Console.WriteLine("Client Received: "+args.msg.ToString());
            OnMessageArrival += CommunicationBasic;
            Host = new IPEndPoint(Dns.GetHostEntry(address).AddressList[0], port);
            try {
                foreach (var ip in Dns.GetHostEntry(address).AddressList) {
                    Host = new IPEndPoint(ip, port);
                    try {
                        client = new TcpClient(Host);
                        client.Connect(Host);
                        if (client.Connected) {
                            Message msg;
                            var res = Task.WaitAny(new[] {Responder_GetResponde("hs", "identify", Identifier)}, 2000);
                            if (res != -1) {
                                Connected = true;
                                break;
                            }
                            client.Close();
                            client = null;
                        }
                        
                        
                    } catch (Exception e) {}

                }
                if (Connected == false)
                    throw new Exception("No connection esablished!");
                Stream = client.GetStream();
                SetupReceiver();
            } catch {
                Connected = false;
            }
        }

        [DebuggerStepThrough]
        private void SetupReceiver() {
            try {
                if (tcpClient.Connected && Stream.CanRead) {
                    var buffer = new byte[1024];
                    Stream.BeginRead(buffer, 0, buffer.Length, ar => {
                        client.GetStream().EndRead(ar);
                        OnReadBytesInvoke(client, buffer, buffer.Length);
                        OnReadStringInvoke(client, Encoding.UTF8.GetString(buffer).Replace("\0", ""), Encoding.UTF8);
                        OnMessageArrivalInvoke(client, buffer);
                        SetupReceiver();
                    }, Stream);
                }
            } catch (Exception e) {Console.WriteLine("Error in SetupReceiver in Client class: "+e);}
        }

        public bool Send(byte[] bytes, int offset, int count) {
            try {
                OnMessageSent(Encoding.UTF8.GetString(bytes));
                if (client.Connected && client.Client.Poll(3000, SelectMode.SelectWrite)) {
                    client.Client.Send(bytes, 0, count, SocketFlags.None);
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }

        }

        public bool Send(string msg) {
            try {
                if (client.Connected && client.Client.Poll(3000, SelectMode.SelectWrite)) {
                    var bytes = Encoding.UTF8.GetBytes(msg);
                    client.Client.Send(bytes, 0, bytes.Length, SocketFlags.None);
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }

        }

        /// <summary>
        /// sends string with default encoding of utf8
        /// </summary>
        public override bool SendMessage(Socket socket, string msg) {
            try {
                if (socket.Connected && socket.Poll(3000, SelectMode.SelectWrite)) {
                    var bytes = Encoding.UTF8.GetBytes(msg);
                    socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }
        }

        public override bool SendMessage(string msg) {
            try {
                if (client.Connected && client.Client.Poll(3000, SelectMode.SelectWrite)) {
                    var bytes = Encoding.UTF8.GetBytes(msg);
                    client.Client.Send(bytes, 0, bytes.Length, SocketFlags.None);
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }
        }

        private void CommunicationBasic(OnMessageArrivalArgs args) {
            if (args.msg.Header == "identify" && args.msg.Method == "OnApprovalPending" && args.msg.Stage == 0) {
                args.MessageToSend = Identity.Contains("Client") ? args.netbase.GetRespondeContext(args.msg, Identity) : args.netbase.GetRespondeContext(args.msg, "Client-" + Identity);
                //args.MessageToSend = "Slave-" + this.Identity;
            }
        }
    }
}
*/
