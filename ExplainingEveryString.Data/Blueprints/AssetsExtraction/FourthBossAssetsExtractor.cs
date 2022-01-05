using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class FourthBossAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<FourthBossBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(FourthBossBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint);
        }

        public IEnumerable<SpriteSpecification> GetSprites(FourthBossBlueprint blueprint)
        {
            return base.GetSprites(blueprint);
        }
    }
}
