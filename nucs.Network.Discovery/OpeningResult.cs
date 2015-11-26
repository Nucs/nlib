using System;
using System.Collections.Generic;

namespace nucs.Network.Discovery {
    public class OpeningResult {
        public Exception Exception { get; set; } = null;
        
        public List<string> ListeningTo { get; set; } = new List<string>();

        public bool Successful { get; set; } = true;

        public override string ToString() {
            return $"Successful={Successful}; Listeners={ListeningTo.Count}; Exception={Exception?.Message ?? "null"}";
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}