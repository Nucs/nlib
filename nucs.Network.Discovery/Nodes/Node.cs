using System;
using System.Net;
using NetworkCommsDotNet;
using ProtoBuf;

namespace nucs.Network.Discovery {
    [ProtoContract]
    [ProtoInclude(500, typeof(PCNode))]
    public class Node : IEquatable<Node> {
        /// <summary>
        ///     IP Address of the node
        /// </summary>
        [ProtoMember(1)]
        public string IP { get; set; }

        [ProtoMember(2)]
        public DateTime LastContact { get; set; } = DateTime.MinValue;
        #region Constructors

        public Node(string ip) {
            if (IPAddress.Parse(ip) == null)
                throw new ArgumentException("Given IP is invalid.");
            IP = ip;
        }

        public Node() {}

        #endregion

        /// <summary>
        ///     Returns Node represting this machine.
        ///     Might take some time.
        /// </summary>
        public static Node This => new Node { IP = IpResolver.GetPublic(),  };

        #region Override

        public static explicit operator IPAddress(Node node) {
            return IPAddress.Parse(node.IP ?? "0.0.0.0");
        }

        #region Equality

        public bool Equals(Node other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return String.Equals(IP, other.IP, StringComparison.InvariantCulture);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((Node) obj);
        }

        public override int GetHashCode() {
            return (IP != null ? StringComparer.InvariantCulture.GetHashCode(IP) : 0);
        }

        public static bool operator ==(Node left, Node right) {
            return Equals(left, right);
        }

        public static bool operator !=(Node left, Node right) {
            return !Equals(left, right);
        }

        #endregion

        public override string ToString() {
            return IP ?? "0.0.0.0";
        }

        #endregion
    }
}