using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using JustManageMarkers.Core;
using JustManageMarkers.Functions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace JustManageMarkers.Windows;

public class MainWindow : Window, IDisposable
{
    private int waymarkOne;
    private int waymarkTwo = 4;

    private JustManageMarkers Plugin;

    public MainWindow(JustManageMarkers plugin) : base(
        JustManageMarkers.Name,
        ImGuiWindowFlags.NoResize
        | ImGuiWindowFlags.NoScrollbar
        | ImGuiWindowFlags.NoScrollWithMouse
    )
    {
        this.Size = new Vector2(280, 200);

        this.Plugin = plugin;
    }

    public override void Draw()
    {
        this.drawMods();

        this.centerWindow();
        this.PositionCondition = ImGuiCond.FirstUseEver;
    }

    private void drawMods()
    {
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();

        this.centerText("Swap position of two markers");

        ImGui.SetNextItemWidth(75);
        ImGui.PushID("SwapWaymarkOne");
        ImGui.Combo(
            "",
            ref waymarkOne,
            Markers.markers.Select(x => x.Name).ToArray(),
            Markers.markers.Count
        );

        ImGui.PopID();

        ImGui.PushID("SwapTwoWaymarks");
        ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Button, ImGuiColors.ParsedGrey);
        if (ImGui.Button("\u2190 Swap \u2192"))
            Swap.swapMarks(
                Markers.getMarkGivenID(this.waymarkOne),
                Markers.getMarkGivenID(this.waymarkTwo)
            );

        ImGui.PopStyleColor(1);
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

        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();


        this.centerText("Swap positions of letter and number markers");

        ImGui.PushID("SwapWaymarkTypes");
        ImGui.PushStyleColor(ImGuiCol.Button, ImGuiColors.ParsedGrey);
        if (ImGui.Button("ABCD \u2190 Swap \u2192 1234", new Vector2(235, 20)))
            Swap.swapTypes();

        ImGui.PopStyleColor(1);
        ImGui.PopID();
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
