using Dalamud.Plugin;
using ECommons.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

#pragma warning disable CS0169 // Field is never used

namespace JustManageMarkers.Core;

public class WaymarksNotConnectedException : Exception
{
    public WaymarksNotConnectedException(string message = "") : base(message)
    {
    }
}

public class WaymarkPresetAPI : IDisposable
{
    private bool Connected { get; set; }
    private IDalamudPlugin? _waymarkPresetPlugin;

    private const string WAYMARK_PLUGIN_NAME = "WaymarkPresetPlugin";
    private const string WAYMARK_PLUGIN_SAFE_IPC = "GetPresetsForCurrentArea";

    // Prefix for private Type? fields below
    private const string WAYMARK_PRESET_PLUGIN_DESIRED_CLASSES_PREFIX = "_waymarkPreset_";

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    private Type? _waymarkPreset_GamePreset { get; set; }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    private Type? _waymarkPreset_MemoryHandler { get; set; }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    private Type? _waymarkPreset_WaymarkPreset { get; set; }

    #region Connecting to WaymarkPresetPlugin and making sure we can reflect into it

    public WaymarkPresetAPI()
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
            JustManageMarkers.PluginInterface
                .GetIpcSubscriber<SortedDictionary<int, string>>(
                    $"{WAYMARK_PLUGIN_NAME}.{WAYMARK_PLUGIN_SAFE_IPC}"
                )
                .InvokeFunc();
        }
        catch (Exception e)
        {
            // Failed to connect if WaymarkPresetPlugin's IPC is not callable
            JustManageMarkers.Log.Error($"Can't find {WAYMARK_PLUGIN_NAME} in IPC: {e.Message}");
            return;
        }

        // Only connect if WaymarkPresetPlugin's IPC is callable
        this._connect();
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
                this.Connected = true;

                // Subscribe to plugin changes to recheck if WaymarkPresetPlugin is loaded
                DalamudReflector.RegisterOnInstalledPluginsChangedEvents(this._checkIPC);

                JustManageMarkers.Log.Info($"Successfully connected to {WAYMARK_PLUGIN_NAME}.");

                // Load WaymarkPresetPlugin's symbols
                this._loadPluginSymbols();
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

    #region Loading classes from WaymarkPresetPlugin

    private void _loadPluginSymbols()
    {
        // Check private properties for classes desired from WaymarkPresetPlugin
        var waymarkClassesDesired = this.GetType()
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(
                classDesired =>
                    classDesired.Name.StartsWith(WAYMARK_PRESET_PLUGIN_DESIRED_CLASSES_PREFIX)
            )
            .Select(
                classDesired => classDesired.Name.Replace(
                    WAYMARK_PRESET_PLUGIN_DESIRED_CLASSES_PREFIX,
                    ""
                )
            ).ToArray();

        // Load all of WaymarkPresetPlugin's classes that are actual classes
        var waymarkClasses = this._waymarkPresetPlugin!.GetType()
            .Assembly.DefinedTypes
            .Where(
                waymarkType => waymarkType.Namespace is "WaymarkPresetPlugin"
                               && waymarkClassesDesired.Contains(waymarkType.Name)
            );

        // Loop through each class and save it if it matches a private field
        foreach (var waymarkClass in waymarkClasses)
        {
            // Get the actual class from WaymarkPresetPlugin's class' assembly
            var waymarkActualClass = this._waymarkPresetPlugin!.GetType()
                .Assembly.GetType(waymarkClass.FullName!)!;

            // Find the corresponding private property here for the class
            var internalField = this.GetType().GetProperty(
                $"{WAYMARK_PRESET_PLUGIN_DESIRED_CLASSES_PREFIX}{waymarkClass.Name}",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            // Save the class to the internal property
            internalField?.SetValue(this, waymarkActualClass);
        }

        // Loop through the desired classes to check that they all loaded
        foreach (var desiredClass in waymarkClassesDesired)
        {
            // Find the corresponding private property here for the class
            var internalField = this.GetType().GetProperty(
                $"{WAYMARK_PRESET_PLUGIN_DESIRED_CLASSES_PREFIX}{desiredClass}",
                BindingFlags.NonPublic | BindingFlags.Instance
            )!.GetValue(this);

            if (internalField == null)
            {
                throw new Exception($"Failed to load {desiredClass} from {WAYMARK_PLUGIN_NAME}");
            }
        }
    }

    #endregion

    #region Call Wrappers

    public object createEmptyGamePreset()
    {
        this._checkConnection();

        // Return a new instance of the GamePreset class
        return this._waymarkPreset_GamePreset!.Assembly.CreateInstance(
            this._waymarkPreset_GamePreset.FullName!
        )!;
    }

    public object createEmptyWaymarkPreset()
    {
        this._checkConnection();

        // Return a new instance of the GamePreset class
        return this._waymarkPreset_WaymarkPreset!.Assembly.CreateInstance(
            this._waymarkPreset_WaymarkPreset.FullName!
        )!;
    }

    public object getWaymarkPresetAsGamePreset(object gamePreset)
    {
        this._checkConnection();

        throw new NotImplementedException();
    }

    public bool getCurrentWaymarksAsPreset(ref object preset)
    {
        this._checkConnection();

        var argument = new object[]
        {
            preset
        };

        // Call te method
        var result = this._waymarkPreset_MemoryHandler!
            .GetMethod("GetCurrentWaymarksAsPresetData")!.Invoke(
                this._waymarkPreset_MemoryHandler!.FullName,
                argument
            );

        // Update the preset with the new waymarks
        preset = argument[0];

        return result != null;
    }

    # endregion

    public void Dispose()
    {
        this._disconnect();
        GC.SuppressFinalize(this);
    }
}
