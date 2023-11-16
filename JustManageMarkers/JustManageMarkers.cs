using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons;
using JustManageMarkers.Commands;
using JustManageMarkers.Core;
using JustManageMarkers.Windows;
using System.Diagnostics.CodeAnalysis;

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
        [PluginService] public static IFramework Framework { get; private set; } = null!;

        public static string Name => "just manage markers";
        private readonly WindowSystem _windowSystem = new("JustManageMarkers");
        public static WaymarkPresetAPI WaymarkPresetAPI { get; private set; } = null!;

        private readonly Handler _commands;

        public Configuration Configuration { get; }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")] // TODO: remove
        public MainWindow MainWindow { get; }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")] // TODO: remove
        public ConfigWindow ConfigWindow { get; }

        public static NoWaymarksPluginWindow NoWaymarksPluginWindow { get; private set; } = null!;

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
            this._windowSystem.AddWindow(this.MainWindow);
            this._windowSystem.AddWindow(this.ConfigWindow);
            this._windowSystem.AddWindow(NoWaymarksPluginWindow);
            PluginInterface.UiBuilder.Draw += this.drawUI;
            PluginInterface.UiBuilder.OpenMainUi += this.drawMainUI;
            PluginInterface.UiBuilder.OpenConfigUi += this.drawConfigUI;

            //Load WaymarkPresetAPI
            WaymarkPresetAPI = new WaymarkPresetAPI();
        }

        private void drawUI()
        {
            this._windowSystem.Draw();
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
            ECommonsMain.Dispose();

            this.ConfigWindow.Dispose();
            this.MainWindow.Dispose();
            NoWaymarksPluginWindow.Dispose();

            this._windowSystem.RemoveAllWindows();

            this._commands.Dispose();
            WaymarkPresetAPI.Dispose();
        }
    }
}
