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
        List<String> GetNeccessarySprites();
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
                    MaxSpeed = 200
                }
            };
        }

        public List<Blueprint> GetBlueprints()
        {
            return blueprints;
        }

        public List<string> GetNeccessarySprites()
        {
            return blueprints.Select(blueprint => blueprint.DefaultSpriteName).ToList();
        }
    }
}
