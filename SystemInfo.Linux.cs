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
            Total = double.Parse(lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]),
            Free = double.Parse(lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]),
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
}
