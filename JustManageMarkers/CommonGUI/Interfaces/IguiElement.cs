namespace JustManageMarkers.CommonGUI.Interfaces;

public interface IguiElement
{
    protected string? _id { get; set; }

    public string? wantsReference { get; set; }

    void draw();

    void draw(ref int refInt1);

    void draw(ref int refInt1, ref int refInt2);
}
