using JustManageMarkers.Structures;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace JustManageMarkers.Core;

public class Markers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static List<Marker> markers { get; } = new()
    {
        new Marker(
            0,
            "A",
            "A",
            "red"
        ),
        new Marker(
            1,
            "B",
            "B",
            "yellow"
        ),
        new Marker(
            2,
            "C",
            "C",
            "blue"
        ),
        new Marker(
            3,
            "D",
            "D",
            "purple"
        ),
        new Marker(
            4,
            "One",
            "1",
            "red"
        ),
        new Marker(
            5,
            "Two",
            "2",
            "yellow"
        ),
        new Marker(
            6,
            "Three",
            "3",
            "blue"
        ),
        new Marker(
            7,
            "Four",
            "4",
            "purple"
        ),
    };

    public static Marker? findMarkGiven(string possibleMarkerString)
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        possibleMarkerString = textInfo.ToTitleCase(possibleMarkerString.Trim());
        return markers.Find(
            (marker) => marker.Name == possibleMarkerString
                        || marker.ShortName == possibleMarkerString
                        || marker.Index.ToString() == possibleMarkerString
        );
    }
}
