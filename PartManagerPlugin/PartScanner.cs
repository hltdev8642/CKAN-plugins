using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PartManagerPlugin
{
    /// <summary>
    /// Scans KSP GameData and craft files to find missing parts.
    /// </summary>
    public static class PartScanner
    {
        /// <summary>
        /// Scans the GameData directory for .cfg files that define a part with the given name.
        /// Looks for "name = partName" inside PART nodes.
        /// </summary>
        public static bool PartExistsInGameData(string gameDir, string partName)
        {
            if (string.IsNullOrEmpty(gameDir) || string.IsNullOrEmpty(partName))
                return false;

            var gameDataDir = Path.Combine(gameDir, "GameData");
            if (!Directory.Exists(gameDataDir))
                return false;

            try
            {
                // Search recursively for .cfg files that might define this part
                foreach (var cfgFile in Directory.GetFiles(gameDataDir, "*.cfg", SearchOption.AllDirectories))
                {
                    try
                    {
                        var content = File.ReadAllText(cfgFile);
                        // Look for "name = partName" in the file (case-insensitive)
                        if (content.IndexOf($"name = {partName}", StringComparison.OrdinalIgnoreCase) >= 0
                            || content.Contains($"name = {partName}_"))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        // Skip files we can't read
                    }
                }
            }
            catch
            {
                // Skip directories we can't access
            }

            return false;
        }

        /// <summary>
        /// Finds which parts from craft files are missing from the KSP installation.
        /// Returns a dict of craft name -> list of missing part names.
        /// </summary>
        public static Dictionary<string, List<string>> FindMissingParts(
            string gameDir,
            Dictionary<string, List<string>> craftParts)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var kvp in craftParts)
            {
                var missing = new List<string>();
                foreach (var part in kvp.Value)
                {
                    if (!PartExistsInGameData(gameDir, part))
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
        /// Scans both VAB and SPH ship directories and returns all parts used across all craft files.
        /// </summary>
        public static Dictionary<string, List<string>> ScanAllCraftFiles(string gameDir)
        {
            var allCraftParts = new Dictionary<string, List<string>>();

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
                }
            }

            return allCraftParts;
        }
    }
}
