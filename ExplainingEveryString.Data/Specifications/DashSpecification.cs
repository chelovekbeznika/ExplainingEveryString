﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class DashSpecification
    {
        public Single MaxSpeedIncrease { get; set; }
        public Single SpeedIncrease { get; set; }
        public Single AccelerationIncrease { get; set; }
        public Single RechargeTime { get; set; }
        public Single Duration { get; set; }
        public SpecEffectSpecification SpecEffect { get; set; }
        public SpriteSpecification Sprite { get; set; }
    }
}
