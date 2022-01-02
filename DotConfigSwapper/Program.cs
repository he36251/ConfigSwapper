using ConfigSwapper;

ConsolePresenter.StartupMessage();

// Get config files from home path
var configsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"/DotConfigSwapper";
var files = Directory.GetFiles(configsFolderPath, "*.json");

ConsolePresenter.FoundConfigsCountMessage(files.Length);

if (files.Length == 0)
{
    ConsolePresenter.ErrorMessage("No config files found in " + configsFolderPath, null, true);
    return;
}

var selectedConfigs = ConsolePresenter.AskConfigsToSwap(files);
var inputConfigs = ConfigsProcessor.GetConfigs(selectedConfigs).ToList();

List<string> invalidConfigs;
try
{
    invalidConfigs = ConfigsProcessor.ValidateTargetConfigPaths(inputConfigs).ToList();
}
catch (Exception e)
{
    ConsolePresenter.ErrorMessage(e.Message, null, true);
    return;
}

if (invalidConfigs.Any())
{
    ConsolePresenter.ErrorMessage("Could not find the following target config files:", invalidConfigs, true);
    return;
}

ConfigsProcessor.ExecuteSelectedConfigs(inputConfigs);