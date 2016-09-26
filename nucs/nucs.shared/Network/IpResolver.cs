using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Network {
    public class IpResolver {
        public static List<string> ExternalIpServices = new List<string> {
            "http://myexternalip.com/raw",
            "https://api.ipify.org/",
            "http://bot.whatismyipaddress.com/",
            "http://icanhazip.com/",
            "http://ipinfo.io/ip"
        };

        /// <summary>
        ///     Checks if there is any interface that can connect to internet and only then attempts to open read to google
        /// </summary>
        /// <returns></returns>
        public static bool IsInternetConnectionAvailable() {
            return NetworkIsAvailable() && OpenReadToGoogle();
        }

        private static bool OpenReadToGoogle() {
            try {
                var _onlinechecker = new FastWebClient() { Timeout = 5000 };

                using (_onlinechecker.OpenRead("http://www.google.com")) { }
                return true;
            } catch (Exception) {
                return false;
            }
        }

        private static bool NetworkIsAvailable() {
            var all = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var item in all) {
                if (item.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                if (item.Name.ToLower().Contains("virtual") || item.Description.ToLower().Contains("virtual"))
                    continue; //Exclude virtual networks set up by VMWare and others
                if (item.OperationalStatus == OperationalStatus.Up)
                    return true;
            }
            return false;
        }
        private static readonly IpMine _ipmine = new IpMine();

        /// <summary>
        ///     Retrieves the external IP from any of the services in the ExternalIpServices list.
        /// </summary>
        public static string GetPublic(int timeout = -1, CancellationTokenSource src = null) {
            return _ipmine.MineFirstOrDefault(timeout, src);
        }
    }
}