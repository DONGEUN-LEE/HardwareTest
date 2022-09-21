using System.Diagnostics;
using System.Text;

public static class CpuUtil
{
    public static double GetWindowsCpuUsage()
    {
        try
        {
            var sb = new StringBuilder();
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c wmic cpu get loadpercentage",
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
            line = line.Substring(line.IndexOf(' ') + 2);
            return Convert.ToDouble(line);
        }
        catch { }

        return 0;
    }

    public static double GetWindowsProcessCpuUsage(int processId)
    {
        try
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c wmic path Win32_PerfFormattedData_PerfProc_Process get IDProcess,PercentProcessorTime | findstr \"{processId}\"",
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
                var pid = str.Substring(0, str.IndexOf(' '));
                var usage = str.Substring(str.IndexOf(' ')).Trim();
                if (Convert.ToInt32(pid) == processId)
                {
                    return Convert.ToDouble(usage);
                }
            }
        }
        catch { }

        return 0;
    }

    public static double GetLinuxCpuUsage()
    {
        try
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
            var line = sb.ToString();
            line = line.Substring(line.IndexOf(':') + 2);
            var userStr = line.Substring(0, line.IndexOf('u')).Trim();
            line = line.Substring(line.IndexOf(',') + 2);
            var sysStr = line.Substring(0, line.IndexOf('s')).Trim();
            return Convert.ToDouble(userStr) + Convert.ToDouble(sysStr);
        }
        catch { }

        return 0;
    }

    public static double GetMacCpuUsage()
    {
        try
        {
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
        }
        catch { }

        return 0;
    }

    public static double GetUnixProcessCpuUsage(int processId)
    {
        try
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"/bin/bash",
                    Arguments = $"-c \"ps ax -o pid -o %cpu | grep {processId}\"",
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