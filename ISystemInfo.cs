public interface ISystemInfo
{
    MemoryInfo GetMemoryInfo();
    string GetMemorySize();
    string GetDiskSize(string path);
    double GetCpuUsage();
    double GetMemoryUsage();
    double GetDiskUsage(string path);
    string GetProcessorName();
    string GetOperatingSystemName();
    bool Is64BitOperatingSystem();
    string GetDotNetVersion();
    List<string> GetDotNetVersions();
}
