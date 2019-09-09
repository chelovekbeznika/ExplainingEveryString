using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs
{
    internal class ShadowEnemyAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<ShadowEnemyBlueprint>
    {
        public IEnumerable<SpriteSpecification> GetSprites(ShadowEnemyBlueprint blueprint)
        {
            return base.GetSprites(blueprint).Concat(new SpriteSpecification[] { blueprint.ShadowSprite });
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(ShadowEnemyBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint).Concat(new SpecEffectSpecification[] { blueprint.PhaseChangeEffect });
        }
    }
}
