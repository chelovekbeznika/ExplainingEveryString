﻿using System;

namespace ExplainingEveryString.Data.Level
{
    public static class LevelDataAccess
    {
        public static ILevelLoader GetLevelLoader()
        {
            return new JsonLevelLoader();
        }
    }

    public interface ILevelLoader
    {
        LevelData Load(String fileName);
        void Save(String fileName, LevelData updatedLevel);
    }

    internal class JsonLevelLoader : ILevelLoader
    {
        public LevelData Load(String fileName)
        {
            var result = JsonDataAccessor.Instance.Load<LevelData>(FileNames.GetJsonLevelsPath(fileName));
            return result;
        }

        public void Save(String fileName, LevelData updatedLevel)
        {
            JsonDataAccessor.Instance.Save(FileNames.GetJsonLevelsPath(fileName), updatedLevel);
        }
    }
}
