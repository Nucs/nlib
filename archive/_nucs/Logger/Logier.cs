using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.Logger {
    public class Logier {

        public Logier() {
        }

        public Logier(string filename) {
            Logy.Filename = filename;
        }

        public Logier(string filename, Logy.PrefixGeneratorHandle prefix) :this(filename) {
            Logy.PrefixGenerator = prefix;
        }

        public void Trace(string message) {
            Logy.Log("[Trace]",message);
        }

        public void Debug(string message) {
            Logy.Log("[Debug]", message);
            
        }

        public void Fatal(string message) {
            Logy.Log("[Fatal]", message);
        }

        public void Fatal(string message, Exception ex) {
            Logy.Log("[Fatal]", message,"\n",ex.ToString());
        }

        public void Info(string message) {
            Logy.Log("[Info]", message);
        }

        public void Warn(string message) {
            Logy.Log("[Warn]", message);
        }

        public void Error(string message) {
            Logy.Log("[Error]", message);
        }

        public void Shutdown() {
            Logy.Log("[Shutdown]", "System has shut down.");
        }
    }
}
