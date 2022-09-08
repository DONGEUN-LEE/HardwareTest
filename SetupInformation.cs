using Microsoft.Extensions.Configuration;

public class SetupInformation
{

    IConfiguration _configuration;
    public SetupInformation(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetWorkingDirectory()
    {
        if (_configuration == null)
            return string.Empty;

        const string key = "workingDirectory";

        var path = _configuration[key];
        if (string.IsNullOrEmpty(path))
            return string.Empty;

        path = Environment.ExpandEnvironmentVariables(path);
        var workingDirectory = System.IO.Path.GetFullPath(path);

        if (!Directory.Exists(workingDirectory))
            Directory.CreateDirectory(workingDirectory);

        return workingDirectory;
    }
}
