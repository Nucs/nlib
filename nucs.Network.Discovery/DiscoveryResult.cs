using System;
using System.Collections.Generic;

namespace nucs.Network.Discovery {
    public class DiscoveryResult<T> where T : Node {
        public Exception Exception { get; set; } = null;

        public NodesList<T> NodesList { get; set; } = null;

        public bool Successful { get; set; } = true;

        public override string ToString() {
            return $"Successful={Successful}; Nodes={NodesList.Count}; Exception={Exception?.Message ?? "null"}";
        }
    }
}