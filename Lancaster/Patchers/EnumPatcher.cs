using BepInEx.Logging;
using HarmonyLib;
using Lancaster.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lancaster.Patchers;

internal static class EnumPatcher
{

    internal static void Patch(Harmony harmony)
    {
        harmony.PatchAll(typeof(EnumPatcher));

        InternalLogger.Log("Enum Patcher is done.", LogLevel.Debug);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Enum), nameof(Enum.GetValues))]
    private static void GetValues(Type enumType, ref Array __result)
    {

    }

}
