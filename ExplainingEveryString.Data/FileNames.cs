using System;
using System.IO;

namespace ExplainingEveryString.Data
{
    internal static class FileNames
    {
        internal static String Configuration => GetJsonDataPath("config");
        internal static String AssetsMetadata => GetJsonDataPath("assets_metadata");
        internal static String LevelSequence => GetJsonDataPath("level_sequence");
        internal static String GameProgress => GetJsonDataPath("game_progress");
        internal static String MusicTestMenu => GetJsonDataPath("music_menu");

        internal static String GetJsonDataPath(String fileName) => Path.Combine("Content", "Data", $"{fileName}.dat");
        internal static String GetJsonBlueprintsPath(String fileName) => Path.Combine("Content", "Data", "Blueprints", $"{fileName}.dat");
    }
}
