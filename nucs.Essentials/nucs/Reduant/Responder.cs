/*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Network {

    public class Responder {
        private readonly List<KVPair<NetworkBase, Registeration>> registrations =
            new List<KVPair<NetworkBase, Registeration>>();

        /// <summary>
        /// Registeration for Client.
        /// </summary>
        /// <returns>unique guid ticket</returns>
        public Guid Register(NetworkBase netbase, Socket socket, string header, int stage, string method, string msg) {
            Registeration reg;
            registrations.Add(new KVPair<NetworkBase, Registeration>(netbase,reg = new Registeration(socket, header, stage, method, msg)));
            return reg.guid;
        }

        /// <summary>
        /// Registeration for Server.
        /// </summary>
        /// <returns>unique guid ticket</returns>
        public Guid Register(NetworkBase netbase, string header, int stage, string method, string msg) {
            Registeration reg;
            registrations.Add(new KVPair<NetworkBase, Registeration>(netbase, reg = new Registeration(header, stage, method, msg)));
            return reg.guid;
        }

        public Registeration GetRegisterationTicket(Guid guid) {
            var regs = registrations.ToList().Where(v => v.registeration.guid == guid).ToList();
            if (regs.Count > 0)
                return regs[0].registeration;
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid">Guid given from registering;</param>
        /// <returns></returns>
        public async Task<Message> GetResponde(Guid guid) {
            var item = registrations.FirstOrDefault((k) => k.registeration.guid == guid);
            if (item == null) throw new InvalidDataException("Invalid guid!");
            var reg = item.registeration;
            var net = item.netbase;

            var s = Task.Run(() => {
                var result = "";
                var handle = new AutoResetEvent(false);
                DataReceivedStringHandler func = (cli, received, encoding) => {
                    var msg = Message.GetMessage(received);
                    if (msg.Failed) {
                        result = reg.respond = null;
                        handle.Set();
                        Console.WriteLine("Message cought in message recieval had wrong context: "+msg.ToString());
                        return;
                    }
                    var subs = received.Substring(0,
                                                    net.Identity.Length + 1 + reg.header.Length + 1 +
                                                    reg.stage.ToString().Length);
                    var sibs = msg.Identity + ":" + reg.header + ":" + (reg.stage + 1);
                    if (subs == sibs) {
                        result = reg.respond = received;
                        handle.Set();
                    }
                };

                net.OnReadString += func;
                handle.WaitOne();
                net.OnReadString -= func;
                return result;
            });

            //s1:hs:1/identify-hello, im slave and this is a message.
            //identity:header:stage/method-text
            bool sendConfirm = false;
            sendConfirm = reg.Type == NetworkBase.NetworkType.Client
                              ? net.SendMessage(reg.socket,
                                                net.GetMessageContext(reg.header, reg.stage, reg.method, reg.text))
                              : net.SendMessage(net.GetMessageContext(reg.header, reg.stage, reg.method, reg.text));

            if (sendConfirm == false) {
                reg.GetStage = GetStages.Failed;
                return null;
            }


            reg.GetStage = GetStages.Sent;
            var str = await s;
            reg.GetStage = GetStages.Responded;
            return Message.GetMessage(str);
        }

        [DebuggerStepThrough]
        private class KVPair<TKey, TValue> {
            private TKey key;
            private TValue value;

            public TKey netbase {
                get { return key; }
            }

            public TValue registeration {
                get { return value; }
            }

            public KVPair(TKey key, TValue value) {
                this.key = key;
                this.value = value;
            }
        }

        [DebuggerStepThrough]
        public class Registeration {
            public string header;
            public string text;
            public string respond;
            public string method;
            public int stage;
            public Socket socket;
            public NetworkBase.NetworkType Type;

            /// <summary>
            /// The stage of the GetResponde process.
            /// </summary>
            public GetStages GetStage;

            public Guid guid = Guid.NewGuid();

            public Registeration(Socket socket, string header, int stage, string method, string text) {
                this.header = header;
                this.text = text;
                this.method = method;
                this.stage = stage;
                this.socket = socket;
                Type = NetworkBase.NetworkType.Client;
                GetStage = GetStages.Added;
            }

            public Registeration(string header, int stage, string method, string text) {
                this.header = header;
                this.text = text;
                this.method = method;
                this.stage = stage;
                Type = NetworkBase.NetworkType.Server;
                GetStage = GetStages.Added;
            }


        }

        public enum GetStages {
            Added,
            Sent,
            Responded,
            Failed
        }
    }

    [DebuggerStepThrough]
    public class Message {
        public readonly Encoding EncodingType = Encoding.UTF8;
        public string Received { get; private set; }

        public string Identity { get; private set; }
        public string Header { get; private set; }
        public int? Stage { get; set; }
        public string Method { get; private set; }
        public string Text { get; private set; }
        public bool Failed { get; set; }

        private string Complete {
            get {
                string s = "";
                s += Identity;
                s += ((string.IsNullOrEmpty(Header)) ? "" : (":" + Header + ":" + Stage));
                s += "/" + ((string.IsNullOrEmpty(Method)) ? "" : Method + ":");
                s += Text;
                return s;
            }
        }

        /// <summary>
        /// Initializes the object. Header and Method can be null, Becareful.
        /// </summary>
        /// <param name="message">The pure string that came through the socket</param>
        public static Message GetMessage(string message) {
            var msg = new Message {Received = message};
            try {
                var a = message.Split('/');
                if (a[0].Contains(":")) {
                    var b = a[0].Split(':');
                    msg.Identity = b[0];
                    msg.Header = b[1];
                    msg.Stage = Convert.ToInt32(b[2]);
                }
                else
                    msg.Identity = a[0];
                var c = a[1];
                if (c.Contains(":")) {
                    var d = c.Split(':');
                    msg.Method = d[0];
                    msg.Text = d[1];
                }
                else
                    msg.Text = c;
                msg.Failed = false;
            }
            catch {
                msg.Failed = true;
            }
            return msg;
        }

        public static Message GetMessage(NetworkBase netbase, string header, string method, string text) {
            return new Message
            {Identity = netbase.Identity, Header = header, Method = method, Text = text, Stage = 0};
        }

        public static Message GetMessage(NetworkBase netbase, string header, int stage, string method, string text) {
            return new Message
            {Identity = netbase.Identity, Header = header, Method = method, Text = text, Stage = stage};
        }

        public override string ToString() {
            return Received;
        }

    }

    [DebuggerStepThrough]
    public class OnMessageArrivalArgs {
        public NetworkBase netbase { get; private set; }
        public TcpClient client { get; private set; }
        public string MessageToSend { get; set; }
        public Message msg { get; private set; }

        public OnMessageArrivalArgs(NetworkBase netbase, TcpClient cli, Message msg) {
            this.msg = msg;
            client = cli;
            this.netbase = netbase;
        }

    }

}
*/
