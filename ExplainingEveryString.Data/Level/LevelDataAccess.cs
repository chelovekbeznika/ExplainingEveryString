using System;

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
    }

    internal class JsonLevelLoader : ILevelLoader
    {
        public LevelData Load(String fileName)
        {
            return JsonDataAccessor.Instance.Load<LevelData>(FileNames.GetJsonDataPath(fileName));
        }
    }
}
