using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class FixedCannonBlueprint : EnemyBlueprint
    {
        public WeaponSpecification Weapon { get; set; }

        internal override IEnumerable<SpecEffectSpecification> GetSpecEffects()
        {
            return base.GetSpecEffects().Concat(new SpecEffectSpecification[] { Weapon.ShootingEffect });
        }

        internal override IEnumerable<SpriteSpecification> GetSprites()
        {
            return base.GetSprites().Concat(Weapon.GetSprites());
        }
    }
}
