#region Boilerplate

using System;

namespace JustManageMarkers.Commands;

public class InvalidArgumentsException : Exception
{
    public InvalidArgumentsException(string message) : base(message)
    {
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
    }

    public static void justManageMarkersConfig(
        JustManageMarkers plugin,
        ArgumentStruct _ = default
    )
    {
        plugin.drawConfigUI();
    }
}
