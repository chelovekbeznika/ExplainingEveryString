using System.Collections.Generic;
using System.Linq;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction
{
    internal class ActorAssetsExtractor : IAssetsExtractor<Blueprint>, IAssetsExtractor<ObstacleBlueprint>
    {
        public IEnumerable<SpriteSpecification> GetSprites(Blueprint blueprint)
        {
            return new SpriteSpecification[] { blueprint.DefaultSprite };
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(Blueprint blueprint)
        {
            return Enumerable.Empty<SpecEffectSpecification>();
        }

        public IEnumerable<SpriteSpecification> GetSpritesFromWeapon(WeaponSpecification specification)
        {
            return new SpriteSpecification[] { specification.Sprite }
                .Concat(specification.Barrels.Select(b => b.Bullet.Sprite));
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffectsFromWeapon(WeaponSpecification specification)
        {
            return new[] { specification.ShootingEffect, specification.Reloader.ReloadStartedEffect }
                .Concat(specification.Barrels.Select(barrel => barrel.Bullet.HitEffect));
        }

        public IEnumerable<SpriteSpecification> GetSpritesFromWeapon(PostMortemWeaponSpecification specification)
        {
            return specification.Barrels.Select(b => b.Bullet.Sprite);
        }

        public IEnumerable<SpriteSpecification> GetSprites(ObstacleBlueprint blueprint)
        {
            return GetSprites(blueprint as Blueprint);
        }

        public IEnumerable<SpecEffectSpecification> GetSpecEffects(ObstacleBlueprint blueprint)
        {
            return GetSpecEffects(blueprint as Blueprint);
        }
    }
}
