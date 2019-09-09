using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs
{
    internal interface IAssetsExtractor<TBlueprint> where TBlueprint : Blueprint
    {
        IEnumerable<SpriteSpecification> GetSprites(TBlueprint blueprint);
        IEnumerable<SpecEffectSpecification> GetSpecEffects(TBlueprint blueprint);
    }
}
