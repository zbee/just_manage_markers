using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ManageMarkers.Windows;

public class ConfigWindow : Window, IDisposable
{
    public int waymarkOne;
    public int waymarkTwo = 4;

    public static readonly String[] markers = new String[8]
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

    private Configuration Configuration;

    public ConfigWindow(Plugin plugin) : base(
        "A Wonderful Configuration Window",
        ImGuiWindowFlags.NoResize |
        ImGuiWindowFlags.NoCollapse |
        ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse
    )
    {
        this.Size = new Vector2(232, 75);
        this.SizeCondition = ImGuiCond.Always;

        this.Configuration = plugin.Configuration;

        ImGui.Begin("MarkerMod");

        if (ImGui.BeginTabBar("MarkerModTabs"))
        {
            if (ImGui.BeginTabItem("Modifications"))
            {
                drawMods();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Preferences"))
            {
                //DrawPreferences();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        ImGui.End();
    }

    private void drawMods()
    {
        ImGui.Text("Swap position of two markers: ");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(75);
        ImGui.PushID("SwapWaymarkOne");
        ImGui.Combo("", ref waymarkOne, markers, markers.Length);
        ImGui.PopID();
        ImGui.SameLine();
        ImGui.SetNextItemWidth(75);
        ImGui.PushID("SwapWaymarkTwo");
        ImGui.Combo("", ref waymarkTwo, markers, markers.Length);
        ImGui.PopID();
        ImGui.SameLine();
        ImGui.Button("Swap");
    }

    public void Dispose() { }

    public override void Draw()
    {
        // can't ref a property, so use a local copy
        var configValue = this.Configuration.SomePropertyToBeSavedAndWithADefault;
        if (ImGui.Checkbox("Random Config Bool", ref configValue))
        {
            this.Configuration.SomePropertyToBeSavedAndWithADefault = configValue;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.Configuration.Save();
        }
    }
}
