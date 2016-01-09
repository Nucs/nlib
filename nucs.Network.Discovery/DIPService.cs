using System;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Network.Discovery {

    /// <summary>
    ///     Provides a service to automatically discover and connect to the DIP network.
    /// </summary>
    public static class DIPService {
        public static event Action<PCNode> NewNodeFound;

        public static bool IsRunning { get; set; } = false;
        public static Thread ServiceThread { get; private set; } = new Thread(ServiceCore) {Name = "DIPService", IsBackground = true};

        private static ManualResetEventSlim _markstop;
        public static readonly PCNodes Nodes = new PCNodes();
        public static void Start() {
            var res = Nodes.Open();
            if (!res.Successful)
                throw res.Exception;
            IsRunning = true;
            _markstop = new ManualResetEventSlim(false); 
            ServiceThread.Start(_markstop);
        }

        public static void Stop() {
            Ping.Stop();
            Nodes.Close();
            _markstop?.Set();
        }


        private static void ServiceCore(object o) {
            var local_stop = o as ManualResetEventSlim;
            while (true) {
                if (local_stop == null || local_stop.IsSet) //marked to stop.
                    break;

                Nodes.SyncSerially();
                Thread.Sleep(60000);
            }
        }

    }
}