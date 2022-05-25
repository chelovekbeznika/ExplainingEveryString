using ExplainingEveryString.Data.RandomVariables;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Specifications
{
    public class FourthBossMovementSpecification
    {
        public Single Radius { get; set; }
        public Single RadiusPulsationPart { get; set; }
        public Single CircleTime { get; set; }
        public GaussRandomVariable BetweenDirectionSwitches { get; set; }
        public Dictionary<String, Single> TimeBetweenPulses { get; set; }
    }
}
