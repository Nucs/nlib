using System;
using System.Net;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
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

        #region Send & Receive Implementation
        
        /// <summary>
        ///     Connects to the node through Discovery port.
        /// </summary>
        public TCPConnection Connect {
            get {
                try {
                    return TCPConnection.GetConnection(new ConnectionInfo(this.IP, 35555));
                } catch { return null; }
            }
        }

        public void Send<T>(T o) {
            if (Node.This == this)
                throw new InvalidOperationException("Can't send data to self.");

            this.Connect.SendObject("data"+this.GetHashCode(), new Data<T>(This.GetHashCode(), o));
        }

        public void Send<T>(string packetName, T o) {
            if (Node.This == this)
                throw new InvalidOperationException("Can't send data to self.");

            this.Connect.SendObject(packetName, o);
        }

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