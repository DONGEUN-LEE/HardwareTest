using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

[SupportedOSPlatform("Windows")]
class SystemInfoWindows : SystemInfoBase
{
    [SecurityCritical]
    public override MemoryInfo GetMemoryInfo()
    {
        try
        {

            var memoryStatusEx = MEMORYSTATUSEX.New();

            GlobalMemoryStatusEx(ref memoryStatusEx);
            var metrics = new MemoryInfo
            {
                Total = Math.Round(memoryStatusEx.ullTotalPhys / 1024.0, 0),
                Free = Math.Round(memoryStatusEx.ullAvailPhys / 1024.0, 0),
            };
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }
        catch
        {
            // ignored
            return new MemoryInfo();
        }
    }

    #region  extern
    [SecurityCritical]
    [DllImport("Kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    private struct MEMORYSTATUSEX
    {
        internal uint dwLength;
        internal uint dwMemoryLoad;
        internal ulong ullTotalPhys;
        internal ulong ullAvailPhys;
        internal ulong ullTotalPageFile;
        internal ulong ullAvailPageFile;
        internal ulong ullTotalVirtual;
        internal ulong ullAvailVirtual;
        internal ulong ullAvailExtendedVirtual;

        public static MEMORYSTATUSEX New()
        {
            return new MEMORYSTATUSEX
            {
                dwLength = checked((uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX)))
            };
        }
    }
    #endregion

    public override string GetProcessorName()
    {
        return ProcessUtil.WindowsProcessor();
    }

    public override double GetCpuUsage()
    {
        return CpuUtil.GetWindowsCpuUsage();
    }

    public override List<string> GetDotNetVersions()
    {
        var versions = new List<string>();

        versions.AddRange(NetFrameworkVersionUtil.GetNetFrameworkVersions());
        versions.AddRange(NetVersionUtil.GetNetVersions());

        return versions;
    }
}
