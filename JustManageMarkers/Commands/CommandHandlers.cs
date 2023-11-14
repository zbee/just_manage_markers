#region Boilerplate

using JustManageMarkers.Core;
using JustManageMarkers.Functions;
using Newtonsoft.Json;
using System;
using System.Linq;

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
        ArgumentStruct _,
        int __
    )
    {
        plugin.drawMainUI();
    }

    public static void justManageMarkersConfig(
        JustManageMarkers plugin,
        ArgumentStruct _,
        int __
    )
    {
        plugin.drawConfigUI();
    }

    private const int SWAP_TYPES = 0;
    private const int SWAP_MARKS = 1;

    public static int swapArguments(
        JustManageMarkers plugin,
        ArgumentStruct arguments
    )
    {
        // Fail out if an insufficient amount of arguments was provided
        if (arguments.Count != 2)
        {
            throw new InvalidArgumentsException(
                arguments.OriginalArguments,
                "You must provide exactly two arguments"
            );
        }

        JustManageMarkers.Log.Debug(arguments.OriginalArguments);
        JustManageMarkers.Log.Debug(arguments.ToString());
        JustManageMarkers.Log.Debug(
            "accepts: " + JsonConvert.SerializeObject(arguments.AcceptedArguments)
        );

        // Check for exact arguments
        if (arguments.AcceptedArguments.Any(
                argumentVariation =>
                    arguments.Argument1.Value == argumentVariation[0]
                    && arguments.Argument2.Value == argumentVariation[1]
            ))
        {
            return SWAP_TYPES;
        }

        // Handle variable arguments
        var markOne = Markers.findMarkGiven(arguments.Argument1.Value!);
        var markTwo = Markers.findMarkGiven(arguments.Argument2.Value!);
        if (markOne != null
            && markTwo != null)
        {
            return SWAP_MARKS;
        }

        throw new InvalidArgumentsException(arguments.OriginalArguments);
    }

    public static void swap(
        JustManageMarkers plugin,
        ArgumentStruct arguments,
        int parseResult
    )
    {
        JustManageMarkers.Log.Debug("Handling swap");

        var swapFunctions = new Swap();

        // Swap letter and number markers
        if (parseResult == SWAP_TYPES)
        {
            swapFunctions.swapTypes();
            return;
        }

        // Swap the given markers
        swapFunctions.swapMarks(
            Markers.getMarkGiven(arguments.Argument1.Value!),
            Markers.getMarkGiven(arguments.Argument2.Value!)
        );
    }
}
