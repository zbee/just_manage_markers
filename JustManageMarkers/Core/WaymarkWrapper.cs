using Dalamud.Plugin;
using ECommons.Reflection;
using Newtonsoft.Json;
using System;

namespace JustManageMarkers.Core;

public class WaymarksNotConnectedException : Exception
{
    public WaymarksNotConnectedException(string message = "") : base(message)
    {
    }
}

public class WaymarkWrapper : IDisposable
{
    // we can check WaymarkPreset's IPCs like this: https://l.zbee.me/40JlipB
    // (with WaymarkPreset's IPCs of course: https://l.zbee.me/40DWeQS)
    // (IPC doc https://l.zbee.me/3QXRv9x)
    // we can check WaymarkPreset's callable like this: https://l.zbee.me/40ysULH
    // invoke commands like this: https://l.zbee.me/49zmVKw

    private IDalamudPlugin? _waymarkPresetPlugin;
    private const string WAYMARK_PLUGIN_NAME = "WaymarkPresetPlugin";

    public bool Connected { get; private set; }

    #region Connection

    public WaymarkWrapper()
    {
        // Default values
        this._waymarkPresetPlugin = null;
        this.Connected = false;
        // Attempt to connect
        this._checkIPC();
    }

    private void _checkIPC()
    {
        // Attempt to call a safe IPC to check if WaymarkPresetPlugin is loaded
        try
        {
            if (
                JustManageMarkers.PluginInterface
                .GetIpcSubscriber<bool>($"{WAYMARK_PLUGIN_NAME}.GetPresetsForCurrentArea")
                .InvokeFunc())
            {
                // Only connect if WaymarkPresetPlugin's IPC is callable
                this._connect();
            }
        }
        catch
        {
            // Failed to connect if WaymarkPresetPlugin's IPC is not callable
        }
    }

    private void _connect()
    {
        try
        {
            // Attempt to get WaymarkPresetPlugin
            if (
                DalamudReflector.TryGetDalamudPlugin(
                    WAYMARK_PLUGIN_NAME,
                    out var plugin,
                    false,
                    true
                )
            )
            {
                // Save WaymarkPresetPlugin
                this._waymarkPresetPlugin = plugin;
                this._loadPluginSymbols();
                this.Connected = true;

                // Subscribe to plugin changes to recheck if WaymarkPresetPlugin is loaded
                DalamudReflector.RegisterOnInstalledPluginsChangedEvents(this._checkIPC);

                JustManageMarkers.Log.Info($"Successfully connected to {WAYMARK_PLUGIN_NAME}.");
            }
            else
            {
                throw new Exception($"{WAYMARK_PLUGIN_NAME} is not initialized");
            }
        }
        catch (Exception e)
        {
            this._disconnect();
            JustManageMarkers.Log.Error($"Can't find {WAYMARK_PLUGIN_NAME} plugin: " + e.Message);
            if (e.StackTrace != null)
            {
                JustManageMarkers.Log.Error(e.StackTrace);
            }
        }
    }

    private void _checkConnection()
    {
        if (!this.Connected)
        {
            throw new WaymarksNotConnectedException();
        }
    }

    private void _disconnect()
    {
        this._waymarkPresetPlugin = null;
        this.Connected = false;
    }

    #endregion

    #region Loading

    private void _loadPluginSymbols()
    {
        this._checkConnection();

        // TODO: set a bunch of private fields to WaymarkPresetPlugin's classes such that they can be .GetMethod()'d on


        throw new Exception($"X symbol failed to load");
    }

    #endregion

    #region Call Wrappers

    public void debugAttemptingToAccessPlugin()
    {
        this._checkConnection();

        var wppName = this._waymarkPresetPlugin!.GetType().AssemblyQualifiedName!;
        JustManageMarkers.Log.Info($"{WAYMARK_PLUGIN_NAME} qualified name: {wppName}");

        var wppTypes = this._waymarkPresetPlugin!.GetType().Assembly.DefinedTypes;
        JustManageMarkers.Log.Info(
            $"{WAYMARK_PLUGIN_NAME} types: "
            + JsonConvert.SerializeObject(wppTypes)
        );
    }

    # endregion

    public void Dispose()
    {
        this._disconnect();
        GC.SuppressFinalize(this);
    }
}
