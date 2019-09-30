using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public class PlayerBlueprint : Blueprint
    {
        public Single BulletHitboxWidth { get; set; }
        public Single MaxSpeed { get; set; }
        public Single MaxAcceleration { get; set; }
        public WeaponSpecification Weapon { get; set; }
        public DashSpecification Dash { get; set; }
        public SpecEffectSpecification DamageEffect { get; set; }
        public SpecEffectSpecification BaseDestructionEffect { get; set; }
        public SpecEffectSpecification CannonDestructionEffect { get; set; }
    }
}
