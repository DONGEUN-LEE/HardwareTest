using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

[SupportedOSPlatform("MacOS")]
class SystemInfoMacOsx : SystemInfoBase
{
    public override MemoryInfo GetMemoryInfo()
    {
        var unit = GetSysCtlIntegerByName("vm.pagesize");
        var total = GetSysCtlIntegerByName("vm.pages") * unit;
        var used = MemoryUtil.GetMacUseMemory() * unit;

        var metrics = new MemoryInfo
        {
            Total = total,
            Free = total - used,
            Used = used,
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

    public override double GetProcessCpuUsage(int processId = -1)
    {
        if (processId == -1)
        {
            processId = Process.GetCurrentProcess().Id;
        }
        return CpuUtil.GetUnixProcessCpuUsage(processId);
    }

    public override double GetProcessMemorySize(int processId = -1)
    {
        if (processId == -1)
        {
            processId = Process.GetCurrentProcess().Id;
        }
        return MemoryUtil.GetUnixProcessUsedMemory(processId);
    }
}