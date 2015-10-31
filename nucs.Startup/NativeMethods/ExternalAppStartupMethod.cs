using System.Collections.Generic;
using System.IO;

namespace nucs.Startup.NativeMethods {
    public class ExternalAppStartupMethod {
        //todo
        public bool Attach(FileInfo file, string alias) {
            throw new System.NotImplementedException();
        }

        public bool Disattach(FileInfo file) {
            throw new System.NotImplementedException();
        }

        public bool IsAttached(string alias) {
            throw new System.NotImplementedException();
        }

        public bool IsAttached(FileInfo file) {
            throw new System.NotImplementedException();
        }

        public bool IsAttachable {
            get { throw new System.NotImplementedException(); }
        }

        public IEnumerable<FileInfo> Attached {
            get { throw new System.NotImplementedException(); }
        }

        public uint Priority {
            get { throw new System.NotImplementedException(); }
        }
    }
}