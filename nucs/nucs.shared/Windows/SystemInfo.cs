﻿#if !NETSTANDARD2_0
using System;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;
using System.Threading;
using nucs.Windows.Registry;

namespace nucs.Windows {
    static class SystemInfo {
        public static bool IsAdministrator = IsCurrentUserAdministrator();

        internal static string GetWindowsProductId() {
            return RegistryUtils.Read(@"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProductId");
        }

        public static string GetSystemInfoSummary() {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb)) {
                sw.WriteLine("Windows Product ID: {0}", GetWindowsProductId());
                sw.WriteLine("MachineName: {0}", Environment.MachineName);
                sw.WriteLine("Windows Version: {0}", GetWindowsVersion());
                sw.WriteLine("ProcessorCount: {0}", Environment.ProcessorCount);
                sw.WriteLine("UserDomainName: {0}", Environment.UserDomainName);
                sw.WriteLine("UserName: {0}", Environment.UserName);
                sw.WriteLine("Version: {0}", Environment.Version);

                foreach (var drive in DriveInfo.GetDrives()) {
                    if (drive.DriveType != DriveType.Fixed) continue;
                    sw.WriteLine("Drive: {0}", drive.Name);
                    sw.WriteLine("Label: {0}", drive.VolumeLabel);
                    sw.WriteLine("Size : {0}", drive.TotalSize);
                }
            }
            return sb.ToString();
        }

        public static string GetMacAddress() {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces()) {
                if (nic.OperationalStatus != OperationalStatus.Up) continue;
                return nic.GetPhysicalAddress().ToString();
            }

            return string.Empty;
        }

        private static bool IsCurrentUserAdministrator() {
            try {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            } catch {
                return false;
            }
        }

        public static string GetWindowsVersion() {
            var os = Environment.OSVersion;
            var windows = "Unknown";

            switch (os.Platform) {
                case PlatformID.Win32Windows:
                    switch (os.Version.Minor) {
                        case 0:
                            windows = "Windows 95";
                            break;
                        case 10:
                            windows = "Windows 98";
                            break;
                        case 90:
                            windows = "Windows ME";
                            break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (os.Version.Major) {
                        case 3:
                            windows = "Windows NT 3.51";
                            break;
                        case 4:
                            windows = "Windows NT 4.0";
                            break;
                        case 5:
                            windows = os.Version.Minor == 0
                                ? "Windows 2000"
                                : "Windows XP";
                            break;
                        case 6:
                            switch (os.Version.Minor) {
                                case 0:
                                    windows = "Windows Vista";
                                    break;
                                case 1:
                                    windows = "Windows 7";
                                    break;
                                case 2:
                                    windows = "Windows 8";
                                    break;
                                case 3:
                                    windows = "Windows 8.1";
                                    break;
                            }
                            break;
                        case 10:
                            windows = "Windows 10";
                            break;
                    }
                    break;
            }

            var processor = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432");
            return windows + (string.IsNullOrEmpty(processor) ? " x86" : " x64");
        }

        public static string GetCPUID() {
            ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_Processor WHERE DeviceID = 'CPU0'");
            string sData = string.Empty;
            foreach (ManagementObject tObj in mSearcher.Get()) {
                sData = Convert.ToString(tObj["ProcessorId"]);
            }
            return sData;
        }

        public static string GetMoboSerial() {
            ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_BaseBoard");
            string sData = string.Empty;
            foreach (ManagementObject tObj in mSearcher.Get()) {
                sData = Convert.ToString(tObj["SerialNumber"]);
            }
            return sData;
        }

        public static string GetGraphicDevice() {
            ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_VideoController");
            string sData = string.Empty;
            foreach (ManagementObject tObj in mSearcher.Get()) {
                sData = Convert.ToString(tObj["Description"]);
            }
            return sData;
        }
    }
}

#endif