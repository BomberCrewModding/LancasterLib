using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using BestHTTP.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lancaster.Utils;

internal static class InternalLogger
{

    internal static bool Initialized = false;
    private static ManualLogSource Logger { get; set; }
    private static FieldInfo ConfigConsoleDisplayedLevel { get; } = typeof(ConsoleLogListener).GetField("ConfigConsoleDisplayedLevel", BindingFlags.Static | BindingFlags.NonPublic);
    private static ConfigEntry<LogLevel> ConsoleLogLevel { get; } = ConfigConsoleDisplayedLevel.GetValue(null) as ConfigEntry<LogLevel>;
    private static FieldInfo ConfigDiskConsoleDisplayedLevel { get; } = typeof(Chainloader).GetField("ConfigDiskConsoleDisplayedLevel", BindingFlags.Static | BindingFlags.NonPublic);
    private static ConfigEntry<LogLevel> diskLogLevel { get; } = ConfigDiskConsoleDisplayedLevel.GetValue(null) as ConfigEntry<LogLevel>;
    internal static DiskLogListener DiskLogListener { get; private set; }
    internal static bool EnableDebugging => (DiskLogListener != null && (DiskLogListener.DisplayedLogLevel & LogLevel.Debug) != LogLevel.None) || (ConsoleLogLevel != null && (ConsoleLogLevel.Value & LogLevel.Debug) != LogLevel.None);

    internal static void SetDebugging(bool enabled)
    {
        if(enabled)
        {
            if (ConsoleLogLevel != null && (ConsoleLogLevel.Value & LogLevel.Debug) == LogLevel.None)
            {
                ConsoleLogLevel.Value = ConsoleLogLevel.Value | LogLevel.Debug;
            }

            if (diskLogLevel != null && (DiskLogListener.DisplayedLogLevel & LogLevel.Debug) == LogLevel.None)
            {
                diskLogLevel.Value = diskLogLevel.Value | LogLevel.Debug;
            }

            if (DiskLogListener != null && (DiskLogListener.DisplayedLogLevel & LogLevel.Debug) == LogLevel.None)
            {
                DiskLogListener.DisplayedLogLevel = DiskLogListener.DisplayedLogLevel | LogLevel.Debug;
            }
        }
        else
        {
            if (ConsoleLogLevel != null && (ConsoleLogLevel.Value & LogLevel.Debug) != LogLevel.None)
            {
                ConsoleLogLevel.Value = ConsoleLogLevel.Value & ~LogLevel.Debug;
            }

            if (diskLogLevel != null && (DiskLogListener.DisplayedLogLevel & LogLevel.Debug) != LogLevel.None)
            {
                diskLogLevel.Value = diskLogLevel.Value & ~LogLevel.Debug;
            }

            if (DiskLogListener != null && (DiskLogListener.DisplayedLogLevel & LogLevel.Debug) != LogLevel.None)
            {
                DiskLogListener.DisplayedLogLevel = DiskLogListener.DisplayedLogLevel & ~LogLevel.Debug;
            }
        }
    }

    internal static void Initialize(ManualLogSource logger)
    {
        if (Initialized)
            return;

        if (BepInEx.Logging.Logger.Listeners.Where((x) => x is DiskLogListener).First((x) => x is DiskLogListener) is DiskLogListener logListener)
            DiskLogListener = logListener;

        Logger = logger;
        Log($"Enable debug logs set to: {EnableDebugging}", LogLevel.Info);

        Initialized = true;
    }

    internal static void Log(string text, LogLevel logLevel = LogLevel.Info)
    {
        if(!Initialized)
        {
            if(logLevel >= LogLevel.Info || EnableDebugging)
            {
                Console.WriteLine($"[Lancaster/{logLevel}] {text}");
            }

            return;
        }
        Logger.Log(logLevel, text);
    }

    internal static void Log(string text, LogLevel logLevel = LogLevel.Info, params object[] args)
    {
        if (args is not null && args.Length > 0)
            text = string.Format(text, args);

        Log(text, logLevel);
    }

}
