using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class ChangeableEnemyAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<ChangeableEnemyBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(ChangeableEnemyBlueprint blueprint)
        {
            var specEffects = base.GetSpecEffects(blueprint);
            if (blueprint.Modifications != null)
                foreach (var modification in blueprint.Modifications.SelectMany(keyValuePair => keyValuePair.Value))
                {
                    if (modification is WeaponSpecification)
                        specEffects = specEffects.Concat(GetSpecEffectsFromWeapon(modification as WeaponSpecification));
                }
            return specEffects;
        }

        public IEnumerable<SpriteSpecification> GetSprites(ChangeableEnemyBlueprint blueprint)
        {
            var sprites = base.GetSprites(blueprint);
            if (blueprint.Modifications != null)
                foreach (var modification in blueprint.Modifications?.SelectMany(keyValuePair => keyValuePair.Value))
                {
                    if (modification is WeaponSpecification)
                        sprites = sprites.Concat(GetSpritesFromWeapon(modification as WeaponSpecification));
                    else if (modification is SpriteSpecification)
                        sprites = sprites.Append(modification as SpriteSpecification);
                }
            return sprites;
        }
    }
}
