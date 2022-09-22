using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{Environment.OSVersion.Platform}.json", optional: true, reloadOnChange: true)
    .Build();

var setupInfo = new SetupInformation(config);
var workingDir = setupInfo.GetWorkingDirectory();

var systemInfo = SystemInfoFactory.Create();

Console.WriteLine($"Processor : {systemInfo.GetProcessorName()}");
Console.WriteLine($"OS : {systemInfo.GetOperatingSystemName()}");
Console.WriteLine($"Total Memory : {systemInfo.GetMemorySize()}");
Console.WriteLine($"Process Memory : {systemInfo.GetProcessMemorySize()}");
Console.WriteLine($".NET Versions : {string.Join(", ", systemInfo.GetDotNetVersions())}");
Console.WriteLine();
Console.WriteLine($"Cpu Usage : {systemInfo.GetCpuUsage()}%");
Console.WriteLine($"Process Cpu Usage : {systemInfo.GetProcessCpuUsage()}%");
Console.WriteLine($"Memory Usage : {systemInfo.GetMemoryUsage()}%");
Console.WriteLine($"Process Memory Usage : {systemInfo.GetProcessMemoryUsage()}%");
Console.WriteLine($"Disk Usage : {systemInfo.GetDiskUsage(workingDir)}%");
