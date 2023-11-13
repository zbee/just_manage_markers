using System;
using System.Collections.Generic;

namespace JustManageMarkers.Commands;

public class Commands
{
    #region Boilerplate

    private readonly List<Command> _commands = new();

    private static void UNIMPLEMENTED_COMMAND(
        JustManageMarkers plugin,
        ArgumentStruct args = default
    )
    {
        throw new NotImplementedException();
    }

    private static ArgumentStruct UNIMPLEMENTED_COMMAND_PARSER(
        JustManageMarkers plugin,
        List<List<string>> acceptedArguments,
        string args
    )
    {
        throw new NotImplementedException();
    }

    public List<Command> getCommands()

        #endregion

    {
        this._addBaseCommands();
        this._addSwapCommand();

        #region Boilerplate

        return this._commands;

        #endregion
    }

    private void _addBaseCommands()
    {
        this._commands.Add(
            new Command(
                "/justmarkers",
                CommandHandlers.justManageMarkers,
                "Open the main window"
            )
        );

        this._commands.Add(
            new Command(
                "/justmarkers config",
                CommandHandlers.justManageMarkersConfig,
                "Open the preferences window"
            )
        );

        this._commands.Add(
            new Command(
                "/justmarkers help",
                UNIMPLEMENTED_COMMAND,
                "Open the help window",
                true,
                true
            )
        );

        this._commands.Add(
            new Command(
                "/justmarkers advanced help",
                UNIMPLEMENTED_COMMAND,
                "Open the advanced help window"
            )
        );
    }

    private void _addSwapCommand()
    {
        this._commands.Add(
            new Command(
                "/justmarkers swap",
                UNIMPLEMENTED_COMMAND,
                "Swap the positions of two markers, or marker types",
                new List<List<string>>()
                {
                    new List<string>()
                    {
                        "letters",
                        "numbers",
                    },
                    new List<string>()
                    {
                        "numbers",
                        "letters",
                    },
                    new List<string>()
                    {
                        "<marker>",
                        "<marker>",
                    },
                },
                UNIMPLEMENTED_COMMAND_PARSER,
                false
            )
        );

        this._commands.Add(
            new Command(
                "/justmarkers swap help",
                UNIMPLEMENTED_COMMAND,
                "Learn how to us the swap command",
                false
            )
        );
    }
}
