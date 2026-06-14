using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PartManagerPlugin
{
    /// <summary>
    /// Scans KSP GameData and craft files to find missing parts.
    /// </summary>
    public static class PartScanner
    {
        // Cache of all known part names from GameData (built once per scan)
        private static HashSet<string> s_KnownParts = null;
        private static string s_CachedGameDir = null;

        /// <summary>
        /// Regex to find "name = partName" lines in .cfg files.
        /// </summary>
        private static readonly Regex NameRegex = new Regex(
            @"^\s*name\s*=\s*(\S+)",
            RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Builds a cache of all part names defined in GameData .cfg files.
        /// This is called once per scan instead of re-reading every file for each part.
        /// </summary>
        public static HashSet<string> BuildKnownPartsCache(string gameDir)
        {
            if (string.IsNullOrEmpty(gameDir))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Return cached version if same game dir
            if (s_KnownParts != null && s_CachedGameDir == gameDir)
                return s_KnownParts;

            var knownParts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var gameDataDir = Path.Combine(gameDir, "GameData");

            if (Directory.Exists(gameDataDir))
            {
                try
                {
                    foreach (var cfgFile in Directory.GetFiles(gameDataDir, "*.cfg", SearchOption.AllDirectories))
                    {
                        try
                        {
                            var content = File.ReadAllText(cfgFile);
                            var matches = NameRegex.Matches(content);
                            foreach (Match match in matches)
                            {
                                if (match.Groups[1].Success)
                                {
                                    knownParts.Add(match.Groups[1].Value);
                                }
                            }
                        }
                        catch
                        {
                            // Skip unreadable files
                        }
                    }
                }
                catch
                {
                    // Skip inaccessible directories
                }
            }

            s_KnownParts = knownParts;
            s_CachedGameDir = gameDir;
            return knownParts;
        }

        /// <summary>
        /// Clears the cached known parts list (e.g. when game instance changes).
        /// </summary>
        public static void ClearCache()
        {
            s_KnownParts = null;
            s_CachedGameDir = null;
        }

        /// <summary>
        /// Checks if a part exists in GameData using the cached known parts set.
        /// </summary>
        public static bool PartExistsInGameData(string gameDir, string partName)
        {
            if (string.IsNullOrEmpty(gameDir) || string.IsNullOrEmpty(partName))
                return false;

            var knownParts = BuildKnownPartsCache(gameDir);
            return knownParts.Contains(partName);
        }

        /// <summary>
        /// Finds which parts from craft files are missing from the KSP installation.
        /// Uses the cached part name list for fast lookup.
        /// </summary>
        public static Dictionary<string, List<string>> FindMissingParts(
            string gameDir,
            Dictionary<string, List<string>> craftParts)
        {
            var result = new Dictionary<string, List<string>>();

            // Build the known-parts cache once (reads GameData one time)
            var knownParts = BuildKnownPartsCache(gameDir);

            foreach (var kvp in craftParts)
            {
                var missing = new List<string>();
                foreach (var part in kvp.Value)
                {
                    if (!knownParts.Contains(part))
                    {
                        missing.Add(part);
                    }
                }
                if (missing.Count > 0)
                {
                    result[kvp.Key] = missing;
                }
            }

            return result;
        }

        /// <summary>
        /// Finds missing parts for a single craft file.
        /// </summary>
        public static List<string> FindMissingPartsForCraft(
            string gameDir,
            string craftName,
            List<string> craftParts)
        {
            var knownParts = BuildKnownPartsCache(gameDir);
            var missing = new List<string>();

            foreach (var part in craftParts)
            {
                if (!knownParts.Contains(part))
                {
                    missing.Add(part);
                }
            }

            return missing;
        }

        /// <summary>
        /// Scans both VAB and SPH ship directories and returns all parts used across all craft files.
        /// Returns a dict of craft file name (without extension) -> list of part names.
        /// Also returns the full file paths via an out parameter.
        /// </summary>
        public static Dictionary<string, List<string>> ScanAllCraftFiles(
            string gameDir,
            out Dictionary<string, string> craftFilePaths)
        {
            var allCraftParts = new Dictionary<string, List<string>>();
            craftFilePaths = new Dictionary<string, string>();

            var shipsDir = Path.Combine(gameDir, "ships");
            if (!Directory.Exists(shipsDir))
                return allCraftParts;

            // Scan VAB and SPH subfolders
            foreach (var subDir in new[] { "VAB", "SPH", "SpacePlaneHangar", "VehicleAssemblyBuilding" })
            {
                var fullPath = Path.Combine(shipsDir, subDir);
                if (Directory.Exists(fullPath))
                {
                    var parts = CraftParser.GetPartsFromCraftFolder(fullPath);
                    foreach (var kvp in parts)
                    {
                        allCraftParts[kvp.Key] = kvp.Value;
                    }

                    // Collect file paths
                    foreach (var craftFile in Directory.GetFiles(fullPath, "*.craft", SearchOption.AllDirectories))
                    {
                        var name = Path.GetFileNameWithoutExtension(craftFile);
                        if (!craftFilePaths.ContainsKey(name))
                        {
                            craftFilePaths[name] = craftFile;
                        }
                    }
                }
            }

            return allCraftParts;
        }

        /// <summary>
        /// Scans all craft files (overload for backward compatibility).
        /// </summary>
        public static Dictionary<string, List<string>> ScanAllCraftFiles(string gameDir)
        {
            Dictionary<string, string> paths;
            return ScanAllCraftFiles(gameDir, out paths);
        }
    }
}
