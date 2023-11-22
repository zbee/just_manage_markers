using ImGuiNET;
using JustManageMarkers.CommonGUI.Interfaces;
using System.Numerics;

namespace JustManageMarkers.CommonGUI;

public class Text : IguiElement, IcolorableElement, IcenterableElement
{
    public string? wantsReference { get; set; } = null;
    private readonly string _label;
    public string? _id { get; set; }
    public Vector4? _textColor { get; set; }
    public bool _centered { get; set; }

    public Text(
        string label,
        string? id = null,
        Vector4? textColor = null,
        bool centered = false
    )
    {
        this._label = label;
        this._id = id;
        this._textColor = textColor;
        this._centered = centered;
    }

    public void draw()
    {
        if (this._id != null)
        {
            ImGui.PushID(this._id);
        }

        if (this._textColor != null)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, (Vector4) this._textColor);
        }

        if (this._centered)
        {
            // https://github.com/Ottermandias/OtterGui/blob/b09bbcc276363bc994d90b641871e6280898b6e5/Util.cs#L461
            var offset = (ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(this._label).X) / 2;
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
            ImGui.TextUnformatted(this._label);
        }
        else
        {
            ImGui.Text(this._label);
        }

        if (this._textColor != null)
        {
            ImGui.PopStyleColor(1);
        }

        if (this._id != null)
        {
            ImGui.PopID();
        }
    }

    public void draw(ref int _)
    {
        this.draw();
    }

    public void draw(ref int _, ref int __)
    {
        this.draw();
    }
}
