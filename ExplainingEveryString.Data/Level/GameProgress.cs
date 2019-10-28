using System;

namespace ExplainingEveryString.Data.Level
{
    public class GameProgress
    {
        public LevelProgress LevelProgress { get; set; }
        public String CurrentLevelFileName { get; set; }
        public String MaxAchievedLevelName { get; set; }
    }
}
