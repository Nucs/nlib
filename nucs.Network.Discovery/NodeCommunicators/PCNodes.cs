using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;

namespace nucs.Network.Discovery {
    public class PCNodes : Nodes<PCNode> {
        public PCNode this[string key] {
            get {
                return key.Count(c=>c=='.')==3 
                    ? KnownNodes.FirstOrDefault(c => c.IP == key) 
                    : KnownNodes.FirstOrDefault(c => c.MachineName == key || c.MacAddress == key);
            }
        }
    }
}