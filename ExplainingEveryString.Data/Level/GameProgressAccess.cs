using System.IO;

namespace ExplainingEveryString.Data.Level
{
    public static class GameProgressAccess
    {
        public static GameProgress Load()
        {
            if (File.Exists(FileNames.GameProgress))
            {
                return JsonDataAccessor.Instance.Load<GameProgress>(FileNames.GameProgress);
            }
            else
                return null;
        }

        public static void Save(GameProgress gameProgress)
        {
            JsonDataAccessor.Instance.Save<GameProgress>(FileNames.GameProgress, gameProgress);
        }
    }
}
