using System;

namespace ExplainingEveryString.Data.Specifications
{
    public class DashSpecification
    {
        public Single AvailabityThreshold { get; set; }
        public Single MaxSpeedIncrease { get; set; }
        public Single SpeedIncrease { get; set; }
        public Single AccelerationIncrease { get; set; }
        public Single RechargeTime { get; set; }
        public Single Duration { get; set; }
        public String[] CollideTagsDefense { get; set; }
        public SpecEffectSpecification ActiveSpecEffect { get; set; }
        public SpecEffectSpecification ReadySpecEffect { get; set; }
        public SpriteSpecification Sprite { get; set; }
    }
}
