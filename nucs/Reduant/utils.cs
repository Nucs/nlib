/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace nucs.Network {

    public delegate void NewClientConnectionHandler(TcpClient cli, Server.NewClientConnectionArgs args);

    public delegate void NewClientConnectionApprovedHandler(TcpClient cli, NetworkStream stream, string Identity);

    public delegate void DataReceivedBytesHandler(TcpClient cli, byte[] received, int lenght);

    public delegate void DataReceivedStringHandler(TcpClient cli, string msg, Encoding encoding);

    public delegate void DataReceivedTranslatedHandler(OnMessageArrivalArgs args);

    public delegate void DataSentFromServer(bool Broadcast, string args);
    public delegate void DataSentFromClient(string args);

    public delegate void ClientDisconnected(TcpClient cli, DateTime approxTime);

    //[DebuggerStepThrough]
    public delegate string ChatMessageHandler(TcpClient client, Message msg);

    internal class Conversation {
        public string Header { get; private set; }
        public string Method { get; private set; }
        public NetworkBase Sender { get; private set; }
        private readonly SortedList<int, ChatMessageHandler> Messages = new SortedList<int, ChatMessageHandler>();

        internal Conversation(NetworkBase sender, string header, string method, params ChatMessageHandler[] actions) {
            Sender = sender;
            Header = header;
            Method = method;

            for (var i = 0; i < actions.Length; i++) {
                Messages.Add((i == 0) ? 0 : (i == 1) ? 2 : i + 2, actions[i]);
            }






        }


        public bool Add(int stage, ChatMessageHandler action) {
            if (Messages.Keys.Contains(stage))
                throw new ArgumentException("Argument 'stage' is already taken! (" + stage + ")");
            Messages.Add(stage, action);
            return true;
        }

    }




}*/