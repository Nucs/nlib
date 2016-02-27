using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace nucs.Identifiers
{
    public static class Identity {

        private static Guid _thiscache = Guid.Empty;

        /// <summary>
        ///     Gets the identity of this computer
        /// </summary>
        public static Guid This {
            get {
                if (_thiscache == Guid.Empty) { //generate
                    var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "identity.guid");
                    if (File.Exists(dir))
                        _thiscache = new Guid(File.ReadAllText(dir));
                    else
                        File.WriteAllText(dir, (_thiscache=Guid.NewGuid()).ToString());
                }
                return _thiscache;
            }
        }
    }
}
