using System.Text.Json;
using System.Xml;
using ConfigSwapper;

var exampleConfigPath = @"./config-example.json";

var rawInputConfigJson = File.ReadAllText(exampleConfigPath);
if (string.IsNullOrEmpty(rawInputConfigJson))
{
    throw new FileNotFoundException(exampleConfigPath);
}

var inputConfig = JsonSerializer.Deserialize<InputConfig>(rawInputConfigJson);
if (inputConfig == null)
{
    throw new Exception("Failed to deserialize input config - Value is null");
}

foreach (var inputConfigFile in inputConfig.ConfigFiles)
{
    switch (inputConfigFile.Type)
    {
        case ConfigType.Config:
            ProcessXmlConfigs(inputConfigFile);
        break;
    }
}

// TODO: Put this in another class
void ProcessXmlConfigs(ConfigFile inputConfigFile)
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
            if (addNode.Name == "add")
            {
                if (addNode.GetAttribute("key") == "LoginApi.BaseUrl")
                {
                    addNode.SetAttribute("value", "http://localhost:5000");
                }
            }
        }
    }

    xDoc.Save(inputConfigFile.FileName);
}