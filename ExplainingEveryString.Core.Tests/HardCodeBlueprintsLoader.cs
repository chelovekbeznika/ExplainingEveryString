using ExplainingEveryString.Data.Blueprints;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Tests
{
    internal class HardCodeBlueprintsLoader : IBlueprintsLoader
    {
        private List<Blueprint> blueprints;

        public void Load()
        {
            blueprints = new List<Blueprint>()
            {
                new MineBlueprint()
                {
                    DefaultSpriteName = @"Sprites/Mine",
                    Height = 16,
                    Width = 16,
                    Hitpoints = 7,
                    Damage = 1.5F,
                },
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
            };
        }

        public List<Blueprint> GetBlueprints()
        {
            return blueprints;
        }
    }
}
