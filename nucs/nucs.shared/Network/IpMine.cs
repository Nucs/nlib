using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using nucs.Threading;
using nucs.Threading.Mining;

namespace nucs.Network {
    public class IpMine : MiningFactory<string, string> {
        protected override string Mine(string url, CancellationToken token) {
            var h = new FastWebClient() { Timeout = -1 };
            return attemptparse(h.DownloadString(new Uri(url)));
        }

        public override IEnumerable<string> Mines() {
            return IpResolver.ExternalIpServices;
        }

        /// <summary>
        ///     Called before mining, return true to start mining or false to abort mning and return default(OUT)
        /// </summary>
        /// <returns></returns>
        public override bool PriorToMining() {
            return IpResolver.IsInternetConnectionAvailable();
        }

        private static string attemptparse(string ip) {
            ip = new string(ip.Trim().Where(c => char.IsDigit(c) || c == '.').ToArray());
            IPAddress _ip;
            var b = IPAddress.TryParse(ip, out _ip);
            if (b == false)
                return null;
            return _ip.ToString();
        }
    }
}