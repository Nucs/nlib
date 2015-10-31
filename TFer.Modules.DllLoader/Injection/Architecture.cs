using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TFer.Modules.DllLoader.Injection {
    public static class Architecture {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        /// <summary>
        ///     Checks if target process is 64bit
        /// </summary>
        public static bool IsProcess64bit(Process proc) {
            return IsProcess64bit(proc.Id);
        }

        /// <summary>
        ///     Checks if target process ID is 64bit.
        /// </summary>
        public static bool IsProcess64bit(int procid) {
            if (!Is64BitOperatingSystem)
                return true;

            IntPtr processHandle;

            try { 
                processHandle = Process.GetProcessById(procid).Handle;
            } catch {
                return false; // access is denied to the process
            }

            bool retVal;
            return IsWow64Process(processHandle, out retVal) && retVal;
        }

        public static readonly bool Is64BitOperatingSystem = (IntPtr.Size == 8) || internalCheckIsWow64();

        private static bool internalCheckIsWow64() {
            if ((Environment.OSVersion.Version.Major != 5 || Environment.OSVersion.Version.Minor < 1) 
                && Environment.OSVersion.Version.Major < 6)
                return false;
            using (var p = Process.GetCurrentProcess()) {
                bool retVal;
                return IsWow64Process(p.Handle, out retVal) && retVal;
            }
        }
    }
}