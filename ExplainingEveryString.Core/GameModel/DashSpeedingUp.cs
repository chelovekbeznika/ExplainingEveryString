using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class DashAcceleration
    {
        internal Single AvailabilityThreshold { get;  }
        internal Single MaxSpeedIncrease { get; }
        internal Single SpeedIncrease { get; }
        internal Single AccelerationIncrease { get; }

        internal DashAcceleration(DashSpecification dashSpecification)
        {
            AvailabilityThreshold = dashSpecification.AvailabityThreshold;
            MaxSpeedIncrease = dashSpecification.MaxSpeedIncrease;
            SpeedIncrease = dashSpecification.SpeedIncrease;
            AccelerationIncrease = dashSpecification.AccelerationIncrease;
        }
    }
}
