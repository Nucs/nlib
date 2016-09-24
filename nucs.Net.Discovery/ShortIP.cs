using System.Collections.Generic;
using System.Net.Sockets;

namespace nucs.Net.Discovery {
    /// <summary>
    ///     Short representation of an ip w/ port
    /// </summary>
    public class ShortIP {

        public int Port { get; }
        public string IP { get; }

        public ShortIP() {}

        public ShortIP(string ip) {
            IP = ip;
        }

        public ShortIP(string ip, int port) {
            Port = port;
            IP = ip;
        }



        public override string ToString() {
            return $"{IP}:{Port}";
        }

        #region Credentials

        protected bool Equals(ShortIP other) {
            return Port == other.Port && string.Equals(IP, other.IP);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ShortIP) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Port*397) ^ (IP?.GetHashCode() ?? 0);
            }
        }

        private sealed class PortIpEqualityComparer : IEqualityComparer<ShortIP> {
            public bool Equals(ShortIP x, ShortIP y) {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Port == y.Port && string.Equals(x.IP, y.IP);
            }

            public int GetHashCode(ShortIP obj) {
                unchecked {
                    return (obj.Port*397) ^ (obj.IP?.GetHashCode() ?? 0);
                }
            }
        }

        public static IEqualityComparer<ShortIP> PortIpComparer { get; } = new PortIpEqualityComparer();
        public TcpClient Client => new TcpClient(IP, Port);

        #endregion
    }
}