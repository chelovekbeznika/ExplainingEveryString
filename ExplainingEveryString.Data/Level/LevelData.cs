using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Level
{
    public class LevelData
    {
        public String MusicName { get; set; }
        public String TileMap { get; set; }
        public ActorStartInfo PlayerPosition { get; set; }
        public EnemyWave[] EnemyWaves { get; set; }
        public FogOfWarSpecification FogOfWar { get; set; }
        public Dictionary<String, PositionOnTileMap[]> ObstaclesTilePositions { get; set; }
    }
}
