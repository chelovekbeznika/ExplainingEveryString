using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class FifthBossAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<FifthBossBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(FifthBossBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint)
                .Concat(GetSpecEffectsFromWeapon(blueprint.LeftWeapon))
                .Concat(GetSpecEffectsFromWeapon(blueprint.RightWeapon));
        }

        public IEnumerable<SpriteSpecification> GetSprites(FifthBossBlueprint blueprint)
        {
            return base.GetSprites(blueprint).Concat(new[] { blueprint.EyeSprite });
        }
    }
}
