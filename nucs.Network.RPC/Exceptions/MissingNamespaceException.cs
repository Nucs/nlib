using System;
using System.Runtime.Serialization;

namespace nucs.Network.RPC {
    [Serializable]
    public class MissingNamespaceException : Exception {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MissingNamespaceException() {
        }

        public MissingNamespaceException(string message) : base(message) {
        }

        public MissingNamespaceException(string message, Exception inner) : base(message, inner) {
        }

        protected MissingNamespaceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {
        }
    }
}