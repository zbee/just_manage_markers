using Dalamud.Game.Command;
using Dalamud.Plugin.Services;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace JustManageMarkers.Commands;

public class Handler : IDisposable
{
    private readonly JustManageMarkers plugin;
    private ICommandManager commandManager;

    private const string ARROW = "\u2192";

    private readonly List<Command> commands = new Commands().getCommands();

    private bool _actualCommandsSet = false;
    private List<string> _actualCommands = new();
    private List<string> _actualHelp = new();

    public Handler(JustManageMarkers plugin, ICommandManager commandManager)
    {
        this.plugin = plugin;
        this.commandManager = commandManager;

        this._actualCommands = this._getActualCommands();
        this._registerActualCommands();
    }

    public void callHandler(string command, string args)
    {
        string commandString = command.Trim() + " " + args.Trim();

        bool foundCommand = false;
        Command commandToUse = default;

        foreach (Command c in this.commands)
        {
            if (commandString.StartsWith(c.Name + " "))
            {
                foundCommand = true;
                commandToUse = c;
            }
        }

        // Fail out if no command was found
        if (!foundCommand)
        {
            this.plugin.Log.Info("command not found");
            return;
        }

        this.plugin.Log.Info("command found!");
        args = commandString.Replace(commandToUse.Name, "").Trim();
        this.plugin.Log.Info(commandToUse.Name + ": " + args);

        this.plugin.Log.Info(JsonConvert.SerializeObject(commandToUse.Arguments));

        // TODO: Check arguments next
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
        foreach (Command command in this.commands)
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
            this.commandManager.AddHandler(
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
            this.commandManager.RemoveHandler(command);
        }
    }

    public void Dispose()
    {
        this._disposeActualCommands();
        GC.SuppressFinalize(this);
    }
}
