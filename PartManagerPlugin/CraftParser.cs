using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PartManagerPlugin
{
    /// <summary>
    /// Parses KSP .craft files to extract part names used in the vessel.
    /// </summary>
    public static class CraftParser
    {
        // Regex to extract the part name from "part = someName_12345" lines inside PART blocks
        private static readonly Regex PartRegex = new Regex(
            @"^\s*part\s*=\s*(\S+)",
            RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Parses a .craft file and returns the list of part names used.
        /// Strips trailing serial numbers (e.g., "mark1Cockpit_12345" -> "mark1Cockpit").
        /// </summary>
        public static List<string> GetPartsFromCraftFile(string filePath)
        {
            var parts = new List<string>();
            try
            {
                var text = File.ReadAllText(filePath);
                var matches = PartRegex.Matches(text);
                foreach (Match match in matches)
                {
                    if (match.Groups[1].Success)
                    {
                        var partName = StripSerial(match.Groups[1].Value);
                        if (!string.IsNullOrEmpty(partName) && !parts.Contains(partName))
                        {
                            parts.Add(partName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing craft file {filePath}: {ex.Message}");
            }
            return parts;
        }

        /// <summary>
        /// Strips the trailing serial number suffix from a KSP part name.
        /// Part names in .craft files look like "partName_12345".
        /// Returns just "partName".
        /// </summary>
        public static string StripSerial(string partName)
        {
            if (string.IsNullOrEmpty(partName))
                return partName;

            // Serial number is _digits at the end
            var lastUnderscore = partName.LastIndexOf('_');
            if (lastUnderscore > 0)
            {
                var suffix = partName.Substring(lastUnderscore + 1);
                if (suffix.Length > 0 && IsDigitsOnly(suffix))
                {
                    return partName.Substring(0, lastUnderscore);
                }
            }
            return partName;
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return str.Length > 0;
        }

        /// <summary>
        /// Scans all .craft files in a directory and returns part names grouped by craft file.
        /// </summary>
        public static Dictionary<string, List<string>> GetPartsFromCraftFolder(string folderPath)
        {
            var result = new Dictionary<string, List<string>>();
            if (!Directory.Exists(folderPath))
                return result;

            foreach (var craftFile in Directory.GetFiles(folderPath, "*.craft", SearchOption.AllDirectories))
            {
                var parts = GetPartsFromCraftFile(craftFile);
                if (parts.Count > 0)
                {
                    result[Path.GetFileNameWithoutExtension(craftFile)] = parts;
                }
            }
            return result;
        }
    }
}
