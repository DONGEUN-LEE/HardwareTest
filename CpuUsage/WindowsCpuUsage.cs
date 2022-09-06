public static class WindowsCpuUsage
{
    public static bool IsSupported => _IsSupported.Value;


    public static CpuUsage? Get(CpuUsageScope scope)
    {
        long kernelMicroseconds;
        long userMicroseconds;
        bool isOk;
        if (scope == CpuUsageScope.Thread)
            isOk = WindowsCpuUsageInterop.GetThreadTimes(out kernelMicroseconds, out userMicroseconds);
        else if (scope == CpuUsageScope.Process)
            isOk = WindowsCpuUsageInterop.GetProcessTimes(out kernelMicroseconds, out userMicroseconds);
        else
            isOk = WindowsCpuUsageInterop.GetSystemTimes(out kernelMicroseconds, out userMicroseconds);

        if (!isOk)
            return null;

        const long m = 1000000L;
        return new CpuUsage()
        {
            KernelUsage = new TimeValue() { Seconds = kernelMicroseconds / m, MicroSeconds = kernelMicroseconds % m },
            UserUsage = new TimeValue() { Seconds = userMicroseconds / m, MicroSeconds = userMicroseconds % m },
        };

    }

    static Lazy<bool> _IsSupported = new Lazy<bool>(() =>
    {
        try
        {
            Get(CpuUsageScope.Process);
            Get(CpuUsageScope.Thread);
            return true;
        }
        catch
        {
            return false;
        }
    });

}
