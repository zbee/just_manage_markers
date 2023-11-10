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
    public sealed class ManageMarkers : IDalamudPlugin
    {
        public string Name => "just manage markers";

        private DalamudPluginInterface PluginInterface { get; init; }
        private readonly PluginCommandManager<ManageMarkers> commandManager;
        private IPluginLog log { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("ManageMarkers");

        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }

        [Command("/justmarkers")]
        [HelpMessage(
            "Open the main window" +
            "\n/justmarkers config \u2192 Open the config window" +
            "\n " +
            "\n/justmarkers help [chat] \u2192 Open the help window, or print it in chat" +
            "\n/justmarkers advanced help [chat] \u2192 Open the advanced help window, or print it in chat" +
            ""
        )]
        public void justManageMarkers(string command, string args)
        {
            this.log.Info("/jmm used");

            MainWindow.IsOpen = true;
        }

        public void justSwap(string command, string args)
        {
            this.log.Info("/markerswap used");

            // Short circuit help message
            if (args.Trim() == "")
            {
                this.log.Info("no args");
                return;
            }

            string[] markers = Markers.findMarkersIn(args);
        }

        public ManageMarkers(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager,
            [RequiredVersion("1.0")] IPluginLog log
        )
        {
            this.log = log;
            this.PluginInterface = pluginInterface;

            // Load all of our commands
            this.commandManager = new PluginCommandManager<ManageMarkers>(this, commandManager);

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
            commandManager.Dispose();
        }
    }
}
