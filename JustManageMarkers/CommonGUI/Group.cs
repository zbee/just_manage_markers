using ImGuiNET;
using JustManageMarkers.CommonGUI.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace JustManageMarkers.CommonGUI;

public class Group : IguiElementCollection, IguiElement
{
    public string? _id { get; set; } = null;
    public string? wantsReference { get; set; }
    private bool _horizontal { get; set; }
    public List<IguiElement> _elements { get; set; }

    public Group(bool horizontal, string? wantsReference, params IguiElement[] elements)
    {
        // Just save the list of elements as a list
        this._elements = elements.ToList();

        this._horizontal = horizontal;
        this.wantsReference = wantsReference;
    }

    public void draw()
    {
        ImGui.BeginGroup();

        foreach (var element in this._elements)
        {
            // Draw each element
            element.draw();

            // Put it on the same line, if it's not the last element
            if (!element.Equals(this._elements.Last()) && this._horizontal)
                ImGui.SameLine();
        }

        ImGui.EndGroup();
    }

    public void draw(ref int refInt1)
    {
        var used1 = false;

        ImGui.BeginGroup();

        foreach (var element in this._elements)
        {
            if (element.wantsReference != null)
            {
                if (!used1)
                {
                    element.draw(ref refInt1);
                    used1 = true;
                }
            }
            else
                element.draw();

            // Put it on the same line, if it's not the last element
            if (!element.Equals(this._elements.Last()) && this._horizontal)
                ImGui.SameLine();
        }

        ImGui.EndGroup();
    }

    public void draw(ref int refInt1, ref int refInt2)
    {
        var used1 = false;
        var used2 = false;

        ImGui.BeginGroup();

        foreach (var element in this._elements)
        {
            if (element.wantsReference != null)
            {
                if (element is Group)
                {
                    if (element.wantsReference.StartsWith("2x"))
                    {
                        element.draw(ref refInt1, ref refInt2);
                        used1 = true;
                        used2 = true;
                    }
                    else if (!used1)
                    {
                        element.draw(ref refInt1);
                        used1 = true;
                    }
                    else if (!used2)
                    {
                        element.draw(ref refInt2);
                        used2 = true;
                    }
                }
                else if (!used1)
                {
                    element.draw(ref refInt1);
                    used1 = true;
                }
                else if (!used2)
                {
                    element.draw(ref refInt2);
                    used2 = true;
                }
            }
            else
                element.draw();

            // Put it on the same line, if it's not the last element
            if (!element.Equals(this._elements.Last()) && this._horizontal)
                ImGui.SameLine();
        }

        ImGui.EndGroup();
    }
}
