using ConfigSwapper.Extensions;
using Spectre.Console;

namespace ConfigSwapper;

public static class ConsolePresenter
{
    public static void StartupMessage()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[blue]Running DotConfigSwapper...[/]");
    }

    public static void WriteLine(string message)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine(message);
    }

    public static void FoundConfigsCountMessage(int count)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule($"[yellow]Found [bold]{count}[/] config files to swap[/]").RuleStyle("grey").LeftAligned());
    }

    public static void ErrorMessage(string errorMessage, IEnumerable<string>? errorDetails = null, bool isExit = false)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(errorMessage.MarkupError(true));

        if (errorDetails != null)
        {
            var table = new Table();
            table.AddColumn("");
            table.HideHeaders();
            
            foreach (var errorDetail in errorDetails)
            {
                table.AddRow("- " + errorDetail.MarkupError());
            }
            
            AnsiConsole.Write(table);
        }

        if (isExit)
        {
            AnsiConsole.WriteLine("Exiting DotConfigSwapper...");
        }
    }

    public static IEnumerable<string> AskConfigsToSwap(IEnumerable<string> files)
    {
        AnsiConsole.WriteLine();

        IEnumerable<string> selectedConfigs = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                {
                    // Converter to make config file paths only show the file name
                    Converter = filePath => Path.GetFileName(filePath.ToString())
                }
                .Title("Which configs would you like to use?")
                .Required()
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle a config, [green]<enter>[/] to execute)[/]")!
                .AddChoices(files)
        );

        return selectedConfigs;
    }
}