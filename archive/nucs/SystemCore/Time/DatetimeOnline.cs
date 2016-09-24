using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace TFer.Server.Tools {
    public static class DatetimeOnline {
        public static List<string> Servers = new List<string>(new [] {
                "nist.netservicesgroup.com",
                "time-a.nist.gov",
                "time-b.nist.gov",
                "nist1-ny.ustiming.org",
                "nist1-nj.ustiming.org",
                "nist1-pa.ustiming.org",
                "nist1.aol-va.symmetricom.com",
                "nist1.columbiacountyga.gov",
                "nist1-chi.ustiming.org",
                "nist.expertsmi.com",
            });


        private static DateTime _nextfetch = DateTime.MinValue;
        private static DateTime _result = DateTime.MinValue;
        /// <summary>
        ///     Retrieves the datetime from the internet.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetFastestNISTDate(bool timedcache = false) {
            if (Servers.IsEmpty()) return DateTime.Now;
#if DEBUG
            return DateTime.Now;
#endif
            if (timedcache) {
                if (_nextfetch==DateTime.MinValue)
                    _nextfetch = DateTime.Now + TimeSpan.FromMinutes(30);
                if (DateTime.Now > _nextfetch) {
                    _result = DateTime.MinValue;
                }
                if (_result==DateTime.MinValue == false)
                    return _result;
            }
            var result = DateTime.MinValue;

            // Initialize the list of NIST time servers
            // http://tf.nist.gov/tf-cgi/servers.cgi
            

            // Try 5 servers in random order to spread the load
            var rnd = new Random();
            foreach (var server in Servers.ToArray())
                try {
                    // Connect to the server (at port 13) and get the response
                    var serverResponse = string.Empty;
                    using (var reader = new StreamReader(new TcpClient(server, 13).GetStream()))
                        serverResponse = reader.ReadToEnd();

                    // If a response was received
                    if (!string.IsNullOrEmpty(serverResponse)) {
                        // Split the response string ("55596 11-02-14 13:54:11 00 0 0 478.1 UTC(NIST) *")
                        var tokens = serverResponse.Split(' ');

                        // Check the number of tokens
                        if (tokens.Length >= 6) {
                            // Check the health status
                            var health = tokens[5];
                            if (health == "0") {
                                // Get date and time parts from the server response
                                var dateParts = tokens[1].Split('-');
                                var timeParts = tokens[2].Split(':');

                                // Create a DateTime instance
                                var utcDateTime = new DateTime(
                                    Convert.ToInt32(dateParts[0]) + 2000,
                                    Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]),
                                    Convert.ToInt32(timeParts[0]), Convert.ToInt32(timeParts[1]),
                                    Convert.ToInt32(timeParts[2]));

                                // Convert received (UTC) DateTime value to the local timezone
                                result = utcDateTime.ToLocalTime();
                                Servers.Remove(server);
                                Servers.Insert(0, server);
                                return _result=result;
                                // Response successfully received; exit the loop
                            }
                        }
                    }
                }
                catch {
                    Servers.Remove(server);
                    // Ignore exception and try the next server
                }
            return result;
        }
    }
}