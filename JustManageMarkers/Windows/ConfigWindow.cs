using System;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using JustManageMarkers.Core;
using System.Linq;

namespace JustManageMarkers.Windows;

public class ConfigWindow : Window, IDisposable
{
    public int waymarkOne;
    public int waymarkTwo = 4;

    private Configuration Configuration;

    public ConfigWindow(JustManageMarkers plugin) : base(
        JustManageMarkers.Name + ": preferences",
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

        ImGui.Combo(
            "",
            ref waymarkOne,
            Markers.markers.Select(x => x.Name).ToArray(),
            Markers.markers.Count
        );

        ImGui.PopID();
        ImGui.SameLine();
        ImGui.SetNextItemWidth(75);
        ImGui.PushID("SwapWaymarkTwo");

        ImGui.Combo(
            "",
            ref waymarkTwo,
            Markers.markers.Select(x => x.Name).ToArray(),
            Markers.markers.Count
        );

        ImGui.PopID();
        ImGui.SameLine();
        ImGui.Button("Swap");
    }

    public override void Draw()
    {
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

    public void Dispose()
    {
    }
}
