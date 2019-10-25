using System;

namespace ExplainingEveryString.Data.Level
{
    public static class LevelSequenceAccess
    {
        public static LevelSequnceSpecification LoadLevelSequence()
        {
            return JsonDataAccessor.Instance.Load<LevelSequnceSpecification>(FileNames.LevelSequence);
        }
    }
}
