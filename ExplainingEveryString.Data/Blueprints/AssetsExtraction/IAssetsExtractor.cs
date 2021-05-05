using ExplainingEveryString.Data.Specifications;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal interface IAssetsExtractor<TBlueprint> where TBlueprint : Blueprint
    {
        IEnumerable<SpriteSpecification> GetSprites(TBlueprint blueprint);
        IEnumerable<SpecEffectSpecification> GetSpecEffects(TBlueprint blueprint);
    }
}
