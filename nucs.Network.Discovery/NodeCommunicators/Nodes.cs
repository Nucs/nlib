using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.DPSBase.SevenZipLZMACompressor;

namespace nucs.Network.Discovery {
    public abstract class Nodes<T> : IDisposable where T : Node, new() {
        private readonly List<ConnectionListenerBase> __listeners = new List<ConnectionListenerBase>(0);

        protected Nodes() {
            try {
                KnownNodes.Add((T) typeof(T).GetProperty("This").GetValue(null,null));
            } catch (NullReferenceException) {
                throw new MissingMethodException($"{typeof(T).Name} class is missing a static get property named 'This' that returns a {typeof(T).Name} object.\n public static {typeof(T).Name} This{{ get {{...}}}}\n public static {typeof(T).Name} This => new {typeof(T).Name}();");
            }
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();
        }

        /// <summary>
        ///     List of all known nodes (including this machine).
        /// </summary>
        public NodesList<T> KnownNodes { get; } = new NodesList<T>();

        public T this[string key] {
            get { return KnownNodes.FirstOrDefault(c => c.IP == key); }
        }
        
        /// <summary>
        ///     Opens up for communication, searches for new nodes and so on.
        /// </summary>
        public OpeningResult Open() {
            var or = new OpeningResult();

            try {
                NetworkComms.AppendGlobalIncomingPacketHandler<NodesList<T>>("Discover", DiscoveryHandler);
            } catch (Exception e) {
                or.Exception = e;
                or.Successful = false;
                return or;
            }

            List<ConnectionListenerBase> _listeningto = null;
            try {
                if (Connection.AllExistingLocalListenEndPoints().Any(kv => kv.Key == ConnectionType.TCP && kv.Value.Any(ep => ((IPEndPoint) ep).Port == Ports.DiscoveryPort)) == false) {
                    _listeningto = Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, Ports.DiscoveryPort));
                } else {
                    _listeningto = Connection.AllExistingLocalListeners().Where(kv => kv.ConnectionType == ConnectionType.TCP && ((IPEndPoint) kv.LocalListenEndPoint).Port == Ports.DiscoveryPort).ToList();
                }
                __listeners.AddRange(_listeningto);
            } catch (Exception e) {
                or.Exception = e;
                or.Successful = false;
                return or;
            }
            var listeningto = _listeningto.Select(o => o.LocalListenEndPoint).Cast<IPEndPoint>().Select(@base => $"{@base.Address}:{@base.Port}").ToArray();

#if DEBUG
            listeningto.ForEach(Console.WriteLine);
#endif
            or.ListeningTo = listeningto.ToList();
            return or;
        }

        private void DiscoveryHandler(PacketHeader packetHeader, Connection connection, NodesList<T> nl) {
            try {
                KnownNodes.MergeInto(nl);
                connection.SendObject("DiscoverReply", KnownNodes);
#if DEBUG
                var ip = new Node(((IPEndPoint) connection.ConnectionInfo.RemoteEndPoint).Address.ToString());
                //Console.WriteLine($"{ip} has performed handshake");
#endif
            } catch {}
        }

        /// <summary>
        ///     Closes any listening connections if exists and removes handler for hello command.
        /// </summary>
        public void Close() {
            NetworkComms.RemoveGlobalIncomingPacketHandler("Discover");

            __listeners.ToList().ForEach(@base => {
                try {
                    Connection.StopListening(@base);
                } catch (Exception) {}
            });
            __listeners.RemoveWhere(@base => !@base.IsListening);
        }
        

        public DiscoveryResult<T> Discover(T node) {
            var dr = new DiscoveryResult<T>();
            try {
#if DEBUG
                var n = NetworkComms.SendReceiveObject<NodesList<T>, NodesList<T>>("Discover", node.IP, Ports.DiscoveryPort, "DiscoverReply", -1, KnownNodes);
#else
                var n = NetworkComms.SendReceiveObject<NodesList<T>, NodesList<T>>("Discover", node.IP, Ports.DiscoveryPort, "DiscoverReply", 10000, KnownNodes);
#endif
                dr.NodesList = n;
            } catch (Exception e) {
                dr.Exception = e;
                dr.Successful = false;
            }
            if (dr.Successful)
                KnownNodes.MergeInto(dr.NodesList);
            return dr;
        }

        /// <summary>
        ///     Syncs with all servers and known nodes.
        /// </summary>
        /// <returns></returns>
        public Task[] Sync() {
            return Sync(NodeServers.List
                .Select(s => new T {IP=s})
                .Concat(KnownNodes).ToArray());
        }

        /// <summary>
        ///     Starts sync progress with the given nodes and returns the tasks.
        /// </summary>
        public Task[] Sync(IEnumerable<T> nodes) {
            return nodes.Select(node => Task.Factory.StartNew(() => Discover(node))
                .ContinueWith(task => {
                    if (task.IsFaulted)
                        return;
                    if (task.Result.Successful)
                        KnownNodes.MergeInto(task.Result.NodesList);
                })).ToArray();
        }

        /// <summary>
        ///     Starts sync progress with the given nodes and returns the nodes found.
        /// </summary>
        public T[] SyncSerially() {
            return SyncSerially(NodeServers.List
                .Select(s => new T { IP = s })
                .Concat(KnownNodes).ToArray());
        }

        /// <summary>
        ///     Starts sync progress with the given nodes and returns the nodes found.
        /// </summary>
        public T[] SyncSerially(IEnumerable<T> nodes) {
            return nodes.Where(node => node.IP.CompareOrdinal(PCNode.This.IP) != 0).Select(node => Task.Factory.StartNew(() => Discover(node))
                .ContinueWith(task => {
                    if (task.IsFaulted)
                        return new T[0];
                    if (task.Result.Successful)
                        KnownNodes.MergeInto(task.Result.NodesList);
                    return task.Result.NodesList?.ToArray() ?? new T[0];
                })).SelectMany(task => task.Result).ToArray();
        }

        /*/// <summary>
        ///     Gets the online 
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<PCNode>> Online() {
            KnownNodes.ToArray().AsParallel().Select();
        }*/

        public void Dispose() {
            Close();
        }
    }
}