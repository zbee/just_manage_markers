using System;

namespace ManageMarkers;

public class Markers
{
    public static string[] Strings { get; } = new String[8]
    {
        "A",
        "B",
        "C",
        "D",
        "One",
        "Two",
        "Three",
        "Four"
    };

    public static char[] Chars { get; } = new char[8]
    {
        'A',
        'B',
        'C',
        'D',
        '1',
        '2',
        '3',
        '4'
    };

    public static string[] findMarkersIn(string possibleMarkerString)
    {
        return Strings;
    }
}
