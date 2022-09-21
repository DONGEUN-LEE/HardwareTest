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

    public static double GetUnixProcessUsedMemory(int processId)
    {
        try
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"/bin/bash",
                    Arguments = $"-c \"ps ax -o pid -o rss | grep {processId}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                var str = proc.StandardOutput.ReadLine();
                if (string.IsNullOrWhiteSpace(str)) continue;
                str = str.Trim();
                var pid = str.Substring(0, str.IndexOf(' '));
                var usage = str.Substring(str.IndexOf(' ')).Trim();
                if (pid == processId.ToString())
                {
                    return Convert.ToDouble(usage);
                }
            }
        }
        catch { }

        return 0;
    }
}