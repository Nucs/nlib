using ProtoBuf;

namespace nucs.Network.Discovery {

    [ProtoContract]
    public class Data<T> {
        [ProtoMember(2)]
        public T _Data { get; set; }

        [ProtoMember(1)]

        public int NodeHashcode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Data(int nodeHashcode, T data) {
            _Data = data;
            NodeHashcode = nodeHashcode;
        }
    }
    [ProtoContract]
    public class Data {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Data(int nodeHashcode, object data) {
            _Data = data;
            NodeHashcode = nodeHashcode;
        }

        [ProtoMember(2)]
        public object _Data { get; set; }
        /// <summary>
        /// The sending node hashcode used to identify it.
        /// </summary>
        [ProtoMember(1)]
        public int NodeHashcode { get; set; }

        public Data<T> ToGenericData<T>() {
            return new Data<T>(NodeHashcode, (T)_Data);
        }
    }
}