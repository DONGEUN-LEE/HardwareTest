public class SystemInfoFactory
{
    public static ISystemInfo Create()
    {
        if (OperatingSystem.IsWindows())
            return new SystemInfoWindows();
        if (OperatingSystem.IsMacOS())
            return new SystemInfoMacOsx();
        if (OperatingSystem.IsLinux())
            return new SystemInfoLinux();

        throw new NotSupportedException(System.Environment.OSVersion.ToString());
    }
}
