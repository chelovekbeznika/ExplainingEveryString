using ExplainingEveryString.Data.Specifications;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs
{
    internal interface IAssetsExtractor<TBlueprint> where TBlueprint : Blueprint
    {
        IEnumerable<SpriteSpecification> GetSprites(TBlueprint blueprint);
        IEnumerable<SpecEffectSpecification> GetSpecEffects(TBlueprint blueprint);
    }
}
