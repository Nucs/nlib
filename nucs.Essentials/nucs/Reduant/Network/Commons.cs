/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using nucs.Annotations;
using nucs.SocketCommunication;
using nucs.SystemCore.String;

namespace nucs.Network {
    #region Delegates
    /*public delegate void OnMasterMessageArrivalHandler(ServerSideSlave slave, OnMessageArrivalArgs args);

    public delegate void OnMessageArrivalHandler(OnMessageArrivalArgs args);

    public delegate void OnTextArrivalHandler(OnTextArrivalArgs args);

    public delegate void OnSlaveDisconnectHandler(ServerSideSlave slave, DateTime occurredAt);

    public delegate void OnMasterDisconnectedHandler(DateTime occurredAt, BooleanHolder AttemptReconnect);

    public delegate void OnReconnectionSuccessfull(DateTime occurredAt);

    public delegate void OnMasterForcedDisconnect(DateTime occurredAt, string reason);

    public delegate void OnSlaveDisconnectedHandler(ServerSideSlave slave, DateTime occurredAt);

    public delegate void OnConnectionArrivalHandler(ServerSideSlave slave);

    public delegate void OnConnectionApprovalHandler(ServerSideSlave slave, BooleanHolder allow);

    public delegate IEnumerable<string> RequestCollecterAction(params string[] Parameters);
#1#
    #endregion

    public static class Constants {
        public static readonly int BUFFER_SIZE = 1024*16;
        public static readonly string Delimiter = "<end>";
        public static readonly string Split_ClassText = "<ct>";
        public static readonly string Split_Variable = "<vr>";
        public static readonly string StartVariable = "<v>";
        public static readonly string EndVariable = "</v>";
        public static readonly string StartMessage = "<m>";
        public static readonly string EndMessage = "</m>";
        public static readonly char ShortSplitLetter = StringAsParameterPassing.ShortSplitLetter;
        public static readonly string ShortSplitString = StringAsParameterPassing.ShortSplitString;
    }
    
    [DebuggerStepThrough]
    public sealed class Message {
        public string Class { get; set; }
        public string Text { get; set; }
        public IEnumerable<string> Variables { get; set; }
        public bool Failed { get; private set; }
        public string FailReason { get; private set; }
        #region Constructors
        public static Message Create(string Class, IEnumerable<string> Variables) {
            if (string.IsNullOrEmpty(Class))
                throw new Exception("Class cannot be empty!");
            return new Message {Class = Class, Text = "", Variables = Variables};
        }

        public static Message Create(string Class, string Text = "") {
            if (string.IsNullOrEmpty(Class))
                throw new Exception("Class cannot be empty!");
            return new Message {Class = Class, Text = Text, Variables = null};
        }

        public static Message Create(string Class, string Text, IEnumerable<string> Variables) {
            if (string.IsNullOrEmpty(Class))
                throw new Exception("Class cannot be empty!");
            return new Message {Class = Class, Text = Text, Variables = Variables};
        }

        public static Message Create(string Class, IEnumerable<object> Variables) {
            if (string.IsNullOrEmpty(Class))
                throw new Exception("Class cannot be empty!");
            return new Message {Class = Class, Text = "", Variables = Variables.Select(i => i.ToString())};
        }

        public static Message Create(string Class, string Text, IEnumerable<object> Variables) {
            if (string.IsNullOrEmpty(Class))
                throw new Exception("Class cannot be empty!");
            return new Message {Class = Class, Text = Text, Variables = Variables.Select(i => i.ToString())};
        }
        #endregion
        [DebuggerStepThrough]
        public static Message Translate(string message) {
            message = message.StringBetween(Constants.StartMessage, Constants.EndMessage);
            if (string.IsNullOrEmpty(message))
                return new Message {
                    Failed = true,
                    FailReason = "Couldn't find closures of the message: " + Constants.StartMessage + " and " + Constants.EndMessage + "."
                };
            int a;
            var @class = message.Substring(0, a = message.IndexOf(Constants.Split_ClassText, StringComparison.InvariantCulture));
            a += Constants.Split_Variable.Length;
            string @text = "";

            try {
                @text = message.Substring(a, message.IndexOf(Constants.StartVariable, a, StringComparison.InvariantCulture) - a);
            }
            catch {
                try {
                    @text = message.Substring(a, message.Length - a);
                }
                catch {
                }

            }


            IEnumerable<string> variables = null;
            try {
                a += @text.Length;
                var @itemsStr = message.StringBetween(Constants.StartVariable, Constants.EndVariable, a);
                @variables = @itemsStr.Split(Constants.Split_Variable);
                if (!@variables.Any() && !string.IsNullOrEmpty(@itemsStr)) @variables = new[] { @itemsStr };
            }
            catch {
            }
            if (variables != null && variables.Any())
                return Create(@class, @text, @variables.ToArray());
            return Create(@class, @text);
        }

        public static string PrepareVariables(IEnumerable<object> items) {
            if (items == null || !items.Any())
                return "";
            return PrepareVariables(items.Select(i => (i != null ? i.ToString() : "null")).ToArray());
        }

        public static string PrepareVariables(List<string> items) {
            return PrepareVariables(items.ToArray());
        }

        public static string PrepareVariables(string[] items) {
            if (items == null || items.Length == 0)
                return string.Empty;

            var r = new StringBuilder();
            r.Append(Constants.StartVariable);
            for (int i = 0; i < items.Length; i++) {
                if (i == 0) {
                    r.Append(items[i]);
                    continue;
                }
                r.Append(Constants.Split_Variable + items[i]);
            }
            r.Append(Constants.EndVariable);
            return r.ToString();
        }

        public override string ToString() {
            return Constants.StartMessage + Class + Constants.Split_ClassText + Text + PrepareVariables(Variables) +
                   Constants.EndMessage;
        }

        public string ReadableToString() {
            var r = new StringBuilder(Class + ":" + Text + "@");
            if (Variables != null) {
                var vars = Variables.ToList();
                for (int i = 0; i < vars.Count; i++) {
                    if (i == 0) {
                        r.Append(vars[i]);
                        continue;
                    }
                    r.Append("^" + vars[i]);
                }
            }
            return r.ToString();
        }
    }
    
    public static class Sockets {
        // Socks proxy inspired by http://www.thecodeproject.com/csharp/ZaSocks5Proxy.asp
        public static bool UseSocks = false;
        /*        private static readonly string[] errorMsgs = {
                                                         "Operation completed successfully.",
                                                         "General SOCKS server failure.",
                                                         "Connection not allowed by ruleset.",
                                                         "Network unreachable.",
                                                         "Host unreachable.",
                                                         "Connection refused.",
                                                         "TTL expired.",
                                                         "Command not supported.",
                                                         "Address type not supported.",
                                                         "Unknown error."
                                                     };#1#

        public static Socket CreateTCPSocket(string address, int port) {
            var host = Dns.GetHostEntry(address).AddressList[0]; //todo figure out automatic testing mechanism

            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                sock.Connect(new IPEndPoint(host, port));
            }
            catch (Exception e) {
                Console.WriteLine("Failed connecting to " + host + ":" + port + "\n" + e);
                throw;
            }

            return sock;
        }

        public static bool Compare(Socket a, Socket b, bool IfDisposedReturnTrue = false) {
            try {
                // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                a.GetHashCode();
                b.GetHashCode();
                // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            } catch { return IfDisposedReturnTrue; }
            return Compare(a, b);

        }


        public static bool Compare(Socket a, Socket b) {
            try {
                return a.AddressFamily == b.AddressFamily &&
                       a.LocalEndPoint.ToString() == b.LocalEndPoint.ToString();
            } catch {
                return false;
            }
        }
    }

    /*public class SimpleIPAddress {
        public string Address { get; private set; }
        public int Port { get; private set; }
        public AddressFamily AddressFamily { get; private set; }
        public bool IsIPv6 { get; private set; }
        public bool IsIPv4 { get; private set; }
        public SimpleIPAddress(IPAddress address, int port) {
            Address = address.ToString();
            Port = port;
            AddressFamily = address.AddressFamily;
            IsIPv6 = address.IsIPv6LinkLocal || address.IsIPv6Multicast || address.IsIPv6SiteLocal || address.IsIPv6Teredo;
            IsIPv4 = !IsIPv6;

        }

        public SimpleIPAddress(IPEndPoint ipend) {
            var address = ipend.Address;
            Address = address.ToString();
            Port = ipend.Port;
            AddressFamily = address.AddressFamily;
            IsIPv6 = address.IsIPv6LinkLocal || address.IsIPv6Multicast || address.IsIPv6SiteLocal || address.IsIPv6Teredo;
            IsIPv4 = !IsIPv6;
        }

        public SimpleIPAddress(string address, int port) {
            Address = address;
            Port = port;
        }

        public static explicit operator SimpleIPAddress(IPAddress address) {
            return new SimpleIPAddress(address, 0);
        }

        public static implicit operator SimpleIPAddress(IPEndPoint ipend) {
            return new SimpleIPAddress(ipend);
        }

        public override string ToString() {
            return Address ?? "...." + ":" + Port;
        }
    }#1#

    #region Requesting System
    /*
    [DebuggerStepThrough]
    public class RequestRegisteration {
        public readonly Guid Guid = Guid.NewGuid();
        public Socket CommonSocket { get; private set; }
        public string RequestName;
        public ArrayList Result { get { if (Cancelled) return null; if (_waiter == null) return _result; return _waiter.Result; } }
        public Task<ArrayList> Waiter { get {return Cancelled ? null : _waiter;} }
        public IEnumerable<string> Parameters; 

        internal volatile ManualResetEventSlim Holder = new ManualResetEventSlim(false);
        internal ArrayList SetResult { set { if (Holder.IsSet || Cancelled) return; _result = value; Holder.Set(); } }

        private volatile ArrayList _result = null;
        private Task<ArrayList> _waiter = null;
        private volatile bool Cancelled = false;

        internal RequestRegisteration(string RequestName, Socket commonSocket, int timeout, IEnumerable<string> Parameters) {
            CommonSocket = commonSocket;
            this.RequestName = RequestName;
            this.Parameters = Parameters;
            GC.KeepAlive(Cancelled);
            _waiter = Task <ArrayList>.Factory.StartNew(
                () => {
                    if (timeout == 0) Holder.Wait(); else Holder.Wait(timeout);
                    if (Holder.IsSet == false) { Cancelled = true; return null; }
                    if (Cancelled) return null;
                    return _result;
                });
        }

        public static RequestRegisteration Create(string RequestName, Socket commonSocket, int timeout, params string[] Parameters) {
            return new RequestRegisteration(RequestName, commonSocket, timeout, Parameters);
        }
        
        public bool Cancel() {
            if (Holder.IsSet || Cancelled) return false;
            Cancelled = true;
            Holder.Set();
            return true;
        }
    }

    public class RequestCollector {
        public RequestCollecterAction Requester;
        public string RequestName { get; set; }
        public Guid GUID = Guid.NewGuid();
        public readonly RequestCollectorOptions Options;
        public RequestCollector(string requestName, RequestCollecterAction requester, RequestCollectorOptions options) {
            RequestName = requestName;
            Requester = requester;
            Options = options;
        }

        public IEnumerable<string> Request(params string[] parameters) {
            switch (Options.Mode) {
                case RequestCollectorOptionsMode.Any:
                    return Requester != null ? Requester.Invoke(parameters ?? new string[0]) : null;
                case RequestCollectorOptionsMode.Parameterless:
                    if (parameters == null || parameters.Length == 0)
                        return Requester != null ? Requester.Invoke(parameters ?? new string[0]) : null;
                    break;
                case RequestCollectorOptionsMode.SingleParameter:
                    if (parameters != null && parameters.Length == 1)
                        return Requester != null ? Requester.Invoke(parameters) : null;
                    break;
                case RequestCollectorOptionsMode.MultipleParameters:
                    if (parameters == null) break;
                    if (Options.MultipleCount != -1 && Options.MultipleCount == parameters.Length)
                        return Requester != null ? Requester.Invoke(parameters) : null;
                    if (Options.MultipleOrderedNames != null) {
                        var c = Options.MultipleOrderedNames.Aggregate((a, b) => a + Constants.ShortSplitLetter + b);
                        var d = parameters.Aggregate((a, b) => a + Constants.ShortSplitLetter + b);
                        if (c==d) //wtf, it compares the result with parameters.. was i drunk?
                            return Requester != null ? Requester.Invoke(parameters) : null;
                       
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;

        }

        public override bool Equals(object obj) {
            try {
                var o = (RequestCollector) obj;
                return GUID.CompareTo(o.GUID) == 0;
            } catch {
                return false;
            }
        }
    }
    
    public class RequestCollectorOptions {
        public RequestCollectorOptionsMode Mode { get; internal set; }
        internal string SingleParamSpecification = null;
        internal int? MultipleCount = null;
        internal string[] MultipleOrderedNames = null;
        
        internal RequestCollectorOptions() {}

        public static RequestCollectorOptions Any() {
            return new RequestCollectorOptions { Mode = RequestCollectorOptionsMode.Any };
        }

        public static RequestCollectorOptions Parameterless() {
            return new RequestCollectorOptions {Mode = RequestCollectorOptionsMode.Parameterless};
        }

        public static RequestCollectorOptions SingleParameter(string specification = "") {
            return new RequestCollectorOptions {
                                                    Mode = RequestCollectorOptionsMode.SingleParameter,
                                                    SingleParamSpecification = (specification != "") ? specification : null
                                                };
        }


        public static RequestCollectorOptions MultipleParameters(int count) {
            return new RequestCollectorOptions
                    {Mode = RequestCollectorOptionsMode.MultipleParameters, MultipleCount = count};
        }

        public static RequestCollectorOptions MultipleParameters(params string[] OrderedNames) {
            if (OrderedNames == null || OrderedNames.Length == 0) 
                throw new InvalidOperationException("Can't create RequestCollectorOptions w/" +
                                                    " mode of MultipleParameters when there are no" +
                                                    " parameters inserted");
            return new RequestCollectorOptions
                    {Mode = RequestCollectorOptionsMode.MultipleParameters, MultipleOrderedNames = OrderedNames};
        }
    }

    public enum RequestCollectorOptionsMode {
        Any = 0,
        Parameterless = 1,
        SingleParameter = 2,
        MultipleParameters = 3
    }#1#

    #endregion
    /// <summary>
    /// Represents a client on server-side
    /// </summary>
    public class ServerSideClient {
        public Socket Socket { get; internal set; }
        public Guid ClientGuid { get; internal set; }
        public string ClientName { get; internal set; }
        public bool Connected {get { return Socket != null && Socket.Connected; } }
        public ServerSideClient([NotNull] Socket socket) {
            Socket = socket;
        }

        public void Disconnect() {
            Disconnect(null);
        }

        public void Disconnect(string reason) {
            if (Connected == false)
                return;
            Socket.Send(Encoding.UTF8.GetBytes("<disconnect>" + (reason ?? "") + "</disconnect>"));
            Socket.BeginDisconnect(false, ar => Socket.EndDisconnect(ar), null);
        }

        public override string ToString() {
            var ip = (IPEndPoint) Socket.LocalEndPoint;
            return "client:" + ClientName + "://" + ip.Address + ":" + ip.Port;
        }
    }
    #region Args

    public class OnMessageArrivalArgs {
        public Socket CommonSocket { get; set; }
        public Message Message { get; set; }

        public List<string> Variables {
            get { if (Message.Variables == null) return new List<string>(); return Message.Variables.ToList(); }
        }

        public string Class {
            get { return Message.Class; }
        }

        private OnMessageArrivalArgs() {
        }

        /// <summary>
        /// Tries to translate the string, on fail, returns null.
        /// </summary>
        public static OnMessageArrivalArgs TryCreate(Socket commonSocket, string ArrivedMessage) {
            Message msg;
            try {
                msg = Message.Translate(ArrivedMessage);
            }
            catch {
                return null;
            }
            return new OnMessageArrivalArgs {CommonSocket = commonSocket, Message = msg};
        }

        public static OnMessageArrivalArgs Create(Socket commonSocket, Message message) {
            return new OnMessageArrivalArgs {CommonSocket = commonSocket, Message = message};
        }

        public OnMessageArrivalArgs Clone() {
            return new OnMessageArrivalArgs {Message = Message, CommonSocket = CommonSocket};
        }
    }

    public class OnTextArrivalArgs {
        public Socket CommonSocket { get; set; }
        public string Text { get; set; }

        private OnTextArrivalArgs() {
        }

        public static OnTextArrivalArgs Create(Socket commonSocket, string ArrivedMessage) {
            return new OnTextArrivalArgs {CommonSocket = commonSocket, Text = ArrivedMessage};
        }

        public OnTextArrivalArgs Clone() {
            return new OnTextArrivalArgs {Text = Text, CommonSocket = CommonSocket};
        }
    }

    public struct AsyncEventArgs {
        public byte[] Buffer { get; set; }
        public Socket Socket { get; set; }
        
        public int Length {
            get { return Buffer.Length; }
        }

        public AsyncEventArgs([NotNull] Socket socket) : this() {
            Socket = socket;
            Buffer = new byte[Constants.BUFFER_SIZE];
        }

        /// <summary>
        /// Initialized a buffer, and holds the socket.
        /// </summary>
        public static AsyncEventArgs Create([NotNull] Socket socket) {
            return new AsyncEventArgs {Buffer = new byte[Constants.BUFFER_SIZE], Socket = socket};
        }

        /// <summary>
        /// holds the socket and a message that will be sent.
        /// </summary>
        public static AsyncEventArgs Create([NotNull] Socket socket, [NotNull] byte[] buffer) {
            return new AsyncEventArgs {Buffer = buffer, Socket = socket};
        }
    }

    /*public struct SlaveApprovalArgs {
        public int ID { get; set; }
        public ManualResetEvent Reset { get; set; }
        public ServerSideSlave Result { get; set; }
    }#1#

    #endregion

    #region Exceptions
    public class ConnectionException : Exception {
        public ConnectionException(string message) : base(message) {}
    }

    public class ServerDisporvalException : Exception {
        public string Reason { get; set; }

        public ServerDisporvalException(string reason) : base("Server Disproved the client request to connect because: \"" + (reason ?? "Undefined Reason")) {
            Reason = string.IsNullOrEmpty(reason) ? null : reason;
            
        }

        public override string ToString() {
            return "Server Disproved the client request to connect because: \"" + (Reason ?? "Undefined Reason") + "\"\n" + base.ToString();
        }
    }

    #endregion

    #region Attributes

    [AttributeUsage(AttributeTargets.Method)]
    public class Request : Attribute {
        public Request() {
            
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Response : Attribute {
        public Response() {
            
        }
    }

    #endregion

}
*/
