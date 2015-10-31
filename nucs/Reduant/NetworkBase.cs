/*using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace nucs.Network {
    public abstract class NetworkBase {
        protected static Responder responder { get; private set; }
        public IPEndPoint Host { get; set; }
        public event DataReceivedBytesHandler OnReadBytes;
        public event DataReceivedStringHandler OnReadString;
        public event DataReceivedTranslatedHandler OnMessageArrival;
        private NetworkStream stream;

        public NetworkStream Stream {
            get {
                if (Type != NetworkType.Server) return stream;
                throw new TypeAccessException();
            }
            set {
                if (Type != NetworkType.Server) stream = value;
                else throw new TypeAccessException();
            }
        }

        public NetworkType Type { get; set; }
        public string Identity { get; set; }
        public readonly Guid guid = Guid.NewGuid();

        [DebuggerStepThrough]
        protected NetworkBase() {
            if (responder == null)
                responder = new Responder();
        }

        [DebuggerStepThrough]
        public abstract bool SendMessage(Socket socket, string msg);

        //used from server responding back

        [DebuggerStepThrough]
        public abstract bool SendMessage(string getMessageContext);

        //used for client responding back


        //[DebuggerStepThrough]
        protected void OnMessageArrivalInvoke(TcpClient cli, byte[] bytes) {
            var msg = Message.GetMessage(Encoding.UTF8.GetString(bytes).Replace("\0", ""));
            OnMessageArrivalArgs args;
            if (msg.Failed)
                return;
            OnMessageArrival(args = new OnMessageArrivalArgs(this, cli, msg));
            if (args.MessageToSend != null) {
                SendMessage(cli.Client, args.MessageToSend);
            }

        }


        [DebuggerStepThrough]
        protected void OnReadBytesInvoke(TcpClient cli, byte[] received, int lenght) {
            OnReadBytes(cli, received, lenght);
        }

        [DebuggerStepThrough]
        protected void OnReadStringInvoke(TcpClient cli, string msg, Encoding encoding) {
            OnReadString(cli, msg, encoding);
        }

        [DebuggerStepThrough]
        public string GetRespondeContext(Message msg, string text) {
            string s = "";
            s += Identity;
            s += ((string.IsNullOrEmpty(msg.Header)) ? "" : (":" + msg.Header + ":" + (msg.Stage + 1)));
            s += "/" + ((string.IsNullOrEmpty(msg.Method)) ? "" : msg.Method + ":");
            s += text;
            return s;
        }

        [DebuggerStepThrough]
        public string GetMessageContext(string header, int stage, string method, string text) {
            string s = "";
            s += Identity;
            s += ((string.IsNullOrEmpty(header)) ? "" : (":" + header + ":" + stage));
            s += "/" + ((string.IsNullOrEmpty(method)) ? "" : method + ":");
            s += text;
            return s;
        }

        #region Responder API

        [DebuggerStepThrough]
        public async Task<Message> Responder_GetResponde(Guid _guid) {
            return await responder.GetResponde(_guid);
        }

        [DebuggerStepThrough]
        public async Task<Message> Responder_GetResponde(Message msg, string text) {
            if (Type == NetworkType.Client) {
                var cli = this as Client;
                return
                    await responder.GetResponde(Responder_Register(cli, cli.tcpClient.Client, msg.Header, 0, msg.Method, text));
            }
            if (Type == NetworkType.Server) {
                var serv = this as Server;
                return await responder.GetResponde(Responder_Register(serv, msg.Header, 0, msg.Method, text));
            }
            return null;

        }

        [DebuggerStepThrough]
        public async Task<Message> Responder_GetResponde(string header, string method, string text) {
            if (Type == NetworkType.Client) {
                var cli = this as Client;
                return
                    await responder.GetResponde(Responder_Register(cli, cli.tcpClient.Client, header, 0, method, text));
            }
            if (Type == NetworkType.Server) {
                var serv = this as Server;
                return await responder.GetResponde(Responder_Register(serv, header, 0, method, text));
            }
            return null;

        }

        [DebuggerStepThrough]
        public async Task<Message> Responder_GetResponde(string header, int stage, string method, string text) {
            if (Type == NetworkType.Client) {
                var cli = this as Client;
                return
                    await
                    responder.GetResponde(Responder_Register(cli, cli.tcpClient.Client, header, stage, method, text));
            }
            if (Type == NetworkType.Server) {
                var serv = this as Server;
                return await responder.GetResponde(Responder_Register(serv, header, stage, method, text));
            }
            return null;

        }

        public async Task<Message> Responder_GetResponde(NetworkBase netbase, Socket socket, string header, int stage,
                                                         string method, string text) {
            return await responder.GetResponde(Responder_Register(netbase, socket, header, stage, method, text));
        }

        public async Task<Message> Responder_GetResponde(NetworkBase netbase, Socket socket, string header,
                                                         string method, string text) {
            return await responder.GetResponde(Responder_Register(netbase, socket, header, 0, method, text));
        }

        public async Task<Message> Responder_GetResponde(NetworkBase netbase, string header, int stage, string method,
                                                         string text) {
            return await responder.GetResponde(Responder_Register(netbase, header, stage, method, text));
        }

        [DebuggerStepThrough]
        public Guid Responder_Register(NetworkBase netbase, Socket socket, string header, int stage, string method,
                                       string text) {
            return responder.Register(netbase, socket, header, stage, method, text);
        }

        [DebuggerStepThrough]
        public Guid Responder_Register(NetworkBase netbase, string header, int stage, string method, string text) {
            if (Type != NetworkType.Server)
                throw new InvalidOperationException("This Registeration method is only for server type");
            return responder.Register(netbase, header, stage, method, text);
        }

        [DebuggerStepThrough]
        public Responder.Registeration Responder_GetRegisterationTicket(Guid _guid) {
            return responder.GetRegisterationTicket(_guid);
        }

        public enum NetworkType {
            Client = 1,
            Server = 2
        }

        #endregion
    }
}*/