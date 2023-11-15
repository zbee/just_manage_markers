using JustManageMarkers.Core;
using JustManageMarkers.Structures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JustManageMarkers.Functions;

public class Swap
{
    public void swapTypes()
    {
        // TODO: call swap marks on letter and number marks, with some sort of groupBy on the mark colors
        throw new NotImplementedException();
    }

    public void swapMarks(
        Marker markOne,
        Marker markTwo
    )
    {
        JustManageMarkers.Log.Debug("Swapping");

        var waymarksAPI = new WaymarkPresetAPI();

        // Fill a preset with current game waymarks
        var gamePreset = waymarksAPI.createEmptyGamePreset();
        if (!waymarksAPI.getCurrentWaymarksAsPreset(ref gamePreset))
        {
            JustManageMarkers.Log.Error(
                "WaymarkPresetPlugin Failed to get current waymarks"
            );

            JustManageMarkers.Chat.Print(
                "Could not get current waymarks. Do you have any placed?"
            );

            return;
        }

        JustManageMarkers.Log.Debug(
            "Current Waymarks: "
            + JsonConvert.SerializeObject(
                gamePreset
            )
        );

        // Build an empty list of waymarks
        var blank = waymarksAPI.createEmptyGamePresetPoint();
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
        var swappedWaymarks = waymarksAPI.modifyCurrentWaymarkPresetWithPresetPoints(
            modifiedWaymarks
        );

        JustManageMarkers.Log.Debug(
            "Current Waymarks: "
            + JsonConvert.SerializeObject(
                swappedWaymarks
            )
        );

        // Place the new waymarks
        // TODO: if this is false, it works in duties but not the overworld
        // If I recall correctly: if it's true, it works in the overworld but with division issues (toggleable division by 1000?)
        waymarksAPI.placeWaymarks(swappedWaymarks, false);
    }
}
