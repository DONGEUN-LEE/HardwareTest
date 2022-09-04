using System.Runtime.InteropServices;

public class MacOsThreadInfo
{
    public static bool IsSupported => _IsSupported.Value;

    public static CpuUsage? GetByThread()
    {
        return Get();
    }

    static CpuUsage? Get()
    {
        int threadId = MacOsThreadInfoInterop.mach_thread_self();
        try
        {
            if (threadId == 0) return null;

            var ret = MacOsThreadInfoInterop.GetThreadCpuUsageInfo(threadId);
            return ret;
        }
        finally
        {
            int? self = null;
            int kResult2 = -424242;
            int kResult1 = MacOsThreadInfoInterop.mach_port_deallocate(threadId, threadId);

            // https://opensource.apple.com/source/xnu/xnu-792/osfmk/mach/kern_return.h
            // KERN_INVALID_TASK 16: target task isn't an active task.
            if (kResult1 != 0)
            {
                self = MacOsThreadInfoInterop.mach_thread_self();
                kResult2 = MacOsThreadInfoInterop.mach_port_deallocate(self.Value, threadId);
            }
#if DEBUG
            if (kResult1 != 0 && kResult2 != 0)
            {
                Console.WriteLine($@"{(kResult1 == 0 || kResult2 == 0 ? "Info" : "Warning!!!!!")} 
    mach_port_deallocate({threadId}, {threadId}) returned {kResult1}
    mach_port_deallocate(mach_thread_self() == {self}, {threadId}) returned {kResult2}");
            }
#endif
        }

    }

    private static Lazy<bool> _IsSupported = new Lazy<bool>(() =>
    {
        try
        {
            GetByThread();
            return true;
        }
        catch
        {
            return false;
        }
    });

}