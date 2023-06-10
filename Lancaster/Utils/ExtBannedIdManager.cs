using BepInEx.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lancaster.Utils;

internal static class ExtBannedIdManager
{
    private static bool IsInitialized = false;

    private static readonly Dictionary<string, List<int>> BannedIdDictionary = new();

    private static readonly string BannedIdDirectory = Path.Combine(Path.Combine(BepInEx.Paths.ConfigPath, Assembly.GetExecutingAssembly().GetName().Name), "RestrictedIDs");

    internal static IEnumerable<int> GetBannedIdsFor(string enumName, params IList<int>[] combineWith)
    {
        if (!IsInitialized)
        {
            LoadFromFiles();
        }

        if (!BannedIdDictionary.ContainsKey(enumName))
        {
            BannedIdDictionary.Add(enumName, new List<int>());
        }

        foreach (IList<int> otherBannedIds in combineWith)
        {
            BannedIdDictionary[enumName].AddRange(otherBannedIds);
        }

        return GetBannedIdsFor(enumName);
    }

    internal static IEnumerable<int> GetBannedIdsFor(string enumName)
    {
        if (!IsInitialized)
        {
            LoadFromFiles();
        }

        if (!BannedIdDictionary.ContainsKey(enumName))
        {
            return new int[0];
        }

        return BannedIdDictionary[enumName].ToArray();
    }

    private static void LoadFromFiles()
    {
        if (!Directory.Exists(BannedIdDirectory))
        {
            CreateBannedIdDirectory();
            IsInitialized = true;
            return;
        }

        string[] files = Directory.GetFiles(BannedIdDirectory);

        foreach (string filePath in files
        {
            string[] entries = File.ReadAllLines(filePath);

            foreach (string line in entries)
            {

                string[] components = line.Split(':');

                int id = -1;

                if (components.Length != 2 ||
                    !int.TryParse(components[0].Trim(), out id))
                {
                    LogBadEntry(filePath, line);
                    continue;
                }

                string key = components[1].Trim();

                if (string.IsNullOrEmpty(key) ||
                    id < 0)
                {
                    LogBadEntry(filePath, line);
                    continue;
                }

                if (!BannedIdDictionary.ContainsKey(key))
                {
                    BannedIdDictionary.Add(key, new List<int>());
                }

                BannedIdDictionary[key].Add(id);
            }
        }

        foreach (string bannedIdType in BannedIdDictionary.Keys)
        {
            InternalLogger.Log($"{BannedIdDictionary[bannedIdType].Count} retricted IDs were registered for {bannedIdType}.", LogLevel.Info);
        }

        IsInitialized = true;
    }

    private static void CreateBannedIdDirectory()
    {
        try
        {
            Directory.CreateDirectory(BannedIdDirectory);
            InternalLogger.Log("RetrictedIDs folder was not found. Folder created.", LogLevel.Debug);
        }
        catch (Exception ex)
        {
            InternalLogger.Log($"RetrictedIDs folder was not found. Failed to create folder.{Environment.NewLine}" +
                               $"        Exception: {ex}", LogLevel.Error);

        }
    }

    private static void LogBadEntry(string filePath, string line)
    {
        InternalLogger.Log($"Badly formatted entry for Retricted IDs{Environment.NewLine}" +
                           $"        File: '{filePath}{Environment.NewLine}'" +
                           $"        Line: '{line}'{Environment.NewLine}" +
                           $"        This entry has been skipped.", LogLevel.Warning);
    }

}