// using Microsoft.Extensions.Configuration;

// var config = new ConfigurationBuilder()
//     .SetBasePath(Directory.GetCurrentDirectory())
//     .AddJsonFile($"appsettings.{Environment.OSVersion.Platform}.json", optional: true, reloadOnChange: true)
//     .Build();

// var setupInfo = new SetupInformation(config);
// var workingDir = setupInfo.GetWorkingDirectory();

// var systemInfo = SystemInfoFactory.Create();

// Console.WriteLine($"Processor : {systemInfo.GetProcessorName()}");
// Console.WriteLine($"OS : {systemInfo.GetOperatingSystemName()}");
// Console.WriteLine($"Total Memory : {systemInfo.GetMemorySize()}");
// Console.WriteLine($".NET Versions : {string.Join(", ", systemInfo.GetDotNetVersions())}");
// Console.WriteLine();
// Console.WriteLine($"Cpu Usage : {systemInfo.GetCpuUsage()}%");
// Console.WriteLine($"Memory Usage : {systemInfo.GetMemoryUsage()}%");
// Console.WriteLine($"Disk Usage : {systemInfo.GetDiskUsage(workingDir)}%");
Console.WriteLine(CpuUtil.GetMacCpuUsage());
Console.WriteLine(CpuUtil.GetUnixProcessCpuUsage(1481));
Console.WriteLine(MemoryUtil.GetUnixProcessUsedMemory(1481));
