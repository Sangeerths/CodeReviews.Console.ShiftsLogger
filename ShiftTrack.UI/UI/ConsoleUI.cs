using Spectre.Console;

namespace ShiftTrack.UI.UI;

public class ConsoleUI
{
    public void ShowHeader()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("ShiftTrack")
                .Centered()
                .Color(Color.Cyan1));

        AnsiConsole.MarkupLine("[grey]Manage employees, track shifts, and stay on schedule.[/]");
        AnsiConsole.WriteLine();

        AnsiConsole.Write(new Rule("[grey]Main Menu[/]").RuleStyle("grey"));
    }

    public void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    public void ShowError(string message) =>
    AnsiConsole.MarkupLine($"[red][[ERROR]][/]: {Markup.Escape(message)}");

    public void ShowSuccess(string message) =>
        AnsiConsole.MarkupLine($"[green][[SUCCESS]][/]: {Markup.Escape(message)}");

    public void ShowInfo(string message) =>
        AnsiConsole.MarkupLine($"[blue][[INFO]][/]: {Markup.Escape(message)}");

    public void ShowWarning(string message) =>
        AnsiConsole.MarkupLine($"[yellow][[WARNING]][/]: {Markup.Escape(message)}");

    private static void ShowMessage(string message, string color, string icon)
    {
        AnsiConsole.MarkupLine($"[{color}]{icon} {Markup.Escape(message)}[/]");
    }

    public bool Confirm(string message)
    {
        return AnsiConsole.Confirm(message);
    }

    public void ShowGoodbye()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("Goodbye!")
                .Centered()
                .Color(Color.Cyan1));

        AnsiConsole.MarkupLine("[green]Thanks for using ShiftTrack![/]");
    }
}