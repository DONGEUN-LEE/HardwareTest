using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

[SupportedOSPlatform("MacOS")]
class SystemInfoMacOsx : SystemInfoBase
{
    public override MemoryInfo GetMemoryInfo()
    {
        var total = GetSysCtlIntegerByName("hw.memsize");
        var free = GetSysCtlIntegerByName("vm.page_free_count") * GetSysCtlIntegerByName("vm.pagesize");


        var metrics = new MemoryInfo
        {
            Total = total,
            Free = free,
            Used = total - free,
        };


        return metrics;
    }

    #region extern
    private static IntPtr SizeOfLineSize = (IntPtr)IntPtr.Size;
    [SecurityCritical]
    public static ulong GetSysCtlIntegerByName(string name)
    {
        sysctlbyname(name, out var lineSize, ref SizeOfLineSize, IntPtr.Zero, IntPtr.Zero);
        return (ulong)lineSize.ToInt64();
    }

    [SecurityCritical]
    [DllImport("libc")]
    private static extern int sysctlbyname(string name, out IntPtr oldp, ref IntPtr oldlenp, IntPtr newp, IntPtr newlen);
    #endregion

    public override string GetProcessorName()
    {
        return ProcessUtil.MacProcessor();
    }

    public override double GetCpuUsage()
    {
        return CpuUtil.GetMacCpuUsage();
    }

}