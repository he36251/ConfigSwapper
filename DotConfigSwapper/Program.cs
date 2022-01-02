using ConfigSwapper;

ConsolePresenter.StartupMessage();

// Get config files from home path
var configsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"/DotConfigSwapper";
var files = Directory.GetFiles(configsFolderPath, "*.json");

ConsolePresenter.FoundConfigsCountMessage(files.Length);

if (files.Length == 0)
{
    var errorMessages = new[]
    {
        "No config files found in " + configsFolderPath,
        "Exiting DotConfigSwapper..."
    };

    ConsolePresenter.ErrorMessage(errorMessages);
    return;
}

var selectedConfigs = ConsolePresenter.AskConfigsToSwap(files);
var inputConfigs = ConfigsProcessor.GetConfigs(selectedConfigs).ToList();

var invalidConfigs = new List<string>();
try
{
    invalidConfigs = ConfigsProcessor.ValidateTargetConfigPaths(inputConfigs).ToList();
}
catch (Exception e)
{
    ConsolePresenter.ErrorMessage(new[]
    {
        e.Message,
        "Exiting DotConfigSwapper..."
    });
    Environment.Exit(0);
}

if (invalidConfigs.Any())
{
    var errorMessages = invalidConfigs.Prepend("Could not find the following target config files:");
    ConsolePresenter.ErrorMessage(errorMessages);
    return;
}

ConfigsProcessor.ExecuteSelectedConfigs(inputConfigs);