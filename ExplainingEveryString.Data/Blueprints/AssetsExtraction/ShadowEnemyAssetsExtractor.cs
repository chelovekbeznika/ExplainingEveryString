using System.Collections.Generic;
using System.Linq;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class ShadowEnemyAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<ShadowEnemyBlueprint>
    {
        public IEnumerable<SpriteSpecification> GetSprites(ShadowEnemyBlueprint blueprint)
        {
            return base.GetSprites(blueprint).Append(blueprint.ShadowSprite);
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(ShadowEnemyBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint).Append(blueprint.PhaseChangeEffect);
        }
    }
}
