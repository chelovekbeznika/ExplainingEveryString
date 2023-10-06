using ExplainingEveryString.Data.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class SecondBossAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<SecondBossBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(SecondBossBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint);
        }

        public IEnumerable<SpriteSpecification> GetSprites(SecondBossBlueprint blueprint)
        {
            return base.GetSprites(blueprint)
                .Concat(blueprint.Phases.Select(phase => phase.Sprite))
                .Concat(new[] { blueprint.DeathZoneBorderSprite });
        }
    }
}
