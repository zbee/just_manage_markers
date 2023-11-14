using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ECommons;
using JustManageMarkers.Commands;
using JustManageMarkers.Windows;

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

        private readonly Handler _commands;

        public Configuration Configuration { get; }

        public ConfigWindow ConfigWindow { get; }
        public MainWindow MainWindow { get; }

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
            this.ConfigWindow = new ConfigWindow(this);
            this.MainWindow = new MainWindow(this);
            this.WindowSystem.AddWindow(this.ConfigWindow);
            this.WindowSystem.AddWindow(this.MainWindow);
            PluginInterface.UiBuilder.Draw += this.drawUI;
            PluginInterface.UiBuilder.OpenMainUi += this.drawMainUI;
            PluginInterface.UiBuilder.OpenConfigUi += this.drawConfigUI;
        }

        private void drawUI()
        {
            this.WindowSystem.Draw();
        }

        public void drawMainUI()
        {
            MainWindow.IsOpen = true;
        }

        public void drawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            this._commands.Dispose();
            ECommonsMain.Dispose();
            ConfigWindow.Dispose();
            MainWindow.Dispose();
        }
    }
}
