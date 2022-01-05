using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

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
            return base.GetSprites(blueprint);
        }
    }
}
