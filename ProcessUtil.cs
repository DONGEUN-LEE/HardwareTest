using System.Diagnostics;
using System.Text;

public static class ProcessUtil
{
    public static string WindowsProcessor()
    {
        var sb = new StringBuilder();
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "wmic.exe",
                Arguments = "cpu list /format:list",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        proc.Start();
        while (!proc.StandardOutput.EndOfStream)
        {
            string line = proc.StandardOutput.ReadLine();
            if (line.StartsWith("Name="))
            {
                sb.Append(line.Replace("Name=", ""));
                break;
            }
        }
        return sb.ToString();
    }

    public static string LinuxProcessor()
    {
        var sb = new StringBuilder();
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cat",
                Arguments = "/proc/cpuinfo",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        proc.Start();
        while (!proc.StandardOutput.EndOfStream)
        {
            string line = proc.StandardOutput.ReadLine();
            if (line.StartsWith("model name"))
            {
                sb.Append(line.Replace("model name", "").Trim().Replace(":", "").Trim());
                break;
            }
        }
        return sb.ToString();
    }

    public static string MacProcessor()
    {
        var sb = new StringBuilder();
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "sysctl",
                Arguments = "-a machdep.cpu",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        proc.Start();
        while (!proc.StandardOutput.EndOfStream)
        {
            string line = proc.StandardOutput.ReadLine();
            if (line.StartsWith("machdep.cpu.brand_string"))
            {
                sb.Append(line.Replace("machdep.cpu.brand_string:", "").Trim());
                break;
            }
        }
        return sb.ToString();
    }
}