﻿using ExplainingEveryString.Data.RandomVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Data.Specifications
{
    public class FourthBossMovementSpecification
    {
        public Single Radius { get; set; }
        public Single CircleTime { get; set; }
        public GaussRandomVariable BetweenDirectionSwitches { get; set; }
    }
}
