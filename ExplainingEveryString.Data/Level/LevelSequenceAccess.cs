namespace ExplainingEveryString.Data.Level
{
    public static class LevelSequenceAccess
    {
        public static LevelSequenceSpecification LoadLevelSequence()
        {
            return JsonDataAccessor.Instance.Load<LevelSequenceSpecification>(FileNames.LevelSequence);
        }
    }
}
