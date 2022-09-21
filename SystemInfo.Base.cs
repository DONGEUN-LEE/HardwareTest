abstract class SystemInfoBase : ISystemInfo
{
    private MemoryInfo? _memoryInfo;
    public MemoryInfo MemoryInfo
    {
        get
        {
            if (_memoryInfo == null)
            {
                _memoryInfo = GetMemoryInfo();
            }
            return _memoryInfo;
        }
    }

    public virtual string GetDotNetVersion()
    {
        return System.Environment.Version.ToString();
    }

    public virtual List<string> GetDotNetVersions()
    {
        return NetVersionUtil.GetNetVersions();
    }

    public virtual string GetMemorySize()
    {
        return GetBytesReadable((long)this.MemoryInfo.Total);
    }

    public virtual string GetDiskSize(string path)
    {
        var diskInfo = this.GetDiskInfo(path);
        return GetBytesReadable((long)diskInfo.Total);
    }

    public abstract double GetCpuUsage();

    public virtual double GetMemoryUsage()
    {
        return Math.Round(this.MemoryInfo.Used / (double)this.MemoryInfo.Total * 100f, 2, MidpointRounding.AwayFromZero);
    }

    public abstract double GetProcessCpuUsage(int processId = -1);

    public abstract double GetProcessMemorySize(int processId = -1);

    public virtual double GetDiskUsage(string path)
    {
        var diskInfo = this.GetDiskInfo(path);
        if (diskInfo == null) return 0;
        return Math.Round(diskInfo.Used / (double)diskInfo.Total * 100f, 2, MidpointRounding.AwayFromZero);
    }

    public abstract MemoryInfo GetMemoryInfo();

    public virtual DiskInfo? GetDiskInfo(string path)
    {
        if (!Directory.Exists(path)) return null;
        var info = DriveInfo.GetDrives().Where(d => d.IsReady && d.Name == Path.GetPathRoot(path)).FirstOrDefault();
        if (info == null) return null;

        return new DiskInfo
        {
            Total = info.TotalSize,
            Free = info.AvailableFreeSpace,
            Used = info.TotalSize - info.AvailableFreeSpace,
        };
    }

    public virtual string GetOperatingSystemName()
    {
        return System.Environment.OSVersion.ToString();
    }

    public virtual string GetProcessorName()
    {
        return string.Empty;
    }

    public virtual bool Is64BitOperatingSystem()
    {
        return System.Environment.Is64BitOperatingSystem;
    }

    // Returns the human-readable file size for an arbitrary, 64-bit file size
    // The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
    private string GetBytesReadable(long i)
    {
        // Get absolute value
        long absolute_i = (i < 0 ? -i : i);
        // Determine the suffix and readable value
        string suffix;
        double readable;
        if (absolute_i >= 0x1000000000000000) // Exabyte
        {
            suffix = "EB";
            readable = (i >> 50);
        }
        else if (absolute_i >= 0x4000000000000) // Petabyte
        {
            suffix = "PB";
            readable = (i >> 40);
        }
        else if (absolute_i >= 0x10000000000) // Terabyte
        {
            suffix = "TB";
            readable = (i >> 30);
        }
        else if (absolute_i >= 0x40000000) // Gigabyte
        {
            suffix = "GB";
            readable = (i >> 20);
        }
        else if (absolute_i >= 0x100000) // Megabyte
        {
            suffix = "MB";
            readable = (i >> 10);
        }
        else if (absolute_i >= 0x400) // Kilobyte
        {
            suffix = "KB";
            readable = i;
        }
        else
        {
            return i.ToString("0 B"); // Byte
        }
        // Divide by 1024 to get fractional value
        readable = (readable / 1024);
        // Return formatted number with suffix
        return readable.ToString("0.### ") + suffix;
    }
}