using System.Runtime.InteropServices;

public class WindowsCpuUsageInterop
{
    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetCurrentThread();

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GetThreadTimes(IntPtr hThread, out long lpCreationTime,
        out long lpExitTime, out long lpKernelTime, out long lpUserTime);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetCurrentProcess();


    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GetProcessTimes(IntPtr hThread, out long lpCreationTime,
        out long lpExitTime, out long lpKernelTime, out long lpUserTime);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GetSystemTimes(out long pIdleTime, out long pKernelTime, out long pUserTime);

    public static bool GetThreadTimes(out long kernelMicroseconds, out long userMicroseconds)
    {
        long ignored;
        long kernel;
        long user;
        if (GetThreadTimes(GetCurrentThread(), out ignored, out ignored, out kernel, out user))
        {
            // Console.WriteLine($"kernel: {kernel}, user: {user}");
            kernelMicroseconds = kernel / 10L;
            userMicroseconds = user / 10L;
            return true;
        }
        else
        {
            kernelMicroseconds = -1;
            userMicroseconds = -1;
            return false;
        }

    }

    public static bool GetProcessTimes(out long kernelMicroseconds, out long userMicroseconds)
    {
        long ignored;
        long kernel;
        long user;
        if (GetProcessTimes(GetCurrentProcess(), out ignored, out ignored, out kernel, out user))
        {
            // Console.WriteLine($"kernel: {kernel}, user: {user}");
            kernelMicroseconds = kernel / 10L;
            userMicroseconds = user / 10L;
            return true;
        }
        else
        {
            kernelMicroseconds = -1;
            userMicroseconds = -1;
            return false;
        }
    }

    public static bool GetSystemTimes(out long kernelMicroseconds, out long userMicroseconds)
    {
        long ignored;
        long kernel;
        long user;
        if (GetSystemTimes(out ignored, out kernel, out user))
        {
            // Console.WriteLine($"kernel: {kernel}, user: {user}");
            kernelMicroseconds = kernel / 10L;
            userMicroseconds = user / 10L;
            return true;
        }
        else
        {
            kernelMicroseconds = -1;
            userMicroseconds = -1;
            return false;
        }
    }

}