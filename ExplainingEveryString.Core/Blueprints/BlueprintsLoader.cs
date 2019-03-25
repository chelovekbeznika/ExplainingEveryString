using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Blueprints
{
    internal interface IBlueprintsLoader
    {
        void Load();
        List<Blueprint> GetBlueprints();
    }

    internal static class IBlueprintsLoaderExtenstions
    {
        internal static List<String> GetNeccessarySprites(this IBlueprintsLoader loader)
        {
            List<Blueprint> blueprints = loader.GetBlueprints();
            return blueprints.SelectMany(blueprint => blueprint.GetSprites()).ToList();
        }
    }

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
                    Hitpoints = 3
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
