using Dalamud.Interface.Colors;
using ImGuiNET;
using JustManageMarkers.CommonGUI.Interfaces;
using System;
using System.Numerics;

namespace JustManageMarkers.CommonGUI;

public class Button : IguiElement, IsizeableElement, IbackgroundableElement,
    IcolorableElement, IonClickableElement
{
    public string? wantsReference { get; set; } = null;
    public string _id { get; set; }
    public string _label { get; set; }
    public int _width { get; set; }
    public int _height { get; set; }
    public Vector4? _backgroundColor { get; set; }
    public Vector4? _textColor { get; set; }
    public Action? _callback { get; set; }

    public Button(
        string id,
        string label,
        Action? callback = null,
        Vector4? textColor = null,
        Vector4? backgroundColor = null,
        int width = 0,
        int height = 20,
        bool widthOfLast = false,
        bool heightOfLast = false
    )
    {
        this._id = id;
        this._label = label;
        this._backgroundColor = backgroundColor ?? ImGuiColors.ParsedGrey;
        this._textColor = textColor;
        this._callback = callback;
        this._width = widthOfLast ? -2 : width;
        this._height = heightOfLast ? -2 : height;
    }

    public void draw()
    {
        ImGui.PushID(this._id);
        ImGui.PushStyleColor(ImGuiCol.Button, (Vector4) this._backgroundColor);

        var size = Vector2.Zero;
        if (this._width != 0 || this._height != 0)
        {
            var lastSize = ImGui.GetItemRectSize();
            size = new Vector2(
                this._width == -2 ? lastSize.X : this._width,
                this._height == -2 ? lastSize.Y : this._height
            );
        }

        if (this._textColor != null)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, (Vector4) this._textColor);
        }

        if (this._callback != null)
        {
            if (ImGui.Button(this._label, size))
            {
                this._callback();
            }
        }
        else
        {
            ImGui.Button(this._label, size);
        }

        if (this._textColor != null)
        {
            ImGui.PopStyleColor(1);
        }

        ImGui.PopStyleColor(1);
        ImGui.PopID();
    }

    public void draw(ref int refInt1)
    {
        this.draw();
    }

    public void draw(ref int _, ref int __)
    {
        this.draw();
    }
}
