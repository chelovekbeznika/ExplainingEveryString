using System.Collections.Generic;
using System.Linq;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class EnemyAssetsExtractor : ActorAssetsExtractor, IAssetsExtractor<EnemyBlueprint>
    {
        public IEnumerable<SpriteSpecification> GetSprites(EnemyBlueprint blueprint)
        {
            IEnumerable<SpriteSpecification> sprites = 
                base.GetSprites(blueprint).Append(blueprint.AppearancePhaseSprite);
            if (blueprint.Behavior.PostMortemSurprise?.Weapon != null)
                sprites = sprites.Concat(GetSpritesFromWeapon(blueprint.Behavior.PostMortemSurprise.Weapon));
            if (blueprint.Behavior.Weapon != null)
                sprites = sprites.Concat(GetSpritesFromWeapon(blueprint.Behavior.Weapon));
            return sprites;
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(EnemyBlueprint blueprint)
        {
            List<SpecEffectSpecification> specEffects = new List<SpecEffectSpecification>
            {
                blueprint.DeathEffect, blueprint.BeforeAppearanceEffect, blueprint.AfterAppearanceEffect, blueprint.GoalAchievedEffect
            };
            if (blueprint.Behavior.Weapon != null)
                specEffects.AddRange(GetSpecEffectsFromWeapon(blueprint.Behavior.Weapon));
            return specEffects;
        }
    }
}
