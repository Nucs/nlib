using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        ///     Retrieves the external IP from any of the services in the ExternalIpServices list.
        /// </summary>
        public static string GetPublic() {
            var h = new FastHttpClient();
            var l = ExternalIpServices.Select(url => h.GetStringAsync(url)).ToArray();
            _rewait:
            if (l.Length == 0)
                return null;
            Task.WaitAny(l.ToArray());

            var _finished = l.Where(task => task.IsFaulted == false && task.IsCompleted);
            var finished = _finished.Select(r => attemptparse(r.Result))
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();

            l = l.Where(t => t.IsCompleted == false && t.IsFaulted == false).ToArray();
            if (finished.Length == 0)
                goto _rewait;
            return finished.FirstOrDefault();
        }

        /// <summary>
        ///     Retrieves the external IP from any of the services in the ExternalIpServices list.
        /// </summary>
        public async static Task<string> GetPublicAsync() {
            return await Task.Run(() => GetPublic());
        }

        private static string attemptparse(string ip) {
            ip = new string(ip.Trim().Where(c=>char.IsDigit(c) || c=='.').ToArray());
            IPAddress _ip;
            var b = IPAddress.TryParse(ip, out _ip);
            if (b == false)
                return null;
            return _ip.ToString();
        }
    }
}