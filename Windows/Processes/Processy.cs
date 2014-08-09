using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using nucs.Forms;

namespace nucs.Windows.Processes {
    public static class Processy {
        /// <summary>
        ///     Customized Launcher allowing to open applications as administrator and handling some errors.
        /// </summary>
        /// <param name="file">path to file.</param>
        /// <param name="arguments">arguments for launching</param>
        /// <param name="forceShellAvoidance">attempt to avoid the annoying 'allowed application' question on startup</param>
        /// <param name="asAdmin">as administrator?</param>
        /// <exception cref="FileNotFoundException">Thrown when file is not found.</exception>
        /// <exception cref="Win32Exception">Unspecified Exception is thrown when file has not extension.</exception>
        public static Process Start(string file,string arguments = "", bool forceShellAvoidance = true, bool asAdmin = false) {
            Process p = null;
            if (File.Exists(file) == false) 
                throw new FileNotFoundException("File '"+file+"' Could not be found.", file);
        _retry:
            var pinfo = new ProcessStartInfo();
                pinfo.FileName = file;
                pinfo.WindowStyle = ProcessWindowStyle.Normal;
                pinfo.Verb = pinfo.Verbs.Any(v => v.Equals("runas", StringComparison.InvariantCultureIgnoreCase)) & asAdmin ? "runas" : "open";
                pinfo.UseShellExecute = !forceShellAvoidance && pinfo.Verb != "runas";
            if (arguments!="")
                pinfo.Arguments = arguments;
            try {
                p = Process.Start(pinfo);
            } catch (Win32Exception e) {
                //handling error:
                switch (e.NativeErrorCode) {
                    case 1223:
                        break; //cancelled by user
                    case 740: //app itself is manifested for admin, open regularly.
                        if (asAdmin == false)
                            throw e; //unexpected error
                        //otherwise retry.
                        asAdmin = false;
                        goto _retry;
                    case -2147467259:
                        throw new Win32Exception("Unspecified error on opening a file, possibly a file without extension...",e);
                    default: //unexpected error
                        throw e;
                }
            }
            return p;
        }

    }
}