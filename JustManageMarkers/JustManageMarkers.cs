using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using JustManageMarkers.Attributes;
using JustManageMarkers.Windows;

namespace JustManageMarkers
{
    public sealed class JustManageMarkers : IDalamudPlugin
    {
        public string Name => "just manage markers";

        public DalamudPluginInterface PluginInterface { get; init; }
        private readonly PluginCommandManager<JustManageMarkers> commandManager;
        public IPluginLog Log { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("JustManageMarkers");

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
            this.Log.Info("/justmarkers used");

            MainWindow.IsOpen = true;
        }

        public void justSwap(string command, string args)
        {
            this.Log.Info("/markerswap used");

            // Short circuit help message
            if (args.Trim() == "")
            {
                this.Log.Info("no args");
                return;
            }

            string[] markers = Markers.findMarkersIn(args);
        }

        public JustManageMarkers(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager,
            [RequiredVersion("1.0")] IPluginLog log
        )
        {
            this.Log = log;
            this.PluginInterface = pluginInterface;

            // Load all of our commands
            this.commandManager = new PluginCommandManager<JustManageMarkers>(this, commandManager);

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "icon.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this, goatImage);

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            this.PluginInterface.UiBuilder.Draw += drawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += drawConfigUI;
            this.PluginInterface.UiBuilder.OpenMainUi += drawConfigUI;
        }

        private void drawUI()
        {
            this.WindowSystem.Draw();
        }

        public void drawConfigUI()
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
