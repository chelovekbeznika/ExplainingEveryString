using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs
{
    internal class ActorAssetsExtractor : IAssetsExtractor<Blueprint>
    {
        public IEnumerable<SpriteSpecification> GetSprites(Blueprint blueprint)
        {
            return new SpriteSpecification[] { blueprint.DefaultSprite };
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(Blueprint blueprint)
        {
            return Enumerable.Empty<SpecEffectSpecification>();
        }

        public IEnumerable<SpriteSpecification> GetSpritesFromWeapon(WeaponSpecification specification)
        {
            return new SpriteSpecification[] { specification.Sprite }
                .Concat(specification.Barrels.Select(b => b.Bullet.Sprite));
        }
    }
}
