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
                .Concat(GetSpritesFromWeapon(blueprint.Weapon))
                .Concat(new SpriteSpecification[] { blueprint.Dash.Sprite });
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(PlayerBlueprint blueprint)
        {
            return base.GetSpecEffects(blueprint).Concat(new SpecEffectSpecification[]
            {
                blueprint.Weapon.ShootingEffect, blueprint.DamageEffect,
                blueprint.BaseDestructionEffect, blueprint.CannonDestructionEffect,
                blueprint.Dash.SpecEffect
            });
        }
    }
}
