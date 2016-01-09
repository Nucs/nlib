using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace nucs.Network.Discovery {
    [Serializable]
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class NodesList<T> : HashSet<T> where T : Node {
        private readonly object _sync = new object();

        /// <summary>
        ///     Inserts the other list into this list.
        /// </summary>
        /// <param name="anotherlist"></param>
        public void MergeInto(NodesList<T> anotherlist) {
            lock (_sync)
                foreach (var node in anotherlist.Where(n => n?.IP != null))
                    base.Add(node);
        }
        public T this[string key] {
            get {
                return this.FirstOrDefault(c => c.IP == key);
            }
        }

        public new void Add(T n) {
            lock (_sync) {
                base.Add(n);
            }
        }
    }
}