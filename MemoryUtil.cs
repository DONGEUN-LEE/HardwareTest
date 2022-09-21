using System.Diagnostics;

public static class MemoryUtil
{
    public static ulong GetMacUseMemory()
    {
        try
        {
            ulong sum = 0;
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "vm_stat",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                if (line.StartsWith("Pages active") ||
                    line.StartsWith("Pages inactive") ||
                    line.StartsWith("Pages wired")
                )
                {
                    sum += Convert.ToUInt64(line.Split(':')[1].Trim().Replace(".", ""));
                }
            }
            return sum;
        }
        catch { }

        return 0;
    }

    // windows memory usage => tasklist | findstr "9924"
}