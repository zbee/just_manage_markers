using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ManageMarkers.Attributes;
using ManageMarkers.Windows;

namespace ManageMarkers
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "just manage markers";

        private DalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        private IPluginLog log { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("ManageMarkers");

        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }

        [Command("/just")]
        [HelpMessage("Use /just manage markers to open the main window.")]
        public void justManageMarkers(string command, string args)
        {
            this.log.Info(args);
            this.log.Info("Message sent successfully.");
            MainWindow.IsOpen = true;
        }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager,
            [RequiredVersion("1.0")] IPluginLog log
        )
        {
            this.log = log;
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "icon.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this, goatImage);

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            this.PluginInterface.UiBuilder.OpenMainUi += DrawConfigUI;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            ConfigWindow.Dispose();
            MainWindow.Dispose();

            var commands = CommandManager.Commands;
            foreach (var (key, value) in commands)
            {
                this.log.Debug("key: " + key + ", value: " + value);
            }
        }
    }
}
