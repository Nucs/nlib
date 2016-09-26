using System.Net;
using System.Threading;

namespace nucs.Network {
    public static class MultithreadedNetworking {
        private static bool hasset = false;
        /// <summary>
        /// Sets up network settings to promise highest startholder performance.
        /// </summary>
        /// <param name="multithreads">The amount of threads to be ready for new tasks</param>
        public static void SetupSettings(int multithreads = 50) {
            if (hasset)
                return;
            hasset = true;
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

    }
}