using ExplainingEveryString.Data.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class FifthBossAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<FifthBossBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(FifthBossBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint)
                .Concat(GetSpecEffectsFromWeapon(blueprint.LeftEye.Weapon))
                .Concat(GetSpecEffectsFromWeapon(blueprint.RightEye.Weapon))
                .Concat(GetSpecEffectsFromWeapon(blueprint.Tentacle.Weapon));
        }

        public IEnumerable<SpriteSpecification> GetSprites(FifthBossBlueprint blueprint)
        {
            return base.GetSprites(blueprint)
                .Concat(new[] { blueprint.LeftEye.Sprite, blueprint.RightEye.Sprite, blueprint.Tentacle.Sprite });
        }
    }
}
