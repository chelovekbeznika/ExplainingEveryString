using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class PlayerBlueprint : Blueprint
    {
        public Single BulletHitboxWidth { get; set; }
        public Single MaxSpeed { get; set; }
        public Single MaxAcceleration { get; set; }
        public WeaponSpecification Weapon { get; set; }
        public SpecEffectSpecification DamageEffect { get; set; }

        internal override IEnumerable<SpriteSpecification> GetSprites()
        {
            return base.GetSprites().Concat(Weapon.GetSprites());
        }
        internal override IEnumerable<SpecEffectSpecification> GetSpecEffects()
        {
            return new SpecEffectSpecification[] { Weapon.ShootingEffect, DamageEffect };
        }
    }
}
