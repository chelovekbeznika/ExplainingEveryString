using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
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
            var result = new List<Door>();
            foreach (var waveNumber in Enumerable.Range(startWave, levelData.EnemyWaves.Count - startWave))
            {
                var wave = levelData.EnemyWaves[waveNumber];
                var doorsInfo = wave.Doors;
                if (doorsInfo != null)
                {
                    IEnumerable<Door> waveDoors = doorsInfo
                        .Where(dsiFilter)
                        .Select(dsi => Convert(dsi))
                        .Select(asi => actorsFactory.ConstructDoor(asi, waveNumber));
                    result.AddRange(waveDoors);
                }
            }
            return result;
        }

        internal List<IActor> InitializeObstacles()
        {
            var result = new List<IActor>();
            foreach (var obstacleType in levelData.ObstaclesTilePositions.Keys)
            {
                var obstaclePositions = levelData.ObstaclesTilePositions[obstacleType]
                    .Select(t => map.GetLevelPosition(t)).ToList();
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
            var wave = levelData.EnemyWaves[waveNumber];
            var enemiesStartInfos = wave.Enemies.Select(asi => Convert(asi));
            var enemies = actorsFactory.ConstructEnemies(enemiesStartInfos);
            return new Queue<IEnemy>(enemies);
        }

        internal IEnumerable<IEnemy> InitializeBosses(Int32 waveNumber)
        {
            var wave = levelData.EnemyWaves[waveNumber];
            if (wave.Bosses != null)
                return wave.Bosses.Select(boss => actorsFactory.ConstructEnemy(Convert(boss)));
            else
                return null;
        }

        private ActorStartInfo Convert(Data.Level.ActorStartInfo dataLayerStartInfo)
        {
            return new ActorStartInfo
            {
                BlueprintType = dataLayerStartInfo.BlueprintType,
                Position = map.GetLevelPosition(dataLayerStartInfo.TilePosition),
                BehaviorParameters = new BehaviorParameters
                {
                    Angle = AngleConverter.ToRadians(dataLayerStartInfo.Angle),
                    TrajectoryParameters = dataLayerStartInfo.TrajectoryParameters?.ToArray(),
                    CustomSpawns = dataLayerStartInfo.CustomSpawns
                },
                AppearancePhaseDuration = dataLayerStartInfo.AppearancePhaseDuration
            };
        }
    }
}
