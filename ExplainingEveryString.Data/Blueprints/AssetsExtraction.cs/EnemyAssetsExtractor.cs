﻿using System.Collections.Generic;
using System.Linq;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs
{
    internal class EnemyAssetsExtractor : ActorAssetsExtractor, IAssetsExtractor<EnemyBlueprint>
    {
        public IEnumerable<SpriteSpecification> GetSprites(EnemyBlueprint blueprint)
        {
            IEnumerable<SpriteSpecification> sprites = 
                base.GetSprites(blueprint).Concat( new SpriteSpecification[] { blueprint.AppearancePhaseSprite});
            return blueprint.Behavior.Weapon != null ? sprites.Concat(GetSpritesFromWeapon(blueprint.Behavior.Weapon)) : sprites;
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(EnemyBlueprint blueprint)
        {
            List<SpecEffectSpecification> specEffects = new List<SpecEffectSpecification>
            {
                blueprint.DeathEffect, blueprint.BeforeAppearanceEffect, blueprint.AfterAppearanceEffect
            };
            if (blueprint.Behavior.Weapon != null)
                specEffects.Add(blueprint.Behavior.Weapon.ShootingEffect);
            return specEffects;
        }
    }
}
