using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nucs.Windows {

    /// <summary>
    /// Contains methods that corporates with Window. e.g. starting programs silently.
    /// </summary>
    public static class Processer {

        /// <summary>
        /// Starts flowlessly an app
        /// </summary>
        /// <param name="filepath">The path to the targeted app including filename and extension.</param>
        /// <param name="windowless">Should it be invisible?</param>
        public static Process Start(string filepath, bool windowless = false) {
            if (File.Exists(filepath) == false)
                throw new IOException("File doesn't exist on: "+filepath);
            var a = new Process { StartInfo = new ProcessStartInfo(filepath) {UseShellExecute = false, CreateNoWindow = windowless} };
            a.Start();
            return a;
        }

    }
}
