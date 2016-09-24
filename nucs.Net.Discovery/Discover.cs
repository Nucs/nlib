using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace nucs.Net.Discovery {
    public class Discover : IDiscovery {

        public List<ShortIP> Known { get; set; } = new List<ShortIP>();

        public List<ShortIP> Discover(ShortIP ip) {
            TcpClient client = ip.Client;
            try {
                Stream s = client.GetStream();
                StreamReader sr = new StreamReader(s);
                StreamWriter sw = new StreamWriter(s) {AutoFlush = true};
                var data = sr.ReadToEnd();

                s.Close();
            } finally {
                client.Close();
            }
        }

        public void Listen() {
            var p = new TcpListener(IPAddress.Any, PortResolver.Resolve());
            p.Start();
#if LOG
            Console.WriteLine("Server mounted, 
                            listening to port 2055");
#endif

            for (int i = 0; i < 4; i++)
            {
                Thread t = new Thread(Service);
                t.Start();
            }
        }

        public static void Main(){
      
    }
    public static void Service(object o) {
        var lis = o as TcpListener;
        if (lis == null) return;
        while(true) {
            Socket soc = lis.AcceptSocket();
            #if LOG
                Console.WriteLine("Connected: {0}", soc.RemoteEndPoint);
            #endif
            try {
                Stream s = new NetworkStream(soc); 
                StreamReader sr = new StreamReader(s);
                StreamWriter sw = new StreamWriter(s);
                sw.AutoFlush = true; // enable automatic flushing
                sw.Write(string.Join("@",Known.ToArray()));
                s.Close();
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            soc.Close();
        }
    }
    }
}