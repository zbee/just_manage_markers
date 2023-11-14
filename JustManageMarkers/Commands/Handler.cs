using Dalamud.Game.Command;
using JustManageMarkers.Core;
using System.Collections.Generic;
using System;

namespace JustManageMarkers.Commands;

public class Handler : IDisposable
{
    private readonly JustManageMarkers _plugin;

    private const string ARROW = "\u2192";

    private readonly List<Command> _commands = new Commands().getCommands();

    private bool _actualCommandsSet = false;
    private List<string> _actualCommands = new();
    private List<string> _actualHelp = new();

    public Handler(JustManageMarkers plugin)
    {
        this._plugin = plugin;
        this._actualCommands = this._getActualCommands();
        this._registerActualCommands();
    }

    public void callHandler(string command, string args)
    {
        string commandString = command.Trim() + " " + args.Trim();

        bool foundCommand = false;
        Command commandToUse = default;

        // TODO: Generate patterns to match against from the command name and arguments in `new Command()`
        // Loop through each command and keep the last one that matches
        foreach (Command c in this._commands)
        {
            if (commandString.StartsWith(c.Name))
            {
                foundCommand = true;
                commandToUse = c;
            }
        }

        // Fail out if no command was found
        if (!foundCommand)
        {
            JustManageMarkers.Log.Info("Command not found: " + commandString);
            JustManageMarkers.Chat.Print(
                "This command was not recognized, please check the help command"
            );

            return;
        }

        // Load up the exact arguments, and trim them
        args = commandString.Replace(
            commandToUse.Name,
            ""
        ).Trim();

        // Fail out if the command doesn't accept arguments and has any
        if (commandToUse.ArgumentParser == null && args != "")
        {
            JustManageMarkers.Log.Warning("No arguments accepted: " + commandToUse.Name);
            JustManageMarkers.Chat.Print(
                "This command does not accept arguments, please check the help command"
            );

            return;
        }

        // Run the handler with no arguments if the command doesn't accept arguments
        if (commandToUse.Arguments == null || commandToUse.ArgumentParser == null)
        {
            var emptyArguments = new ArgumentStruct(null, null);
            commandToUse.Handler.Invoke(this._plugin, emptyArguments, 0);
            return;
        }

        // Parse the arguments and call the handler with their return, catching an argument exception
        try
        {
            var arguments = new ArgumentStruct(commandToUse.Arguments, args);

            var result = commandToUse.ArgumentParser.Invoke(
                this._plugin,
                arguments
            );

            commandToUse.Handler.Invoke(
                this._plugin,
                arguments,
                result
            );
        }
        // Fail out if the arguments are invalid
        catch (InvalidArgumentsException error)
        {
            JustManageMarkers.Log.Warning("Invalid arguments: " + error.Arguments);
            if (error.Message != "")
            {
                JustManageMarkers.Chat.PrintError(
                    error.Message
                );
            }
            else
            {
                JustManageMarkers.Chat.PrintError(
                    "This command's arguments were invalid, please check the help command"
                );
            }
        }
        catch (WaymarksNotConnectedException)
        {
            JustManageMarkers.Chat.PrintError(
                "Waymark Preset Plugin is not available, please ensure it is installed"
            );
        }
    }

    private string _getDalamudCommand(Command command)
    {
        // Get first word in command
        string possibleCommand = command.Name.Split(" ")[0];

        // Make sure it starts with a slash
        if (!possibleCommand.StartsWith("/"))
        {
            possibleCommand = "/" + possibleCommand;
        }

        return possibleCommand;
    }

    private List<string> _getActualCommands()
    {
        // If commands have been retrieved
        if (this._actualCommandsSet)
        {
            return this._actualCommands;
        }

        // Loop over the commands
        foreach (Command command in this._commands)
        {
            // Get the actual Dalamud command from each command
            string dalamudCommand = this._getDalamudCommand(command);
            if (!this._actualCommands.Contains(dalamudCommand))
            {
                // If the actual Dalamud command is not in the list of actual commands, add it
                this._actualCommands.Add(dalamudCommand);

                string helpText = "";
                // Add the help message to the list of help messages as well
                if (command.IncludeInHelp)
                {
                    helpText += command.ShortDescription;
                }

                // Add the help message to the list of help messages as well
                this._actualHelp.Add(helpText);
            }
            else
            {
                // If the actual Dalamud command already in the list, append the help, if requested
                int forCommand = this._actualCommands.IndexOf(dalamudCommand);
                string helpText = "";
                if (command.IncludeInHelp)
                {
                    string extraSpace = command.SpaceAbove ? "\n" : "";
                    helpText += "\n"
                                + extraSpace
                                + command.Name
                                + " "
                                + ARROW
                                + " "
                                + command.ShortDescription;
                }

                this._actualHelp[forCommand] += helpText;
            }
        }

        // Don't load the commands again
        this._actualCommandsSet = true;

        return this._actualCommands;
    }

    private void _registerActualCommands()
    {
        int forCommand = 0;
        foreach (string command in this._actualCommands)
        {
            // Register each command with Dalamud
            JustManageMarkers.CommandManager.AddHandler(
                command, // the /command
                new CommandInfo(this.callHandler) // Always call the handler here to sort commands
                {
                    // Any help info
                    HelpMessage = this._actualHelp[forCommand],
                    ShowInHelp = this._actualHelp[forCommand] != "",
                }
            );

            forCommand++;
        }
    }

    private void _disposeActualCommands()
    {
        foreach (string command in this._actualCommands)
        {
            JustManageMarkers.CommandManager.RemoveHandler(command);
        }
    }

    public void Dispose()
    {
        this._disposeActualCommands();
        GC.SuppressFinalize(this);
    }
}
