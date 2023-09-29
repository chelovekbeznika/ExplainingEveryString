using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class ReloaderSpecification
    {
        public Single FireRate { get; set; }
        [DefaultValue(1)]
        public Int32 Ammo { get; set; }
        [DefaultValue(0.0)]
        public Single ReloadTime { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification ReloadStartedEffect { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification ReloadFinishedEffect { get; set; }
    }
}
