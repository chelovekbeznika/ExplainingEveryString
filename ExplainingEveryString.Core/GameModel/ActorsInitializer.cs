using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActorsInitializer
    {
        private TileWrapper map;
        private ActorsFactory actorsFactory;
        private WallsFactory wallsFactory;
        private LevelData levelData;

        internal ActorsInitializer(TileWrapper map, ActorsFactory actorsFactory, LevelData levelData)
        {
            this.map = map;
            this.actorsFactory = actorsFactory;
            this.wallsFactory = new WallsFactory(map);
            this.levelData = levelData;
        }

        internal List<Door> InitializeCommonDoors(Int32 startWave)
        {
            return InitializeDoors(startWave, dsi => !dsi.ClosesAt.HasValue || dsi.ClosesAt < startWave);
        }

        internal List<Door> InitializeClosingDoors(Int32 closesAt)
        {
            return InitializeDoors(closesAt, dsi => dsi.ClosesAt == closesAt);
        }

        private List<Door> InitializeDoors(Int32 startWave, Func<DoorStartInfo, Boolean> dsiFilter)
        {
            List<Door> result = new List<Door>();
            foreach (Int32 waveNumber in Enumerable.Range(startWave, levelData.EnemyWaves.Length - startWave))
            {
                EnemyWave wave = levelData.EnemyWaves[waveNumber];
                DoorStartInfo[] doorsInfo = wave.Doors;
                if (doorsInfo != null)
                {
                    IEnumerable<Door> waveDoors = doorsInfo
                        .Where(dsiFilter)
                        .Select(dsi => Convert(dsi, wave))
                        .Select(asi => actorsFactory.ConstructDoor(asi, waveNumber));
                    result.AddRange(waveDoors);
                }
            }
            return result;
        }

        internal List<IActor> InitializeObstacles()
        {
            List<IActor> result = new List<IActor>();
            foreach (String obstacleType in levelData.ObstaclesTilePositions.Keys)
            {
                List<Vector2> obstaclePositions = levelData.ObstaclesTilePositions[obstacleType]
                    .Select(t => map.GetPosition(t)).ToList();
                result.AddRange(actorsFactory.ConstructObstacles(obstacleType, obstaclePositions));
            }
            return result;
        }

        internal Int32 MaxEnemiesAtOnce(Int32 waveNumber)
        {
            return levelData.EnemyWaves[waveNumber].MaxEnemiesAtOnce;
        }

        internal ICollidable[] InitializeWalls()
        {
            return wallsFactory.ConstructWalls().OfType<ICollidable>().ToArray();
        }

        internal Player InitializePlayer(ActorStartInfo playerStartInfo) => actorsFactory.ConstructPlayer(playerStartInfo);

        internal Hitbox InitializeStartRegion(Int32 waveNumber)
        {
            return map.GetHitbox(levelData.EnemyWaves[waveNumber].StartRegion);
        }

        internal Queue<IEnemy> InitializeEnemies(Int32 waveNumber)
        {
            EnemyWave wave = levelData.EnemyWaves[waveNumber];
            IEnumerable<ActorStartInfo> enemiesStartInfos = wave.Enemies.Select(asi => Convert(asi, wave));
            IEnumerable<IEnemy> enemies = actorsFactory.ConstructEnemies(enemiesStartInfos);
            return new Queue<IEnemy>(enemies);
        }

        private ActorStartInfo Convert(Data.Level.ActorStartInfo dataLayerStartInfo, EnemyWave enemyWave = null)
        {
            return new ActorStartInfo
            {
                BlueprintType = dataLayerStartInfo.BlueprintType,
                Position = map.GetPosition(dataLayerStartInfo.TilePosition),
                Angle = AngleConverter.ToRadians(dataLayerStartInfo.Angle),
                TrajectoryParameters = dataLayerStartInfo.TrajectoryParameters,
                AppearancePhaseDuration = dataLayerStartInfo.AppearancePhaseDuration,
                LevelSpawnPoints = enemyWave?.SpawnPoints?.Select(sp => map.GetPosition(sp)).ToArray()
            };
        }
    }
}
