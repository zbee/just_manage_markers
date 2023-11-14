using FFXIVClientStructs.FFXIV.Client.UI.Misc;
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
            $"Current waymarks: {JsonConvert.SerializeObject(gamePreset, Formatting.Indented)}"
        );

        var blank = new GamePresetPoint();
        var modifiedWaymarks = new List<GamePresetPoint>()
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

        // Get the currently applicable waymarks
        var waymarkOne = (GamePresetPoint) gamePreset.GetType()
            .GetField(markOne.Name)!.GetValue(gamePreset)!;

        var waymarkTwo = (GamePresetPoint) gamePreset.GetType()
            .GetField(markTwo.Name)!.GetValue(gamePreset)!;

        // Swap the values of the currently applicable waymarks
        modifiedWaymarks[markTwo.Index] = waymarkOne;
        modifiedWaymarks[markOne.Index] = waymarkTwo;
    }
}
