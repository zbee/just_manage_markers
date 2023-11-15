using JustManageMarkers.Core;
using JustManageMarkers.Structures;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace JustManageMarkers.Functions;

public class Swap
{
    public static void swapTypes()
    {
        JustManageMarkers.Log.Debug("Swapping Types");

        // Fill a preset with current game waymarks
        var gamePreset = JustManageMarkers.WaymarkPresetAPI.createEmptyGamePreset();
        if (!JustManageMarkers.WaymarkPresetAPI.getCurrentWaymarksAsPreset(ref gamePreset))
        {
            JustManageMarkers.Log.Error(
                "WaymarkPresetPlugin Failed to get current waymarks"
            );

            JustManageMarkers.Chat.Print(
                "Could not get current waymarks. Do you have any placed?"
            );

            return;
        }

        JustManageMarkers.Log.Verbose(
            "Current Waymarks: "
            + JsonConvert.SerializeObject(
                gamePreset
            )
        );

        // Build an empty list of waymarks
        var blank = JustManageMarkers.WaymarkPresetAPI.createEmptyGamePresetPoint();
        var modifiedWaymarks = new List<object>()
        {
            blank,
            blank,
            blank,
            blank,
            blank,
            blank,
            blank,
            blank,
        };

        var marksByColor = Markers.markers.GroupBy(
            mark => mark.Color
        );

        foreach (var colors in marksByColor.Select(colorGroup => colorGroup.ToList()))
        {
            // Get the currently applicable waymarks from the game's waymarks
            var waymarkOne = gamePreset.GetType()
                .GetField(colors[0].Name)!.GetValue(gamePreset)!;

            var waymarkTwo = gamePreset.GetType()
                .GetField(colors[1].Name)!.GetValue(gamePreset)!;

            // Swap the values of the currently applicable waymarks
            modifiedWaymarks[colors[1].Index] = waymarkOne;
            modifiedWaymarks[colors[0].Index] = waymarkTwo;
        }

        // Create a new WaymarkPreset with the modified waymark values over the game's current marks
        var swappedWaymarks = JustManageMarkers.WaymarkPresetAPI
            .modifyCurrentWaymarkPresetWithPresetPoints(
                modifiedWaymarks
            );

        JustManageMarkers.Log.Verbose(
            "Swapped Waymarks: "
            + JsonConvert.SerializeObject(
                swappedWaymarks
            )
        );

        // Place the new waymarks
        // TODO: if this is false, it works in duties but not the overworld
        // If I recall correctly: if it's true, it works in the overworld but with division issues (toggleable division by 1000?)
        JustManageMarkers.WaymarkPresetAPI.placeWaymarks(swappedWaymarks, false);

        JustManageMarkers.Log.Info(
            "Swapped types"
        );

        JustManageMarkers.Chat.Print(
            "Swapped letters and numbers",
            JustManageMarkers.Name
        );
    }

    public static void swapMarks(Marker markOne, Marker markTwo)
    {
        JustManageMarkers.Log.Debug(
            $"Swapping {markOne.ShortName} and {markTwo.ShortName}"
        );

        // Fill a preset with current game waymarks
        var gamePreset = JustManageMarkers.WaymarkPresetAPI.createEmptyGamePreset();
        if (!JustManageMarkers.WaymarkPresetAPI.getCurrentWaymarksAsPreset(ref gamePreset))
        {
            JustManageMarkers.Log.Error(
                "WaymarkPresetPlugin Failed to get current waymarks"
            );

            JustManageMarkers.Chat.Print(
                "Could not get current waymarks. Do you have any placed?"
            );

            return;
        }

        JustManageMarkers.Log.Verbose(
            "Current Waymarks: "
            + JsonConvert.SerializeObject(
                gamePreset
            )
        );

        // Build an empty list of waymarks
        var blank = JustManageMarkers.WaymarkPresetAPI.createEmptyGamePresetPoint();
        var modifiedWaymarks = new List<object>()
        {
            blank,
            blank,
            blank,
            blank,
            blank,
            blank,
            blank,
            blank,
        };

        // Get the currently applicable waymarks from the game's waymarks
        var waymarkOne = gamePreset.GetType()
            .GetField(markOne.Name)!.GetValue(gamePreset)!;

        var waymarkTwo = gamePreset.GetType()
            .GetField(markTwo.Name)!.GetValue(gamePreset)!;

        // Swap the values of the currently applicable waymarks
        modifiedWaymarks[markTwo.Index] = waymarkOne;
        modifiedWaymarks[markOne.Index] = waymarkTwo;

        // Create a new WaymarkPreset with the modified waymark values over the game's current marks
        var swappedWaymarks = JustManageMarkers.WaymarkPresetAPI
            .modifyCurrentWaymarkPresetWithPresetPoints(
                modifiedWaymarks
            );

        JustManageMarkers.Log.Verbose(
            "Swapped Waymarks: "
            + JsonConvert.SerializeObject(
                swappedWaymarks
            )
        );

        // Place the new waymarks
        // TODO: if this is false, it works in duties but not the overworld
        // If I recall correctly: if it's true, it works in the overworld but with division issues (toggleable division by 1000?)
        JustManageMarkers.WaymarkPresetAPI.placeWaymarks(swappedWaymarks, false);

        JustManageMarkers.Log.Info(
            $"Swapped {markOne.ShortName} and {markTwo.ShortName}"
        );

        JustManageMarkers.Chat.Print(
            $"Swapped {markOne.ShortName} and {markTwo.ShortName}",
            JustManageMarkers.Name
        );
    }
}
