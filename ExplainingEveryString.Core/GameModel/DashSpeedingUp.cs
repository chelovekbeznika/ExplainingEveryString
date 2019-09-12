﻿using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class DashAcceleration
    {
        internal Single MaxSpeedIncrease { get; }
        internal Single SpeedIncrease { get; }
        internal Single AccelerationIncrease { get; }

        internal DashAcceleration(DashSpecification dashSpecification)
        {
            MaxSpeedIncrease = dashSpecification.MaxSpeedIncrease;
            SpeedIncrease = dashSpecification.SpeedIncrease;
            AccelerationIncrease = dashSpecification.AccelerationIncrease;
        }
    }
}
