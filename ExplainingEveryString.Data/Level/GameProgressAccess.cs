using System;
using System.IO;

namespace ExplainingEveryString.Data.Level
{
    public static class GameProgressAccess
    {
        public static GameProgress Load(Int32 profileNumber)
        {
            if (File.Exists(FileNames.GameProgress(profileNumber)))
            {
                return JsonDataAccessor.Instance.Load<GameProgress>(FileNames.GameProgress(profileNumber));
            }
            else
                return null;
        }

        public static void Save(GameProgress gameProgress, Int32 profileNumber)
        {
            JsonDataAccessor.Instance.Save(FileNames.GameProgress(profileNumber), gameProgress);
        }
    }
}
