﻿using System;
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
                    Width = 16
                },
                new PlayerBlueprint()
                {
                    DefaultSpriteName = @"Sprites/Rectangle",
                    Height = 32,
                    Width = 32,
                    MaxAcceleration = 200,
                    MaxSpeed = 200,
                    BulletSpeed = 800,
                    FireRate = 1,
                    WeaponRange = 400,
                    BulletSpriteName = @"Sprites/PlayerBullet"
                }
            };
        }

        public List<Blueprint> GetBlueprints()
        {
            return blueprints;
        }
    }
}