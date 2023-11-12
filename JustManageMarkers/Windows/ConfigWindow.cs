using System;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace JustManageMarkers.Windows;

public class ConfigWindow : Window, IDisposable
{
    public int waymarkOne;
    public int waymarkTwo = 4;

    public static readonly String[] markers = Markers.Strings;

    private Configuration Configuration;

    public ConfigWindow(JustManageMarkers plugin) : base(
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
