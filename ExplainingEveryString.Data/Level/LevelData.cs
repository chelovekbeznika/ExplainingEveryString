using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Level
{
    public class LevelData
    {
        public GameObjectStartPosition PlayerPosition { get; set; }
        public Dictionary<String, List<GameObjectStartPosition>> EnemiesPositions { get; set; }
        public Dictionary<String, List<Vector2>> WallsPositions { get; set; }
    }
}
