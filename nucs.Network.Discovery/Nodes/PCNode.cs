using System;
using ProtoBuf;

namespace nucs.Network.Discovery {

    [ProtoContract]
    public class PCNode : Node {
        public PCNode(string ip) : base(ip){}
        public PCNode() : base(){}

        [ProtoMember(1)]
        public string MachineName { get; set; } = null;
        [ProtoMember(2)]
        public string MacAddress { get; set; } = null;


        private static PCNode _node_cache;

        /// <summary>
        ///     Returns Node represting this machine.
        ///     Might take some time.
        /// </summary>
        public new static PCNode This {
            get {
                if (_node_cache != null && string.IsNullOrEmpty(_node_cache.IP))
                    _node_cache.IP = IpResolver.GetPublic();
                return _node_cache 
                    ?? (_node_cache = new PCNode {
                        IP = IpResolver.GetPublic(),
                        MachineName = Environment.MachineName,
                        MacAddress = NetworkInterfaces.MacAddress,
                        LastContact = DateTime.Now.ToUniversalTime()
                    });
            }
        }

        protected bool Equals(PCNode other) {
            return base.Equals(other) && string.Equals(MacAddress, other.MacAddress) && string.Equals(MachineName, other.MachineName);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PCNode) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (MacAddress?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (MachineName?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (IP?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public static bool operator ==(PCNode left, PCNode right) {
            return Equals(left, right);
        }

        public static bool operator !=(PCNode left, PCNode right) {
            return !Equals(left, right);
        }

        public override string ToString() {
            return $"{MachineName} - {IP ?? "0.0.0.0"} - {MacAddress}";
        }
    }
}