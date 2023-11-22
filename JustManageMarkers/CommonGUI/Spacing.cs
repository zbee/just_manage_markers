using ImGuiNET;
using JustManageMarkers.CommonGUI.Interfaces;
using System.Numerics;

namespace JustManageMarkers.CommonGUI;

public class Spacing : IguiElement, IsizeableElement
{
    public string? wantsReference { get; set; } = null;
    public string? _id { get; set; }
    public int _width { get; set; }
    public int _height { get; set; }

    public Spacing(int width = 75, int height = 20, string? id = null)
    {
        this._width = width;
        this._height = height;
        this._id = id;
    }

    public void draw()
    {
        if (this._id != null)
        {
            ImGui.PushID(this._id);
        }

        var noColor = new Vector4(
            0,
            0,
            0,
            0
        );

        ImGui.PushStyleColor(ImGuiCol.Button, noColor);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, noColor);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, noColor);
        ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, Vector2.Zero);
        ImGui.Button("", new Vector2(this._width, this._height));
        ImGui.PopStyleVar(1);
        ImGui.PopStyleColor(3);

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
