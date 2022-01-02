using System.Text.Json;
using System.Xml;
using ConfigSwapper.Models;

namespace ConfigSwapper;

public static class ConfigsProcessor
{
    public static IEnumerable<ConfigFile> GetConfigs(IEnumerable<string> paths)
    {
        var configs = new List<ConfigFile>();

        foreach (var path in paths)
        {
            var rawJsonValues = File.ReadAllText(path);

            var parsedConfig = JsonSerializer.Deserialize<IEnumerable<ConfigFile>>(
                rawJsonValues,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            configs.AddRange(parsedConfig!);
        }

        return configs;
    }

    /// <summary>
    /// Check if all target config files exist.
    /// </summary>
    /// <remarks>
    /// This assumes a valid valid/existing file path is given,
    /// otherwise it will throw an exception for an invalid path.
    /// </remarks>
    /// <param name="selectedConfigs"></param>
    /// <returns>Invalid target config paths</returns>
    public static IEnumerable<string> ValidateTargetConfigPaths(IEnumerable<ConfigFile> selectedConfigs)
    {
        var targetConfigPaths = selectedConfigs.Select(x => x.FileName);

        var invalidConfigs = targetConfigPaths
            .Where(targetConfigPath => !File.Exists(targetConfigPath))
            .ToList();

        return invalidConfigs;
    }

    /// <summary>
    /// Execute user selected DotConfigSwapper config files
    /// </summary>
    /// <param name="configs"></param>
    public static void ExecuteSelectedConfigs(List<ConfigFile> configs)
    {
        foreach (var config in configs)
        {
            switch (config.Type)
            {
                case ConfigType.Config:
                    ProcessXmlConfigs(config);
                    break;
            }
        }
    }

    private static void ProcessXmlConfigs(ConfigFile inputConfigFile)
    {
        var xDoc = new XmlDocument();
        xDoc.Load(inputConfigFile.FileName);

        // <connectionStrings/>
        var connectionStrings = xDoc.GetElementsByTagName("connectionStrings");
        foreach (XmlNode connectionString in connectionStrings)
        {
            // List of <add/> nodes 
            var addNodes = connectionString.ChildNodes;
            foreach (XmlElement addNode in addNodes)
            {
                if (addNode.Name == "add")
                {
                    addNode.SetAttribute("connectionString", "Data Source=localhost;Initial Catalog=testdb;Integrated Security=True");
                }
            }
        }

        var appsettings = xDoc.GetElementsByTagName("appSettings");
        foreach (XmlNode appsetting in appsettings)
        {
            // List of <add/> nodes 
            var addNodes = appsetting.ChildNodes;
            foreach (XmlElement addNode in addNodes)
            {
                if (addNode.Name == "add" && addNode.GetAttribute("key") == "LoginApi.BaseUrl")
                {
                    addNode.SetAttribute("value", "http://localhost:5000");
                }
            }
        }

        xDoc.Save(inputConfigFile.FileName);
    }
}