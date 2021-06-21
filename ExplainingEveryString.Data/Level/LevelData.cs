using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class LevelData
    {
        public String[] Blueprints { get; set; }
        public String MusicName { get; set; }
        public String TitleSpriteName { get; set; }
        public String TileMap { get; set; }
        public CheckpointSpecification StartCheckpoint { get; set; }
        public List<EnemyWave> EnemyWaves { get; set; }
        public FogOfWarSpecification FogOfWar { get; set; }
        public Dictionary<String, PositionOnTileMap[]> ObstaclesTilePositions { get; set; }
        [DefaultValue(null)]
        public SpriteEmitterData SpriteEmitter { get; set; }
    }
}
