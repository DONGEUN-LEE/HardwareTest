// using LibreHardwareMonitor.Hardware;

// var computer = new Computer
// {
//     IsCpuEnabled = true,
//     IsGpuEnabled = true,
//     IsMemoryEnabled = true,
//     IsMotherboardEnabled = false,
//     IsControllerEnabled = true,
//     IsNetworkEnabled = true,
//     IsStorageEnabled = true
// };

// var collector = new SensorCollector(computer);
// collector.Start();

// foreach (var report in collector.ReadAllSensors())
// {
//     Console.WriteLine($"{report.Hardware} / {report.Sensor} : {report.Value}");
// }

// collector.Dispose();

// var onStart = 
// Thread.Sleep(500);
// var onEnd = CpuUsage.GetByProcess();
// Console.WriteLine("CPU Usage: " + (onEnd - onStart));

Console.WriteLine(CpuUsage.GetByProcess().ToString());
