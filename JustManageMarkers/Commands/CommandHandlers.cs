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
        ArgumentStruct _ = default
    )
    {
        plugin.drawMainUI();
        JustManageMarkers.Chat.Print("test");
    }

    public static void justManageMarkersConfig(
        JustManageMarkers plugin,
        ArgumentStruct _ = default
    )
    {
        plugin.drawConfigUI();
    }
}
