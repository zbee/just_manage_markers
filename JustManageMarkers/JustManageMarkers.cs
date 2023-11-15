using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons;
using JustManageMarkers.Commands;
using JustManageMarkers.Core;
using JustManageMarkers.Windows;
using System.Threading;

namespace JustManageMarkers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class JustManageMarkers : IDalamudPlugin
    {
        [PluginService]
        public static DalamudPluginInterface PluginInterface { get; private set; } = null!;

        [PluginService] public static ICommandManager CommandManager { get; private set; } = null!;
        [PluginService] public static IPluginLog Log { get; private set; } = null!;
        [PluginService] public static IChatGui Chat { get; private set; } = null!;

        public static string Name => "just manage markers";
        public WindowSystem WindowSystem = new("JustManageMarkers");
        public static WaymarkPresetAPI WaymarkPresetAPI = null!;

        private readonly Handler _commands;

        public Configuration Configuration { get; }

        public MainWindow MainWindow { get; }
        public ConfigWindow ConfigWindow { get; }
        public static NoWaymarksPluginWindow NoWaymarksPluginWindow { get; set; }

        private CancellationTokenSource _cancellationTokenSource = new();
        public CancellationToken CancelToken { get; private set; }

        public JustManageMarkers()
        {
            // Load all of our commands
            this._commands = new Handler(this);

            // Load ECommons
            ECommonsMain.Init(
                PluginInterface,
                this,
                Module.DalamudReflector
            );

            // Load or create our config
            this.Configuration = PluginInterface.GetPluginConfig() as Configuration
                                 ?? new Configuration();

            this.Configuration.Initialize(PluginInterface);

            // Setup our windows
            this.MainWindow = new MainWindow(this);
            this.ConfigWindow = new ConfigWindow(this);
            NoWaymarksPluginWindow = new NoWaymarksPluginWindow(this);
            this.WindowSystem.AddWindow(this.MainWindow);
            this.WindowSystem.AddWindow(this.ConfigWindow);
            this.WindowSystem.AddWindow(NoWaymarksPluginWindow);
            PluginInterface.UiBuilder.Draw += this.drawUI;
            PluginInterface.UiBuilder.OpenMainUi += this.drawMainUI;
            PluginInterface.UiBuilder.OpenConfigUi += this.drawConfigUI;

            //Load WaymarkPresetAPI
            try
            {
                WaymarkPresetAPI = new WaymarkPresetAPI();
            }
            catch (WaymarksNotConnectedException)
            {
                NoWaymarksPluginWindow.IsOpen = true;
            }
        }

        private void drawUI()
        {
            this.WindowSystem.Draw();
        }

        public void drawMainUI()
        {
            this.MainWindow.IsOpen = true;
        }

        public void drawConfigUI()
        {
            this.ConfigWindow.IsOpen = true;
        }

        public void Dispose()
        {
            this.ConfigWindow.Dispose();
            this.MainWindow.Dispose();
            NoWaymarksPluginWindow.Dispose();

            this.WindowSystem.RemoveAllWindows();

            this._commands.Dispose();
            ECommonsMain.Dispose();
            WaymarkPresetAPI.Dispose();
        }
    }
}
