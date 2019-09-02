using System;
using System.IO;

namespace ExplainingEveryString.Data
{
    internal static class FileNames
    {
        internal static String Configuration => GetJsonDataPath("config.dat");
        internal static String Blueprints => GetJsonDataPath("blueprints.dat");
        internal static String AssetsMetadata => GetJsonDataPath("assets_metadata.dat");
        internal static String LevelSequence => GetJsonDataPath("level_sequence.dat");
        internal static String GameProgress => GetJsonDataPath("game_progress.dat");

        internal static String GetJsonDataPath(String fileName)
        {
            return Path.Combine("Content", "Data", fileName);
        }
    }
}
