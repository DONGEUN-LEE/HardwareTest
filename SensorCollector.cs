using System.Collections.Concurrent;
using LibreHardwareMonitor.Hardware;

public class SensorCollector
{
    private readonly Computer _computer;
    private readonly IVisitor _updateVisitor = new UpdateVisitor();

    private readonly ConcurrentDictionary<Identifier, object> _ids =
        new ConcurrentDictionary<Identifier, object>();

    // private readonly ConcurrentDictionary<Identifier, OhmNvme> _nvmes =
    //     new ConcurrentDictionary<Identifier, OhmNvme>();

    public SensorCollector(Computer computer)
    {
        _computer = computer;
    }

    public void Open()
    {
        foreach (var hardware in _computer.Hardware)
        {
            HardwareAdded(hardware);
        }

        _computer.HardwareAdded += HardwareAdded;
        _computer.HardwareRemoved += HardwareRemoved;
        _computer.Open();
    }

    public void Close()
    {
        _computer.HardwareRemoved -= HardwareRemoved;
        _computer.HardwareAdded -= HardwareAdded;
        foreach (var hardware in _computer.Hardware)
        {
            HardwareRemoved(hardware);
        }
        _computer.Close();
    }

    public void Start() => Open();
    public void Dispose() => Close();

    private void HardwareAdded(IHardware hardware)
    {
        if (!_ids.TryAdd(hardware.Identifier, hardware))
        {
            Console.WriteLine("Hardware previously added: {0}", hardware.Identifier);
            return;
        }

        Console.WriteLine("Hardware added: {0}", hardware.Identifier);
        hardware.SensorAdded += SensorAdded;
        hardware.SensorRemoved += SensorRemoved;
        foreach (var sensor in hardware.Sensors)
        {
            SensorAdded(sensor);
        }

        // if (hardware is NVMeGeneric nvme)
        // {
        //     var ohmNvme = new OhmNvme(nvme);
        //     _nvmes.TryAdd(hardware.Identifier, ohmNvme);
        //     SensorAdded(ohmNvme.MediaErrors);
        //     SensorAdded(ohmNvme.PowerCycles);
        //     SensorAdded(ohmNvme.ErrorInfoLogEntryCount);
        //     SensorAdded(ohmNvme.UnsafeShutdowns);
        // }

        foreach (var sub in hardware.SubHardware)
        {
            HardwareAdded(sub);
        }
    }

    private void HardwareRemoved(IHardware hardware)
    {
        _ids.TryRemove(hardware.Identifier, out _);
        Console.WriteLine("Hardware removed: {0}", hardware.Identifier);
        hardware.SensorAdded -= SensorAdded;
        hardware.SensorRemoved -= SensorRemoved;
        foreach (var sensor in hardware.Sensors)
        {
            SensorRemoved(sensor);
        }

        // if (_nvmes.TryRemove(hardware.Identifier, out OhmNvme ohmNvme))
        // {
        //     SensorRemoved(ohmNvme.MediaErrors);
        //     SensorRemoved(ohmNvme.PowerCycles);
        //     SensorRemoved(ohmNvme.ErrorInfoLogEntryCount);
        //     SensorRemoved(ohmNvme.UnsafeShutdowns);
        // }

        foreach (var sub in hardware.SubHardware)
        {
            HardwareRemoved(sub);
        }
    }

    private void SensorAdded(ISensor sensor)
    {
        var added = _ids.TryAdd(sensor.Identifier, sensor);
        var msg = added ? "Sensor added: {0} \"{1}\"" : "Sensor previously added: {0} \"{1}\"";
        Console.WriteLine(msg, sensor.Identifier, sensor.Name);
    }

    private void SensorRemoved(ISensor sensor)
    {
        Console.WriteLine("Sensor removed: {0}", sensor.Identifier);
        _ids.TryRemove(sensor.Identifier, out _);
    }

    public IEnumerable<ReportedValue> ReadAllSensors()
    {
        _computer.Accept(_updateVisitor);
        // foreach (var nvme in _nvmes.Values)
        // {
        //     nvme.Update();
        // }

        return _ids.Values.OfType<ISensor>().SelectMany(ReportedValues);
    }

    private IEnumerable<ReportedValue> ReportedValues(ISensor sensor)
    {
        string id = sensor.Identifier.ToString();

        // Only report a value if the sensor was able to get a value
        // as 0 is different than "didn't read". For example, are the
        // fans really spinning at 0 RPM or was the value not read.
        if (!sensor.Value.HasValue)
        {
            Console.WriteLine($"{id} did not have a value");
        }
        else if (float.IsNaN(sensor.Value.Value))
        {
            Console.WriteLine($"{id} had a NaN value");
        }
        else if (float.IsInfinity(sensor.Value.Value))
        {
            Console.WriteLine($"{id} had an infinite value");
        }
        else
        {
            var hwInstance = sensor.Hardware.Identifier.ToString();
            var ind = hwInstance.LastIndexOf('/');
            hwInstance = hwInstance.Substring(ind + 1);

            var name = sensor.Name;

            yield return new ReportedValue(id,
                name,
                sensor.Value.Value,
                sensor.SensorType,
                sensor.Hardware.Name,
                sensor.Hardware.HardwareType,
                hwInstance,
                sensor.Index);
        }
    }
}