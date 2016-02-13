using System.Linq;
using System.Net.NetworkInformation;

namespace nucs.Network {
    public class NetworkInterfaces {
        public static NetworkInterface GetActive() {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(a => a.OperationalStatus == OperationalStatus.Up)
                .Select(ni=>new {ni, stats=ni.GetIPv4Statistics()})
                .OrderByDescending(c=>c.stats.BytesReceived)
                .ThenByDescending(c=>c.stats.BytesSent)
                .FirstOrDefault()?.ni;
            
        }

        private static string _mac_cache = null;
        public static string MacAddress => _mac_cache ?? (_mac_cache=GetActive().Id.Trim('{','}'));
    }
}