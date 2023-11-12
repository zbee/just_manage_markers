using System.Collections.Generic;

namespace JustManageMarkers.Commands;

public class Commands
{
    private List<Command> commands = new();

    public List<Command> getCommands()
    {
        this._addBaseCommands();
        this._addSwapCommand();

        return this.commands;
    }

    private void _addBaseCommands()
    {
        this.commands.Add(
            new Command(
                "/justmarkers",
                "Open the main window"
            )
        );

        this.commands.Add(
            new Command(
                "/justmarkers config",
                "Open the preferences window"
            )
        );

        this.commands.Add(
            new Command(
                "/justmarkers help",
                "Open the help window",
                true,
                true
            )
        );

        this.commands.Add(
            new Command(
                "/justmarkers advanced help",
                "Open the advanced help window"
            )
        );
    }

    private void _addSwapCommand()
    {
        this.commands.Add(
            new Command(
                "/justmarkers swap",
                "Swap the positions of two markers, or marker types",
                new List<List<string>>() {
                    new List<string>() {
                        "letters",
                        "numbers",
                    },
                    new List<string>() {
                        "numbers",
                        "letters",
                    },
                    new List<string>() {
                        "<marker>",
                        "<marker>",
                    },
                },
                false
            )
        );

        this.commands.Add(
            new Command(
                "/justmarkers swap help",
                "Learn how to us the swap command",
                false
            )
        );
    }
}
