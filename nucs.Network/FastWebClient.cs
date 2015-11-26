using System;
using System.Net;
using System.Text;
using System.Threading;

namespace nucs.Network {
    /// <summary>
    ///     A webclient with customized config to run 200% faster than normal webclient.
    /// </summary>
    public class FastWebClient : WebClient {
        static FastWebClient() {
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

        public FastWebClient() {
            Encoding = Encoding.UTF8;
            Proxy = null;
            //this.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        }

        public uint Timeout { get; set; } = 10000;

        protected override WebRequest GetWebRequest(Uri address) {
            var req = (HttpWebRequest) base.GetWebRequest(address);
            req.Timeout = Convert.ToInt32(Timeout);
            req.Proxy = null;
            return req;
        }
    }
}