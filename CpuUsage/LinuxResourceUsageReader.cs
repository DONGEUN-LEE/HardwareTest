public class LinuxResourceUsageReader
{

    public const int RESOURCE_USAGE_FIELDS_COUNT = 18;

    public static bool IsSupported => _IsSupported.Value;

    public static CpuUsage? GetByScope(CpuUsageScope scope)
    {
        var s = scope == CpuUsageScope.Process ? LinuxResourceUsageInterop.RUSAGE_SELF : LinuxResourceUsageInterop.RUSAGE_THREAD;
        return GetLinuxCpuUsageByScope(s);
    }

    public static CpuUsage? GetByProcess()
    {
        return GetLinuxCpuUsageByScope(LinuxResourceUsageInterop.RUSAGE_SELF);
    }

    // returns null on mac os x
    public static CpuUsage? GetByThread()
    {
        return GetLinuxCpuUsageByScope(LinuxResourceUsageInterop.RUSAGE_THREAD);
    }

    static Lazy<bool> _IsSupported = new Lazy<bool>(() =>
    {
        try
        {
            GetByScope(CpuUsageScope.Process);
            GetByScope(CpuUsageScope.Thread);
            return true;
        }
        catch
        {
            return false;
        }
    });

    internal static unsafe PosixResourceUsage? GetResourceUsageByScope(int scope)
    {
        if (IntPtr.Size == 4)
        {
            int* rawResourceUsage = stackalloc int[RESOURCE_USAGE_FIELDS_COUNT];
            int result = LinuxResourceUsageInterop.getrusage_heapless(scope, new IntPtr(rawResourceUsage));
            if (result != 0) return null;
            return new PosixResourceUsage()
            {
                UserUsage = new TimeValue() { Seconds = *rawResourceUsage, MicroSeconds = rawResourceUsage[1] },
                KernelUsage = new TimeValue() { Seconds = rawResourceUsage[2], MicroSeconds = rawResourceUsage[3] },
                MaxRss = rawResourceUsage[4],
                SoftPageFaults = rawResourceUsage[8],
                HardPageFaults = rawResourceUsage[9],
                Swaps = rawResourceUsage[10],
                ReadOps = rawResourceUsage[11],
                WriteOps = rawResourceUsage[12],
                SentIpcMessages = rawResourceUsage[13],
                ReceivedIpcMessages = rawResourceUsage[14],
                ReceivedSignals = rawResourceUsage[15],
                VoluntaryContextSwitches = rawResourceUsage[16],
                InvoluntaryContextSwitches = rawResourceUsage[17],
            };
        }
        else
        {
            long* rawResourceUsage = stackalloc long[RESOURCE_USAGE_FIELDS_COUNT];
            int result = LinuxResourceUsageInterop.getrusage_heapless(scope, new IntPtr(rawResourceUsage));
            if (result != 0) return null;
            // microseconds are 4 bytes length on mac os and 8 bytes on linux
            return new PosixResourceUsage()
            {
                UserUsage = new TimeValue() { Seconds = *rawResourceUsage, MicroSeconds = rawResourceUsage[1] & 0xFFFFFFFF },
                KernelUsage = new TimeValue() { Seconds = rawResourceUsage[2], MicroSeconds = rawResourceUsage[3] & 0xFFFFFFFF },
                MaxRss = rawResourceUsage[4],
                SoftPageFaults = rawResourceUsage[8],
                HardPageFaults = rawResourceUsage[9],
                Swaps = rawResourceUsage[10],
                ReadOps = rawResourceUsage[11],
                WriteOps = rawResourceUsage[12],
                SentIpcMessages = rawResourceUsage[13],
                ReceivedIpcMessages = rawResourceUsage[14],
                ReceivedSignals = rawResourceUsage[15],
                VoluntaryContextSwitches = rawResourceUsage[16],
                InvoluntaryContextSwitches = rawResourceUsage[17],
            };
        }
    }

    private static unsafe CpuUsage? GetLinuxCpuUsageByScope(int scope)
    {
        if (IntPtr.Size == 4)
        {
            int* rawResourceUsage = stackalloc int[RESOURCE_USAGE_FIELDS_COUNT];
            int result = LinuxResourceUsageInterop.getrusage_heapless(scope, new IntPtr(rawResourceUsage));
            if (result != 0) return null;
            return new CpuUsage()
            {
                UserUsage = new TimeValue() { Seconds = *rawResourceUsage, MicroSeconds = rawResourceUsage[1] },
                KernelUsage = new TimeValue() { Seconds = rawResourceUsage[2], MicroSeconds = rawResourceUsage[3] },
            };
        }
        else
        {
            long* rawResourceUsage = stackalloc long[RESOURCE_USAGE_FIELDS_COUNT];
            int result = LinuxResourceUsageInterop.getrusage_heapless(scope, new IntPtr(rawResourceUsage));
            if (result != 0) return null;
            // microseconds are 4 bytes length on mac os and 8 bytes on linux
            return new CpuUsage()
            {
                UserUsage = new TimeValue() { Seconds = *rawResourceUsage, MicroSeconds = rawResourceUsage[1] & 0xFFFFFFFF },
                KernelUsage = new TimeValue() { Seconds = rawResourceUsage[2], MicroSeconds = rawResourceUsage[3] & 0xFFFFFFFF },
            };
        }
    }
}
