using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nucs.Network {
    [TestClass]
    public class IpResolverTests {

        static IpResolverTests() {
            
        }

        [TestMethod]
        public void AsyncResolvePublicIpTest() {
            var n = new FastHttpClient(); //init settings
            Thread.Sleep(3000);
            var sw = new Stopwatch();
            sw.Start();
            Assert.IsNotNull(IpResolver.GetPublic());
            Debug.WriteLine(sw.ElapsedMilliseconds);
        }

        
        [TestMethod]
        public void MultiAsyncResolvePublicIpTest() {
            var l = new List<long>();
            var n = new FastHttpClient(); //init settings
            Thread.Sleep(3000);
            var sw = new Stopwatch();
            for (int i = 0; i < 20; i++) {
                sw.Restart();
                Assert.IsNotNull(IpResolver.GetPublic());
                Debug.WriteLine(sw.ElapsedMilliseconds);
                l.Add(sw.ElapsedMilliseconds);
            }
            Debug.WriteLine("Average: "+l.Average());
        }

        [TestMethod]
        public void TestAllPublicIpSources() {
            var l = IpResolver.ExternalIpServices;
            var h = new HttpClient();
            foreach (var url in l) {
                var r = attemptparse(h.GetStringAsync(url).Result);
                Assert.IsNotNull(r, url);
                Debug.WriteLine(url);
            }
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
