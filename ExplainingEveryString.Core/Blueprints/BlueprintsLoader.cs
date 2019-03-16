using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Blueprints
{
    internal interface IBlueprintsLoader
    {
        List<Blueprint> Load();
    }

    internal class HardCodeBlueprintsLoader : IBlueprintsLoader
    {
        public List<Blueprint> Load()
        {
            return new List<Blueprint>()
            {
                new MineBlueprint()
                {
                    DefaultSpriteName = "mine",
                    Height = 16,
                    Width = 16
                },
                new PlayerBlueprint()
                {
                    DefaultSpriteName = "player",
                    Height = 32,
                    Width = 32,
                    MaxAcceleration = 200,
                    MaxSpeed = 200
                }
            };
        }
    }
}
