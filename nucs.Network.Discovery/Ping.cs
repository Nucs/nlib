using System;
using System.Diagnostics;
using System.Net;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.UDP;

namespace nucs.Network.Discovery {
    public class Ping {
        public static int Port { get; set; } = 6;

        public static bool IsRunning { get; set; } = false;

        public static void StartPingService() {
            try {
                IsRunning = Connection.StartListening(ConnectionType.UDP, new IPEndPoint(IPAddress.Any, Port)).Count > 0;
            } catch (CommsSetupShutdownException e) when (e.Message.Contains($":{Port}")) {
                
            }
        }

        public static void Stop() {
            Connection.StopListening(ConnectionType.UDP, new IPEndPoint(IPAddress.Any, Port));
            IsRunning = false;
        }

        public static bool To(string ip, int timeout = 10000) {
            var conn = UDPConnection.GetConnection(new ConnectionInfo(ip, Port), UDPOptions.Handshake, true, false);
            try {
                conn.EstablishConnection();
            } catch (Exception e) {
                return false;
            }
            conn.CloseConnection(false);
            return true;
        }

        public static long ToMeasure(string ip, int timeout = 10000) {
            var sw = new Stopwatch();
            sw.Start();
            var res = To(ip, timeout);
            return res==true ? sw.ElapsedMilliseconds : -1;
        }

    }
}