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
    private Type? _waymarkPreset_GamePresetPoint { get; set; }

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

    public object createEmptyGamePresetPoint()
    {
        this._checkConnection();

        // Return a new instance of the GamePresetPoint class
        return this._waymarkPreset_GamePresetPoint!.Assembly.CreateInstance(
            this._waymarkPreset_GamePresetPoint.FullName!
        )!;
    }

    public string gamePresetPointToString(object gamePresetPoint)
    {
        this._checkConnection();

        // Run the ToString method on the GamePresetPoint
        return (gamePresetPoint.GetType().GetMethod("ToString")!
            .Invoke(
                gamePresetPoint,
                Array.Empty<object>()
            ) as string)!;
    }

    public object createEmptyWaymarkPreset()
    {
        this._checkConnection();

        // Return a new instance of the WaymarkPreset class
        return this._waymarkPreset_WaymarkPreset!.Assembly.CreateInstance(
            this._waymarkPreset_WaymarkPreset.FullName!
        )!;
    }

    public object getWaymarkPresetAsGamePreset(object gamePreset)
    {
        this._checkConnection();

        // Call the method, converting the passed preset
        return gamePreset.GetType().GetMethod("GetAsGamePreset")!
            .Invoke(
                gamePreset,
                Array.Empty<object>()
            )!;
    }

    public bool getCurrentWaymarksAsPreset(ref object preset)
    {
        this._checkConnection();

        var argument = new[]
        {
            preset
        };

        // Call the method
        var result = this._waymarkPreset_MemoryHandler!
            .GetMethod("GetCurrentWaymarksAsPresetData")!.Invoke(
                this._waymarkPreset_MemoryHandler!.FullName,
                argument
            );

        // Update the preset with the new waymarks
        preset = argument[0];

        return result != null;
    }

    public void placeWaymarks(object gamePreset, bool allowClientSide = false)
    {
        this._checkConnection();

        // Pass the preset and the client-side preference
        var argument = new[]
        {
            gamePreset,
            allowClientSide
        };

        // Call the method
        this._waymarkPreset_MemoryHandler!
            .GetMethod("PlacePreset")!.Invoke(
                this._waymarkPreset_MemoryHandler!.FullName,
                argument
            );
    }

    public object modifyCurrentWaymarkPresetWithPresetPoints(List<object> modifiedGamePreset)
    {
        this._checkConnection();

        // Create an empty preset to work from
        var newWaymarksPreset = this.createEmptyWaymarkPreset();

        // Get the current waymarks
        var currentGamePreset = this.createEmptyGamePreset();
        this.getCurrentWaymarksAsPreset(ref currentGamePreset);

        // Make an empty GamePresetPoint to compare against
        var emptyGamePresetPoint = this.createEmptyGamePresetPoint();
        var blank = this.gamePresetPointToString(emptyGamePresetPoint);

        var waymarkPresetAttributes = new[]
        {
            "X",
            "Y",
            "Z",
            "Active"
        };

        // Loop through each waymark in the preset and update its attributes
        foreach (var marker in Markers.markers)
        {
            // Set current waymark in the new WaymarkPreset to edit
            var newMark
                = newWaymarksPreset.GetType().GetProperty(marker.Name)!
                    .GetValue(newWaymarksPreset)!;

            // Set current waymark in the modified GamePresetPoints to fill from
            var modifiedMark = modifiedGamePreset[marker.Index];

            // Set current waymark in the current WaymarkPreset to fill from
            var currentMark
                = currentGamePreset.GetType().GetField(marker.Name)!.GetValue(currentGamePreset)!;

            // Get the string value of the modified and current waymarks to check if blank
            var testIfModifiedBlank = this.gamePresetPointToString(
                modifiedGamePreset[marker.Index]
            );

            var testIfCurrentBlank = this.gamePresetPointToString(
                currentMark
            );

            // Loop through the available attributes for the new WaymarkPreset
            foreach (var attribute in waymarkPresetAttributes)
            {
                // Set up the attribute that will be changed in the new WaymarkPreset
                var changingAtt = newMark.GetType().GetProperty(attribute)!;

                // Short circuit assign active attribute
                if (attribute == "Active")
                {
                    var active = testIfModifiedBlank != blank
                                 || testIfCurrentBlank != blank;

                    changingAtt.SetValue(
                        newMark,
                        active
                    );

                    continue;
                }

                // Fill with current waymark position float
                var fillingAtt = (int) currentMark.GetType().GetField(attribute)!
                                     .GetValue(currentMark)!
                                 / 1000.0f;

                // Instead fill with the modified position float if it is not blank
                if (testIfModifiedBlank != blank)
                {
                    // Get the equivalent attribute from the swapped GamePresetPoint
                    fillingAtt = (int) modifiedMark.GetType().GetField(attribute)!
                                     .GetValue(modifiedMark)!
                                 / 1000.0f;
                }

                changingAtt.SetValue(
                    newMark,
                    fillingAtt
                );
            }
        }

        return this.getWaymarkPresetAsGamePreset(newWaymarksPreset);
    }

    # endregion

    public void Dispose()
    {
        this._disconnect();
        GC.SuppressFinalize(this);
    }
}
