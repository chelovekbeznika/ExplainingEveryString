using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class LastBossPhaseAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<LastBossBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(LastBossBlueprint blueprint)
        {
            return blueprint.Weapons.SelectMany(w => GetSpecEffectsFromWeapon(w.Weapon))
                .Append(blueprint.WeaponsChangeEffect)
                .Concat(base.GetSpecEffects(blueprint));
        }

        public IEnumerable<SpriteSpecification> GetSprites(LastBossBlueprint blueprint)
        {
            return blueprint.Weapons.SelectMany(w => GetSpritesFromWeapon(w.Weapon)).Concat(base.GetSprites(blueprint));
        }
    }
}
