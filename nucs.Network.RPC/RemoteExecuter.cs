using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using csscript;
using CSScriptLibrary;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using ProtoBuf;

namespace nucs.Network.RPC {

    /// <summary>
    ///     A service to run code on a remote node.
    /// </summary>
    public static class RemoteExecuter {

        /// <summary>
        ///     Initialize Listeners to allow running code from remote node on this machine. 
        ///     This is not required to execute code on a remote node.
        /// </summary>
        public static void Listen() {
            NetworkComms.AppendGlobalIncomingPacketHandler<ExecuteRequest>("RemoteExecution", _Execute);
            NetworkComms.AppendGlobalIncomingPacketHandler<ExecuteRequest>("RemoteReturnExecution", _ExecuteReturn);
            
            if (Connection.AllExistingLocalListenEndPoints().Any(kv => kv.Key == ConnectionType.TCP && kv.Value.Any(ep => ((IPEndPoint) ep).Port == 35555)) == false) {
                Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, 35555));
            }
        }

        /// <summary>
        ///     Closes Handlers for remote code execution on this machine
        /// </summary>
        public static void Stop() {
            NetworkComms.RemoveGlobalIncomingPacketHandler("RemoteExecution");
            NetworkComms.RemoveGlobalIncomingPacketHandler("RemoteReturnExecution");
        }

        /// <summary>
        ///     prepares code for execution
        /// </summary>
        private static string _prepare_code(string code, List<string> namespaces) {
            var ns = string.Join(Environment.NewLine, (namespaces?.Select(s => $"using {s};") ?? new string[0]));
            if (string.IsNullOrEmpty(code.Trim()))
                return "";
            if (code.Contains("Execute()"))
                return ns + code;
            //has no body, pure code.
            if (code.Contains("return ")) {
                return $@"
                {ns}
                using System;
                public class HeadlessClass : IExecute<object> {{
                    public object Execute() {{
                        {code}
                    }}
                }}           
                ".Trim();
            } else
                return $@"
                {ns}
                using System;
                public class HeadlessClass : IExecute {{
                    public void Execute() {{
                        {code}
                    }}
                }}           
                ".Trim();
        }

        private static List<string> _prepare_namespaces(IEnumerable<string> namespaces) {
            return namespaces?.Select(ns => ns.Trim().Replace("using ", "").Trim(' ', ';')).ToList() ?? new List<string>(0);
        }

        /// <summary>
        ///     Executes the given code in a remote location
        /// </summary>
        public static void Execute(string code, IEnumerable<string> namespaces = null) {
            code = _prepare_code(code, _prepare_namespaces(namespaces));
            if (code == "")
                return;
            IExecute exe = null;
            try {
                exe = CSScript.LoadCode(code)
                    .CreateObject("*")
                    .AlignToInterface<IExecute>();
            } catch (CompilerException e) when (e.Message.Contains("error CS0103")) {
                var ex = new MissingNamespaceException(e.Message.Split(new[] {": error"}, StringSplitOptions.RemoveEmptyEntries)[1], e);
                Console.WriteLine(ex);
                throw ex;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw e;
            }

            try {
                exe.Execute();
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public static object ExecuteReturn(string code, IEnumerable<string> namespaces = null) {
            code = _prepare_code(code, _prepare_namespaces(namespaces));
            if (code == "")
                return null;
            IExecute<object> exe = null;
            try {
                exe = CSScript.LoadCode(code)
                    .CreateObject("*")
                    .AlignToInterface<IExecute<object>>();
            } catch (CompilerException e) when (e.Message.Contains("error CS0103")) {
                var ex = new MissingNamespaceException(e.Message, e);
                Console.WriteLine(ex);
                throw ex;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw e;
            }

            try { 
                return exe.Execute();
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            return null;
        }

        #region HandleNetReqs

        private static void _Execute(PacketHeader packetHeader, Connection conn, ExecuteRequest req) {
            Execute(req.Code,req.Namespace);
        }

        private static void _ExecuteReturn(PacketHeader packetHeader, Connection conn, ExecuteRequest req) {
            conn.SendObject("RemoteReturnExecutionReply", ExecuteReturn(req.Code, req.Namespace));
        }

        #endregion
    }

    public static class RemoteExecuterExtension {
        public static bool Execute(this Connection conn, string code, params string[] namespaces) {
            try {
                conn.EstablishConnection();
                conn.SendObject("RemoteExecution", code);
            } catch (Exception e) {
                return false;
            }
            return true;
        }

        public static T Execute<T>(this Connection conn, string code, params string[] namespaces) {
            try {
                conn.EstablishConnection();
                var ret = conn.SendReceiveObject<ExecuteRequest, T>("RemoteReturnExecution", "RemoteReturnExecutionReply", 10000, new ExecuteRequest {Code = code, Namespace = namespaces});
                return ret;
            } catch (Exception e) {
                return default(T);
            }
        }
    }

    [ProtoContract]
    public class ExecuteRequest {
        [ProtoMember(1)]
        public string Code { get; set; }

        [ProtoMember(2)]
        public string[] Namespace { get; set; }
    }
}