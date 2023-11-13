#region Boilerplate

using System;

namespace JustManageMarkers.Commands;

public class InvalidArgumentsException : Exception
{
    public string Arguments { get; }

    public InvalidArgumentsException(string arguments, string message = "") : base(message)
    {
        this.Arguments = arguments;
    }
}

#endregion

public static class CommandHandlers
{
    public static void justManageMarkers(
        JustManageMarkers plugin,
        ArgumentStruct _
    )
    {
        plugin.drawMainUI();
    }

    public static void justManageMarkersConfig(
        JustManageMarkers plugin,
        ArgumentStruct _
    )
    {
        plugin.drawConfigUI();
    }

    public static ArgumentStruct swapArguments(
        JustManageMarkers plugin,
        ArgumentStruct arguments
    )
    {
        JustManageMarkers.Log.Info("Handling arguments for swap");

        // Fail out if an insufficient amount of arguments was provided
        if (arguments.Count != 2)
        {
            throw new InvalidArgumentsException(
                arguments.OriginalArguments,
                "You must provide exactly two arguments"
            );
        }

        JustManageMarkers.Log.Debug(arguments.OriginalArguments + " (" + arguments.Count + ")");
        JustManageMarkers.Log.Debug("1: " + arguments.Argument1);
        JustManageMarkers.Log.Debug("2: " + arguments.Argument2);
        JustManageMarkers.Log.Debug("3: " + arguments.Argument3);
        JustManageMarkers.Log.Debug("4: " + arguments.Argument4);
        JustManageMarkers.Log.Debug("5: " + arguments.Argument5);

        return arguments;
    }
}
