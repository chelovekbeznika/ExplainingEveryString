using System.Collections.Generic;
using System.Linq;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs
{
    internal class DoorAssetsExtractor : ActorAssetsExtractor, IAssetsExtractor<DoorBlueprint>
    {
        public IEnumerable<SpriteSpecification> GetSprites(DoorBlueprint blueprint)
        {
            return base.GetSprites(blueprint).Concat(new SpriteSpecification[] { blueprint.OpeningSprite });
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(DoorBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint)
                .Concat(new SpecEffectSpecification[] { blueprint.OpeningStartedEffect, blueprint.CompletelyOpenedEffect });
        }
    }
}
