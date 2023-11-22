using Dalamud.Interface.Colors;
using ImGuiNET;
using JustManageMarkers.CommonGUI.Interfaces;
using System.Numerics;

namespace JustManageMarkers.CommonGUI;

public class Separator : IguiElement, IsizeableElement, IbackgroundableElement
{
    public string? wantsReference { get; set; } = null;
    public string? _id { get; set; }
    public int _width { get; set; }
    public int _height { get; set; }
    public int _padding { get; set; }
    public int _paddingBottom { get; set; }
    public Vector4? _backgroundColor { get; set; }

    public Separator(
        int width = -1,
        int height = 2,
        int padding = 10,
        int paddingBottom = 0,
        string? id = null,
        Vector4? backgroundColor = null
    )
    {
        this._width = width;
        this._height = height;
        this._id = id;
        this._backgroundColor = backgroundColor ?? ImGuiColors.ParsedGrey;
        this._padding = padding;
        this._paddingBottom = paddingBottom == 0 ? padding / 2 : paddingBottom;
    }

    public void draw()
    {
        new Spacing(height: this._padding).draw();

        if (this._id != null)
        {
            ImGui.PushID(this._id);
        }

        ImGui.PushStyleColor(ImGuiCol.Button, (Vector4) this._backgroundColor!);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, (Vector4) this._backgroundColor!);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, (Vector4) this._backgroundColor!);
        ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, Vector2.Zero);
        var thisWidth = this._width == -1 ? ImGui.GetContentRegionAvail().X : this._width;
        var offset = (ImGui.GetContentRegionAvail().X - thisWidth) / 2;
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
        ImGui.Button("", new Vector2(this._width, this._height));
        ImGui.PopStyleVar(1);
        ImGui.PopStyleColor(3);

        if (this._id != null)
        {
            ImGui.PopID();
        }

        new Spacing(height: this._paddingBottom).draw();
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
