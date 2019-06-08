using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActorsInitializer
    {
        private TileWrapper map;
        private ActorsFactory actorsFactory;
        private TileWallsFactory tileWallsFactory;
        private LevelData levelData;

        internal ActorsInitializer(TileWrapper map, ActorsFactory actorsFactory, LevelData levelData)
        {
            this.map = map;
            this.actorsFactory = actorsFactory;
            this.tileWallsFactory = new TileWallsFactory(map);
            this.levelData = levelData;
        }

        internal List<IActor> InitializeWalls()
        {
            List<IActor> result = new List<IActor>();
            foreach (String wallType in levelData.WallsTilePositions.Keys)
            {
                List<Vector2> wallPositions = levelData.WallsTilePositions[wallType]
                    .Select(t => map.GetPosition(t)).ToList();
                result.AddRange(actorsFactory.ConstructWalls(wallType, wallPositions));
            }
            return result;
        }

        internal ICollidable[] InitializeTileWalls()
        {
            return tileWallsFactory.ConstructTileWalls().OfType<ICollidable>().ToArray();
        }

        internal Player InitializePlayer() => actorsFactory.ConstructPlayer(Convert(levelData.PlayerPosition));

        internal List<IActor> InitializeEnemies(Int32 waveNumber)
        {
            EnemyWave wave = levelData.EnemyWaves[waveNumber];
            List<IActor> enemies = new List<IActor>();
            foreach (String enemyType in wave.EnemiesPositions.Keys)
            {
                IEnumerable<ActorStartInfo> enemiesPositions = wave.EnemiesPositions[enemyType].Select(asi => Convert(asi));
                enemies.AddRange(actorsFactory.ConstructEnemies(enemyType, enemiesPositions));
            }
            return enemies;
        }

        private ActorStartInfo Convert(Data.Level.ActorStartInfo dataLayerStartInfo)
        {
            return new ActorStartInfo
            {
                Position = map.GetPosition(dataLayerStartInfo.TilePosition),
                Angle = dataLayerStartInfo.Angle,
                TrajectoryTargets = dataLayerStartInfo.TrajectoryTargets
            };
        }
    }
}
