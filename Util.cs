using System.Diagnostics;
using System.Text;

public static class Util
{
    public static double GetWindowsCpuUsage()
    {
        var sb = new StringBuilder();
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = "cpu get loadpercentage",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        proc.Start();
        while (!proc.StandardOutput.EndOfStream)
        {
            sb.Append(proc.StandardOutput.ReadLine());
        }
        Console.WriteLine(sb.ToString());

        return 0;
    }

    public static double GetLinuxCpuUsage()
    {
        var sb = new StringBuilder();
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = @"/bin/bash",
                Arguments = "-c \"top -bn1 | head -5 | grep Cpu\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        proc.Start();
        while (!proc.StandardOutput.EndOfStream)
        {
            sb.Append(proc.StandardOutput.ReadLine());
        }
        Console.WriteLine(sb.ToString());

        return 0;
    }

    public static double GetMacCpuUsage()
    {
        try {
            var sb = new StringBuilder();
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"/bin/bash",
                    Arguments = "-c \"top -l 1 | head -5 | grep CPU\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                sb.Append(proc.StandardOutput.ReadLine());
            }
            var line = sb.ToString();
            line = line.Substring(line.IndexOf(':') + 2);
            var userStr = line.Substring(0, line.IndexOf('%'));
            line = line.Substring(line.IndexOf(',') + 2);
            var sysStr = line.Substring(0, line.IndexOf('%'));
            return Convert.ToDouble(userStr) + Convert.ToDouble(sysStr);
        } catch { }

        return 0;
    }
}