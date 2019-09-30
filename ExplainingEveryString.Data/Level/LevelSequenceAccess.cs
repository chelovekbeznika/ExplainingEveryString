using System;

namespace ExplainingEveryString.Data.Level
{
    public static class LevelSequenceAccess
    {
        public static String[] LoadLevelSequence()
        {
            return JsonDataAccessor.Instance.Load<String[]>(FileNames.LevelSequence);
        }
    }
}
