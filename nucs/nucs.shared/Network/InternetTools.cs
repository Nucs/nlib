using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#if NET45
using System.Threading.Tasks;
using System.Threading;
#else

#endif

namespace nucs.Network {
    public static class InternetTools {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int conn, int val);

        /// <summary>
        ///     Uses 'bool InternetGetConnectedState(out int conn, int val)' pinvoke to check if computer has internet connection.
        /// </summary>
        /// <returns></returns>
        public static bool HasInternetConnection() {
            int Out;
            return InternetGetConnectedState(out Out, 0) && NetworkInterface.GetIsNetworkAvailable();
        }

        /// <summary>
        ///     Peforms a ping towards an <see cref="IPAddress" />
        /// </summary>
        /// <param name="address">The <see cref="IPAddress" /> to ping</param>
        /// <param name="times">how many times to ping, must be times > 0</param>
        /// <param name="verifyLocalInternetConnection">
        ///     Force checking of computer's internet connection, if false -> returns
        ///     automatically false
        /// </param>
        /// <returns>If the server has responded</returns>
        public static bool TestHostConnection(IPAddress address, int times, bool verifyLocalInternetConnection = false) {
            if (times <= 0)
                throw new ArgumentException("You can't perform " + times + " times a test.. wtf bro");
            if (address == null)
                return false;
            var returnMessage = string.Empty; //string to hold our return messge
            var pingOptions = new PingOptions(128, true); //set the ping options, TTL 128 
            var ping = new Ping(); //create a new ping instance
            var buffer = new byte[32]; //32 byte buffer (create empty)
            var result = false;
            //first make sure we actually have an internet connection
            if (verifyLocalInternetConnection && HasInternetConnection() == false) {
                Console.WriteLine("HCT: " + "No Internet connection found...");
                return false;
            }
            //here we will ping the host
            for (var i = 0; i < times; i++)
                try {
                    //send the ping 4 times to the host and record the returned data.
                    //The Send() method expects 4 items:
                    //1) The IPAddress we are pinging
                    //2) The timeout value
                    //3) A buffer (our byte array)
                    //4) PingOptions'
                    PingReply pingReply;
                    try {
                        pingReply = ping.Send(address, 1000, buffer, pingOptions);
                    }
                    catch (Exception e) {
                        Console.WriteLine("HCT: " + string.Format("Connection Error: {0}", e.Message));
                        result = false;
                        continue;
                    }

                    //make sure we dont have a null reply
                    if (pingReply != null)
                        switch (pingReply.Status) {
                            case IPStatus.Success:
                                try {
                                    Console.WriteLine("HCT: " +
                                                      string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}",
                                                          pingReply.Address, pingReply.Buffer.Length,
                                                          pingReply.RoundtripTime,
                                                          (pingReply.Options == null
                                                              ? "-1"
                                                              : pingReply.Options.Ttl.ToString())));
                                    result = true;
                                }
                                catch (Exception) {
                                    result = false;
                                }
                                break;
                            case IPStatus.TimedOut:
                                Console.WriteLine("HCT: " + "Connection has timed out...");
                                result = false;
                                break;
                            default:
                                Console.WriteLine("HCT: " + string.Format("Ping failed: {0}", pingReply.Status));
                                result = false;
                                break;
                        }
                    else {
                        Console.WriteLine("HCT: " + "Connection failed for an unknown reason...");
                        result = false;
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("HCT: " + string.Format("Connection Error: {0}", ex.Message));
                    result = false;
                }
            return result;
        }

        /// <summary>
        ///     Equalivent to Dns.GetHostAddresses(txt), but returned async.
        /// </summary>
        /// <param name="_ip">The IP address</param>
        /// <returns>List of aliases to this IP</returns>
        public static Task<List<IPAddress>> GetDnsAliases(string _ip) {
            if (string.IsNullOrEmpty(_ip)) return null;
            if (_ip == "localhost" || _ip == "::1") _ip = "127.0.0.1"; //GetHostAddresses doesn't recognize localhost.

            var t = Tasky.Run(() => {
                try {
                    return Dns.GetHostAddresses(_ip).ToList();
                }
                catch (Exception) {
                    return null;
                }
            });
            return t;
        }

        /// <summary>
        ///     Collects the IP Aliases to the given '_ip' then attempt to ping it for 'pingAttempts' times and return succesful
        ///     pings
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="pingAttempts"></param>
        /// <returns></returns>
#if NET45
        public static async Task<List<IPAddress>> GetConnectableAliases(string _ip, int pingAttempts = 1) {
            var ips = await GetDnsAliases(_ip);
            if (ips == null || ips.Count == 0)
                return null;
            return ips.AsParallel().Where(i => TestHostConnection(i, pingAttempts)).ToList();
#else
        public static Task<List<IPAddress>> GetConnectableAliases(string _ip, int pingAttempts = 1) {
            return Tasky.Run(() => {
                var ips = GetDnsAliases(_ip).Result;
                if (ips == null || ips.Count == 0)
                    return null;
#if !(NET35 || NET3 || NET2)
                        return ips.AsParallel().Where(i => TestHostConnection(i, pingAttempts)).ToList();
#else
                return ips.Where(i => TestHostConnection(i, pingAttempts)).ToList();
#endif
            });
#endif
        }
    }
}