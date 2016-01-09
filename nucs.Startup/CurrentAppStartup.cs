using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using nucs.Filesystem;
using nucs.SystemCore.Boolean;

namespace nucs.Startup {

    /// <summary>
    ///     Manages startup for the current startup mode.
    /// </summary>
    public static class CurrentAppStartup {
        /// <summary>
        ///     Is current running application is attached to startup.
        /// </summary>
        public static bool IsAttached => StartupManager.Enumerate(fc => fc.FileInfo.FullName.SequenceEqual(Paths.ExecutingExe.FullName)).Any();

        /// <summary>
        ///     Attached the current running exe to startup
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static StartupManager.StartupAttachResult Attach(string alias=null) {
            return StartupManager.AttachBestNative(new FileCall(Paths.ExecutingExe), alias);
        }

        /// <summary>
        ///     Disattaches the current running exe to startup
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static bool Disattach(string alias=null) {
            return StartupManager.DisattachNative(Paths.ExecutingExe);
        }

    }
}