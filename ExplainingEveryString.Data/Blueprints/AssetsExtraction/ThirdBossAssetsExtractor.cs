using ExplainingEveryString.Data.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class ThirdBossAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<ThirdBossBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(ThirdBossBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint).Concat(GetSpecEffectsFromWeapon(blueprint.SmallWeapon));
        }

        public IEnumerable<SpriteSpecification> GetSprites(ThirdBossBlueprint blueprint)
        {
            return base.GetSprites(blueprint).Concat(GetSpritesFromWeapon(blueprint.SmallWeapon));
        }
    }
}
