using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.SystemCore.Settings {
    public interface ISaveable {
        void Save(string filename);
        void Save();
    }
}
