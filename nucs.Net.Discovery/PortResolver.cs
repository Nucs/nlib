using System;
using System.Linq;

namespace nucs.Net.Discovery {
    public static class PortResolver {
        public static int Resolve(string IP = "Local IP") {
            if (IP == "Local IP")
                IP = GetPublicIP();
            var port = Convert.ToInt64(new string(IP.ToCharArray().Where(char.IsDigit).ToArray()));
            return Convert.ToInt16(port%ushort.MaxValue);
        }


        private static string GetPublicIP() {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }
    }
}
}