using System;
using System.IO;
using CKAN;
using CKAN.GUI;

namespace PartManagerPlugin
{
    public static class Cache
    {

        private static string GetCachePath()
        {
            var ckanDir = Main.Instance?.CurrentInstance?.CkanDir;
            if (ckanDir == null) return null;

            var cachePath = Path.Combine(ckanDir, "PartManager", "cache");
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }
            return cachePath;
        }

        private static string GetGameDir()
        {
            return Main.Instance?.CurrentInstance?.GameDir;
        }

        public static void RemovePartFromCache(string part)
        {
            var cachePath = GetCachePath();
            if (cachePath == null) return;

            var fullPath = Path.Combine(cachePath, part);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public static void MovePartToCache(string part)
        {
            var gameDir = GetGameDir();
            var cachePath = GetCachePath();
            if (gameDir == null || cachePath == null) return;

            var fullPath = Path.Combine(gameDir, part);
            if (!File.Exists(fullPath)) return;

            var targetPath = Path.Combine(cachePath, part);

            try
            {
                var targetDir = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrEmpty(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
            }
            catch (Exception) { }

            File.Move(fullPath, targetPath);
        }

        public static void MovePartFromCache(string part)
        {
            var gameDir = GetGameDir();
            var cachePath = GetCachePath();
            if (gameDir == null || cachePath == null) return;

            var fullPath = Path.Combine(cachePath, part);
            if (!File.Exists(fullPath)) return;

            var targetPath = Path.Combine(gameDir, part);

            try
            {
                var targetDir = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrEmpty(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
            }
            catch (Exception) { }

            File.Move(fullPath, targetPath);
        }

    }
}
