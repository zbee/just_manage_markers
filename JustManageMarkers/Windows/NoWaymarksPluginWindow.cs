using Dalamud.Interface.Windowing;
using ImGuiNET;
using JustManageMarkers.Core;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;

namespace JustManageMarkers.Windows;

public class NoWaymarksPluginWindow : Window, IDisposable
{
    private JustManageMarkers Plugin;

    public NoWaymarksPluginWindow(JustManageMarkers plugin) : base(
        $"Missing {WaymarkPresetAPI.WAYMARK_PLUGIN_NAME_SPACED}",
        ImGuiWindowFlags.AlwaysAutoResize
    )
    {
        this.Plugin = plugin;
    }

    public override void Draw()
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        var pluginName = textInfo.ToTitleCase(JustManageMarkers.Name);

        ImGui.Text("");
        ImGui.Spacing();
        this.centerText($"{WaymarkPresetAPI.WAYMARK_PLUGIN_NAME_SPACED} was not detected.");
        ImGui.Spacing();
        this.centerText($"Please install it to use {pluginName}.");
        ImGui.Spacing();
        ImGui.Spacing();
        if (ImGui.Button("Open Installer", new Vector2(275, 25)))
            JustManageMarkers.CommandManager.ProcessCommand("/xlplugins");

        ImGui.Spacing();
        if (ImGui.Button("Check Again", new Vector2(275, 25)))
        {
            try
            {
                JustManageMarkers.WaymarkPresetAPI.checkIPC();
                if (JustManageMarkers.WaymarkPresetAPI.Connected)
                    this.IsOpen = false;
            }
            catch (Exception e)
            {
                JustManageMarkers.Log.Error(e.Message);
            }
        }

        ImGui.Spacing();

        this.centerWindow();
    }

    #region OtterGUI's text centering

    // https://github.com/Ottermandias/OtterGui/blob/b09bbcc276363bc994d90b641871e6280898b6e5/Util.cs#L461

    private void centerText(string text)
    {
        var offset = (ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(text).X) / 2;
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
        ImGui.TextUnformatted(text);
    }

    #endregion

    #region Defile's window centering

    // https://discord.com/channels/581875019861328007/653504487352303619/1049081385152946276

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);


    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left; // x position of upper-left corner
        public int Top; // y position of upper-left corner
        public int Right; // x position of lower-right corner
        public int Bottom; // y position of lower-right corner
        public Vector2 Position => new(Left, Top);
        public Vector2 Size => new(Right - Left, Bottom - Top);

/*
        internal bool contains(Vector2 v) => v.X > Position.X && v.X < Position.X + Size.X && v.Y > Position.Y && v.Y < Position.Y + Size.Y;
*/
    }

    /// <summary>
    /// Centers the GUI window to the game window.
    /// </summary>
    private void centerWindow()
    {
        // Get the pointer to the window handle.
        var hWnd = IntPtr.Zero;
        foreach (var pList in Process.GetProcesses())
            if (pList.ProcessName is "ffxiv_dx11" or "ffxiv")
                hWnd = pList.MainWindowHandle;

        // If failing to get the handle then abort.
        if (hWnd == IntPtr.Zero)
            return;

        // Get the game window rectangle
        GetWindowRect(new HandleRef(null, hWnd), out var rGameWindow);

        // Get the size of the current window.
        var vThisSize = ImGui.GetWindowSize();

        // Set the position.
        this.Position = rGameWindow.Position
                        + new Vector2(
                            rGameWindow.Size.X / 2 - vThisSize.X / 2,
                            rGameWindow.Size.Y / 2 - vThisSize.Y / 2
                        );
    }

    #endregion

    public void Dispose()
    {
        this.IsOpen = false;
        GC.SuppressFinalize(this);
    }
}
