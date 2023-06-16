using System;
using System.IO;

namespace ExplainingEveryString.Data
{
    internal static class FileNames
    {
        internal static String Configuration => GetJsonDataPath("config");
        internal static String AssetsMetadata => GetJsonDataPath("assets_metadata");
        internal static String LevelSequence => GetJsonDataPath("level_sequence");
        internal static String GameProgress(int profileNumber) => GetJsonDataPath($"game_progress_{profileNumber}");
        internal static String MusicTestMenu => GetJsonDataPath("music_menu");
        internal static String Notifications => GetJsonDataPath("notifications");
        internal static String CutscenesMetadata => GetJsonDataPath("cutscenes_metadata");

        private static String GetJsonDataPath(String fileName) => Path.Combine("Content", "Data", $"{fileName}.dat");

        internal static String GetJsonLevelsPath(String fileName) => Path.Combine("Content", "Data", "Levels", $"{fileName}.dat");
        internal static String GetJsonBlueprintsPath(String fileName) => Path.Combine("Content", "Data", "Blueprints", $"{fileName}.dat");
    }
}
