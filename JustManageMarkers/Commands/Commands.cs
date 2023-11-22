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
        this._addSquareCommand();

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
                    },
                    new List<string>()
                    {
                        "numbers",
                    },
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

    private void _addSquareCommand()
    {
        this._commands.Add(
            new Command(
                "/justmarkers square",
                CommandHandlers.swap,
                "Square up markers to the center, optionally also to an anchor marker",
                new List<List<string>>()
                {
                    new List<string>()
                    {
                        "", // Line up all markers with center
                    },
                    new List<string>()
                    {
                        "letters", // Line up letter markers with center
                    },
                    new List<string>()
                    {
                        "numbers", // Line up number markers with center
                    },
                    new List<string>()
                    {
                        "<marker>", // Line up marker with markers it's roughly in line with
                    },
                    new List<string>()
                    {
                        "to",
                        "<marker>", // Line up all markers with center and based off of an anchor marker
                    },
                    new List<string>()
                    {
                        "letters",
                        "to",
                        "<marker>", // Line up all letters with center and based off of an anchor marker
                    },
                    new List<string>()
                    {
                        "numbers",
                        "to",
                        "<marker>", // Line up all numbers with center and based off of an anchor marker
                    },
                },
                CommandHandlers.swapArguments,
                false
            )
        );

        this._commands.Add(
            new Command(
                "/justmarkers square help",
                UNIMPLEMENTED_COMMAND_HANDLER,
                "Learn how to us the square command",
                false
            )
        );
    }
}
