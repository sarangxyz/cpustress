using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace cpustress
{
    internal static class ProcessorInfo
    {
        //[DllImport("ntdll.dll", SetLastError = true)]
        //private static extern bool NtQuerySystemInformation(out _SYSTEM_INFO sysInfo);
    }

    internal static class SystemInfo
    {

        [StructLayout(LayoutKind.Sequential)]
        internal struct _SYSTEM_INFO
        {
            public short wProcessorArchitecture;
            public short wReserved;
            public int dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public int dwNumberOfProcessors;
            public int dwProcessorType;
            public int dwAllocationGranularity;
            public short wProcessorLevel;
            public short wProcessorRevision;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetSystemInfo(out _SYSTEM_INFO sysInfo);

        public static int GetNumProcessors()
        {
            return Environment.ProcessorCount;
        }

        public static Int64 GetPhysicalMemory()
        {
            return PerformanceInfo.GetTotalMemoryInMiB();
        }

        public static Int64 GetAvailablePhysicalMemory()
        {
            return PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
        }

        public static string GetComputerName()
        {
            return System.Environment.MachineName;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetProductInfo(int dwOSMajorVersion, int dwOSMinorVersion, int dwSpMajorVersion,
                                                  int dwSpMinorVersion, out int pdwReturnedProductType);

        public static string GetProductType()
        {
            int prodTypeNum = 0;

            switch (prodTypeNum)
            {
                case 4:
                    return "Windows 10 Enterprise";
                case 70:
                    return "Windows 10 Enterprise E";
                default:
                    return "Windows - someversion";
            }
        }

        public static bool Is64BitOperatingSystem()
        {
            return Environment.Is64BitOperatingSystem;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class _OSVERSIONINFOEX
        {
            public _OSVERSIONINFOEX()
            {
                dwOSVersionInfoSize = (int)Marshal.SizeOf(this);
            }

            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetVersionEx(out _OSVERSIONINFOEX osVerEx);
        public static string GetOSName()
        {
            System.OperatingSystem osInfo = Environment.OSVersion;
            return osInfo.VersionString;
        }

        public static void GetNumProcessesAndThreads(out int numProcess, out int numThreads)
        {
            PerformanceInfo.GetNumProcessAndThreads(out numProcess, out numThreads);
        }
    }
}


