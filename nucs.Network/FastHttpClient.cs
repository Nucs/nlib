using System;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace nucs.Network {
    /// <summary>
    ///     A webclient with customized config to run 200% faster than normal webclient.
    /// </summary>
    public class FastHttpClient : HttpClient {
        static FastHttpClient() {
            WebRequest.DefaultWebProxy = null;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            int minWorker, minIOC;
            // Get the current settings.
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            // Change the minimum number of worker threads to four, but
            // keep the old setting for minimum asynchronous I/O 
            // completion threads.
            ThreadPool.SetMinThreads(50, minIOC); //50 threads to handle async tasks.
        }

        public FastHttpClient(int timeout) {
            if (timeout > 0)
                Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        public FastHttpClient() : this(-1) {}
    }
}