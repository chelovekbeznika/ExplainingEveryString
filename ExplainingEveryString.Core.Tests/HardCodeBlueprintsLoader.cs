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
                        DefaultSpriteName = @"Sprites/Mine",
                        Height = 16,
                        Width = 16,
                        Hitpoints = 7,
                        CollisionDamage = 1.5F,
                        MaxSpeed = 0
                    }
                },
                {
                    "Player",
                    new PlayerBlueprint()
                    {
                        DefaultSpriteName = @"Sprites/Rectangle",
                        Height = 32,
                        Width = 32,
                        Hitpoints = 3,
                        MaxAcceleration = 200,
                        MaxSpeed = 200,
                        Weapon = new PlayerWeaponBlueprint()
                        {
                            BulletSpeed = 800,
                            FireRate = 1,
                            Damage = 2,
                            WeaponRange = 2000,
                            BulletSpriteName = @"Sprites/PlayerBullet"
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
