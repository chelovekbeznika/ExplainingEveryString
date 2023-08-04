using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class GameProgress
    {
        public LevelProgress LevelProgress { get; set; }
        public String CurrentLevelFileName { get; set; }
        public String MaxAchievedLevelName { get; set; }
        public Dictionary<String, Single> LevelRecords { get; set; }
        [DefaultValue(null)]
        public Single? PersonalBest { get; set; }
        [DefaultValue(null)]
        public Dictionary<String, Single> PersonalBestSplits { get; set; }
    }
}
