using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ManageMarkers.Windows;

public class ConfigWindow : Window, IDisposable
{
    public int waymarkOne;
    public int waymarkTwo = 4;

    public static readonly String[] markers = Markers.Strings;

    private Configuration Configuration;

    public ConfigWindow(ManageMarkers plugin) : base(
        plugin.Name + ": preferences",
        ImGuiWindowFlags.AlwaysAutoResize
    )
    {
        this.Configuration = plugin.Configuration;
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
    }

    public void Dispose() { }
}
