using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;
using Lancaster.Patchers;
using Lancaster.Utils;

namespace Lancaster;

/// <summary>
/// Initializer of the Lancaster Lib.
/// </summary>
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Initializer : BaseUnityPlugin
{
    private static readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);

    /// <summary>
    /// WARNING: This method is for use only by BepInEx.
    /// </summary>
    [Obsolete("This method is for use only by BepInEx.", true)]
    Initializer()
    {
        InternalLogger.Log($"Loading v{PluginInfo.PLUGIN_VERSION}", BepInEx.Logging.LogLevel.Info);

        EnumPatcher.Patch(_harmony);
    }
}
