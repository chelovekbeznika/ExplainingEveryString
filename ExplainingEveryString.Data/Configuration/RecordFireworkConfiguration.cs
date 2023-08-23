using ExplainingEveryString.Data.RandomVariables;
using System;

namespace ExplainingEveryString.Data.Configuration
{
    public class RecordFireworkConfiguration
    {
        public GaussRandomVariable LifeTime { get; set; }
        public GaussRandomVariable BetweenSpawns { get; set; }
        public Single Volume { get; set; }
        public GaussRandomVariable SoundCooldown { get; set; }
    }
}
