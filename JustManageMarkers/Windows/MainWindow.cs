using Dalamud.Interface.Windowing;
using ImGuiNET;
using JustManageMarkers.CommonGUI;
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
    private int _swapWaymarkOne;
    private int _swapWaymarkTwo = 4;
    private int _squareMark;
    private int _squareAnchorMark;
    private int _squareType;
    private int _squareTypeType;
    private int _squareTypeAnchor;

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
        new Spacing(height: 10).draw();

        this.drawSwap();

        new Separator(width: 75).draw();

        if (ImGui.CollapsingHeader("Squaring up markers"))
            this.drawSquare();

        new Spacing(height: 5).draw();

        if (ImGui.CollapsingHeader("Fitting markers to your preferences"))
            this.drawFit();

        new Spacing(height: 5).draw();

        if (ImGui.CollapsingHeader("Basics"))
            this.drawBasics();

        new Spacing(height: 5).draw();

        if (ImGui.CollapsingHeader("Adjusting markers"))
            this.drawAdjust();

        new Spacing(height: 10).draw();

        this.centerWindow();
        this.PositionCondition = ImGuiCond.FirstUseEver;
    }

    private void drawSwap()
    {
        new Text("Swap position of two markers", centered: true).draw();

        new Group(
            true,
            null,
            new Combo(
                "Waymark to Swap 1",
                Markers.markers.Select(x => x.Name),
                width: 75
            ),
            new Button(
                "Swap Two Marks",
                "\u2190 Swap \u2192",
                callback: () => Swap.swapMarks(
                    Markers.getMarkGivenID(this._swapWaymarkOne),
                    Markers.getMarkGivenID(this._swapWaymarkTwo)
                )
            ),
            new Combo(
                "Waymark to Swap 2",
                Markers.markers.Select(x => x.Name),
                width: 75
            )
        ).draw(ref this._swapWaymarkOne, ref this._swapWaymarkTwo);

        new Spacing(height: 5).draw();

        new Text("Swap positions of letter and number markers", centered: true).draw();

        new Button(
            "Swap Types of Marks",
            "ABCD \u2190 Swap \u2192 1234",
            callback: Swap.swapTypes,
            width: -1
        ).draw();
    }

    private void drawSquare()
    {
        new Text("Square one mark with those in 'line' with it", centered: true).draw();

        new Group(
            true,
            null,
            new Spacing(width: 30, height: 1),
            new Combo(
                "one mark to square",
                Markers.markers.Select(x => x.Name),
                label: "",
                width: 65
            ),
            new Button(
                "Square one mark",
                "[square one]",
                width: 90
            )
        ).draw(ref this._squareMark);

        new Spacing(height: 5).draw();

        new Text("Square up all markers with center", centered: true).draw();

        new Button(
            "Square All Marks",
            "[square all]",
            callback: Swap.swapTypes,
            width: -1
        ).draw();

        new Spacing(height: 5).draw();

        new Text("Square up all markers based off of an anchor", centered: true).draw();

        new Group(
            true,
            null,
            new Spacing(width: 1, height: 1),
            new Text("Anchor:"),
            new Combo(
                "Waymark to Anchor to",
                Markers.markers.Select(x => x.Name),
                label: "",
                width: 65
            ),
            new Button(
                "Square with Anchor",
                "[square all to]",
                width: 90
            )
        ).draw(ref this._squareAnchorMark);

        new Spacing(height: 5).draw();

        new Text("Square up a type of marker", centered: true).draw();

        new Group(
            true,
            null,
            new Spacing(width: 5, height: 1),
            new Text("Type:"),
            new Combo(
                "type to square",
                new[]
                {
                    "Letter",
                    "Number"
                },
                label: "",
                width: 65
            ),
            new Button(
                "Square type",
                "[square some]",
                width: 90
            )
        ).draw(ref this._squareType);

        new Spacing(height: 5).draw();

        new Text("Square up a type of marker to an anchor", centered: true).draw();

        new Group(
            true,
            null,
            new Group(
                false,
                "2x int",
                new Group(
                    true,
                    "int",
                    new Text("Type:      "),
                    new Combo(
                        "Waymark Type to Square with anchor",
                        new[]
                        {
                            "Letter",
                            "Number"
                        },
                        label: "",
                        width: 75
                    )
                ),
                new Group(
                    true,
                    "int",
                    new Text("Anchor:"),
                    new Combo(
                        "Waymark to Square type to",
                        Markers.markers.Select(x => x.Name),
                        label: "",
                        width: 75
                    )
                )
            ),
            new Group(
                false,
                null,
                new Spacing(width: 1, height: 5),
                new Button(
                    "Square with Type",
                    "[square some to]"
                )
            )
        ).draw(ref this._squareTypeType, ref this._squareTypeAnchor);

        new Separator(width: 75).draw();
    }

    private void drawFit()
    {
        new Text("Not implemented yet", centered: true).draw();

        new Separator(width: 75).draw();
    }

    private void drawBasics()
    {
        new Text("Not implemented yet", centered: true).draw();

        new Separator(width: 75).draw();
    }

    private void drawAdjust()
    {
        new Text("Not implemented yet", centered: true).draw();
    }

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
