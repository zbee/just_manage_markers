namespace JustManageMarkers.Structures;

public readonly struct Marker
{
    public int Index { get; }
    public string Name { get; }
    public string ShortName { get; }
    public string Color { get; }

    public Marker(int index, string name, string shortName, string color)
    {
        this.Index = index;
        this.Name = name;
        this.ShortName = shortName;
        this.Color = color;
    }
}
