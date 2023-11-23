using ImGuiNET;
using JustManageMarkers.CommonGUI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace JustManageMarkers.CommonGUI;

public ref struct ComboSelection
{
    public ref int _selection;
}

public class Combo : IguiElement, IbackgroundableElement, IsizeableElement
{
    public string? wantsReference { get; set; } = "Int";
    public string? _id { get; set; }

    public Vector4? _backgroundColor { get; set; }
    public int _width { get; set; }
    public int _height { get; set; }
    private string? _label { get; set; }
    private string[] _options { get; set; }

    public Combo(
        string id,
        IEnumerable<string> options,
        string? label = null,
        Vector4? backgroundColor = null,
        int width = 0,
        int heightInItems = 10
    )
    {
        this._id = id;
        this._backgroundColor = backgroundColor;
        this._width = width;
        this._height = heightInItems;
        this._label = label;
        this._options = options.ToArray();
    }

    public void draw()
    {
        throw new WantsReferenceException();
    }

    public void draw(ref int currentItem)
    {
        ImGui.PushID(this._id);

        if (this._backgroundColor != null)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, (Vector4) this._backgroundColor);
        }

        if (this._width != 0)
        {
            ImGui.SetNextItemWidth(this._width);
        }

        ImGui.Combo(
            this._label ?? "",
            ref currentItem,
            this._options,
            this._options.Length,
            this._height
        );

        if (this._backgroundColor != null)
        {
            ImGui.PopStyleColor(1);
        }

        ImGui.PopID();
    }

    public void draw(ref int _, ref int __)
    {
        this.draw();
    }
}
