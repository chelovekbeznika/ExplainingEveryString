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
        public String TileMap { get; set; }
        public ActorStartInfo PlayerPosition { get; set; }
        public EnemyWave[] EnemyWaves { get; set; }
        public Dictionary<String, PositionOnTileMap[]> WallsTilePositions { get; set; }
    }
}
