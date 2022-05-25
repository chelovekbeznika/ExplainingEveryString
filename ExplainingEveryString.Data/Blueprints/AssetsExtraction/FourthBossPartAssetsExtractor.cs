using ExplainingEveryString.Data.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class FourthBossPartAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<FourthBossPartBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(FourthBossPartBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint);
        }

        public IEnumerable<SpriteSpecification> GetSprites(FourthBossPartBlueprint blueprint)
        {
            return base.GetSprites(blueprint).Concat(new[]
            {
                blueprint.PhaseSwitchSprite,
                blueprint.SecondPhaseSprite
            });
        }
    }
}
