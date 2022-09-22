using System.Diagnostics;
using System.Runtime.Versioning;

[SupportedOSPlatform("Linux")]
class SystemInfoLinux : SystemInfoBase
{
    public override MemoryInfo GetMemoryInfo()
    {
        var lines = File.ReadLines("/proc/meminfo").Take(2).ToArray();
        // MemTotal: 3773288 kB
        // MemFree:  1164244 kB

        var metrics = new MemoryInfo
        {
            Total = double.Parse(lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]) * 1024f,
            Free = double.Parse(lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]) * 1024f,
        };
        metrics.Used = metrics.Total - metrics.Free;

        return metrics;
    }

    public override string GetProcessorName()
    {
        return ProcessUtil.LinuxProcessor();
    }

    public override double GetCpuUsage()
    {
        return CpuUtil.GetLinuxCpuUsage();
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
