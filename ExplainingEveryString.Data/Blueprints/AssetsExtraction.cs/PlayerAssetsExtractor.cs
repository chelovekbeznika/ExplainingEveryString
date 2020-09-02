using System.Collections.Generic;
using System.Linq;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs
{
    internal class PlayerAssetsExtractor : ActorAssetsExtractor, IAssetsExtractor<PlayerBlueprint>
    {
        public IEnumerable<SpriteSpecification> GetSprites(PlayerBlueprint blueprint)
        {
            return base.GetSprites(blueprint)
                .Concat(blueprint.Weapons.SelectMany(weapon => GetSpritesFromWeapon(weapon)))
                .Concat(new SpriteSpecification[] { blueprint.Dash.Sprite });
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(PlayerBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint)
                .Concat(blueprint.Weapons.SelectMany(weapon => GetSpecEffectsFromWeapon(weapon)))
                .Concat(new SpecEffectSpecification[]
            {
                blueprint.BaseDestructionEffect, blueprint.WeaponSwitchEffect,
                blueprint.CannonDestructionEffect, blueprint.Dash.SpecEffect,
                blueprint.DamageEffect, blueprint.SoftDamageEffect, blueprint.RefreshEffect
            });
        }
    }
}
