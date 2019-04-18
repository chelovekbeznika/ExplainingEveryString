using ExplainingEveryString.Data.Blueprints;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Tests
{
    internal class HardCodeBlueprintsLoader : IBlueprintsLoader
    {
        private Dictionary<String, Blueprint> blueprints;

        public void Load()
        {
            blueprints = new Dictionary<String, Blueprint>()
            {
                {
                    "Mine",
                    new EnemyBlueprint()
                    {
                        DefaultSprite = new SpriteSpecification { Name = @"Sprites/Mine" },
                        Height = 16,
                        Width = 16,
                        Hitpoints = 7,
                        CollisionDamage = 1.5F,
                        MaxSpeed = 0,
                        DeathEffect = new SpecEffectSpecification()
                    }
                },
                {
                    "Player",
                    new PlayerBlueprint()
                    {
                        DefaultSprite = new SpriteSpecification { Name = @"Sprites/Rectangle" },
                        Height = 32,
                        Width = 32,
                        Hitpoints = 3,
                        MaxAcceleration = 200,
                        MaxSpeed = 200,
                        DamageEffect = new SpecEffectSpecification(),
                        Weapon = new PlayerWeaponBlueprint()
                        {
                            BulletSpeed = 800,
                            FireRate = 1,
                            Damage = 2,
                            WeaponRange = 2000,
                            BulletSprite = new SpriteSpecification { Name = @"Sprites/PlayerBullet" },
                            ShootingEffect = new SpecEffectSpecification()
                        }
                    }
                }
            };
        }

        public Dictionary<String, Blueprint> GetBlueprints()
        {
            return blueprints;
        }
    }
}
