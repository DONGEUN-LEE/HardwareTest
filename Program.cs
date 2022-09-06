// using LibreHardwareMonitor.Hardware;

// var computer = new Computer
// {
//     IsCpuEnabled = true,
//     IsGpuEnabled = true,
//     IsMemoryEnabled = false,
//     IsMotherboardEnabled = false,
//     IsControllerEnabled = false,
//     IsNetworkEnabled = false,
//     IsStorageEnabled = false
// };

// var collector = new SensorCollector(computer);
// collector.Start();

// foreach (var report in collector.ReadAllSensors())
// {
//     Console.WriteLine($"{report.Hardware} / {report.Sensor} : {report.Value}");
// }

// collector.Dispose();

var onStart = CpuUsage.GetBySystem();
Thread.Sleep(500);
var onEnd = CpuUsage.GetBySystem();
Console.WriteLine("CPU Usage System: " + (onEnd - onStart));

var onStartP = CpuUsage.GetByProcess();
Thread.Sleep(500);
var onEndP = CpuUsage.GetByProcess();
Console.WriteLine("CPU Usage Process: " + (onEndP - onStartP));
