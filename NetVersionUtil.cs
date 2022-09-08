using System.Diagnostics;

public static class NetVersionUtil
{
    public static List<string> GetNetVersions()
    {
        var versions = new List<string>();
        try
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "--list-runtimes",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                var items = line.Split(' ');
                if (items.Length > 2)
                {
                    var version = $"v{items[1]}";
                    if (!versions.Contains(version))
                    {
                        versions.Add(version);
                    }
                }
            }

        }
        catch { }

        return versions;
    }
}