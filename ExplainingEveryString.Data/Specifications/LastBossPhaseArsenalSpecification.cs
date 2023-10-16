using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class LastBossPhaseWeaponSpecification
    {
        public WeaponSpecification Weapon { get; set; }
        [DefaultValue(0)]
        public Single Angle { get; set; }
    }
}
