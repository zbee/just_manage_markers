using System;
using System.Collections.Generic;

namespace JustManageMarkers.Commands;

public class Commands
{
    #region Boilerplate

    private readonly List<Command> _commands = new();

    private static void UNIMPLEMENTED_COMMAND_HANDLER(
        JustManageMarkers plugin,
        ArgumentStruct args,
        int parserResult
    )
    {
        throw new NotImplementedException();
    }

    private static int UNIMPLEMENTED_COMMAND_PARSER(
        JustManageMarkers plugin,
        ArgumentStruct arguments
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
                UNIMPLEMENTED_COMMAND_HANDLER,
                "Open the help window",
                true,
                true
            )
        );

        this._commands.Add(
            new Command(
                "/justmarkers advanced help",
                UNIMPLEMENTED_COMMAND_HANDLER,
                "Open the advanced help window"
            )
        );
    }

    private void _addSwapCommand()
    {
        this._commands.Add(
            new Command(
                "/justmarkers swap",
                CommandHandlers.swap,
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
                CommandHandlers.swapArguments,
                false
            )
        );

        this._commands.Add(
            new Command(
                "/justmarkers swap help",
                UNIMPLEMENTED_COMMAND_HANDLER,
                "Learn how to us the swap command",
                false
            )
        );
    }
}
