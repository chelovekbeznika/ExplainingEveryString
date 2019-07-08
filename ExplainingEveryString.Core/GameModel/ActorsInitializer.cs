using ExplainingEveryString.Core.Math;
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

        internal List<Door> InitializeCommonDoors()
        {
            return InitializeDoors(null);
        }

        internal List<Door> InitializeClosingDoors(Int32 closesAt)
        {
            return InitializeDoors(closesAt);
        }

        private List<Door> InitializeDoors(Int32? closesAt)
        {
            List<Door> doors = new List<Door>();
            for (Int32 waveNumber = 0; waveNumber < levelData.EnemyWaves.Length; waveNumber++)
            {
                IEnumerable<DoorStartInfo> doorsStartInfo = levelData.EnemyWaves[waveNumber].Doors;
                doors.AddRange(InitializeDoors(doorsStartInfo, waveNumber, closesAt));
            }
            return doors;
        }

        private IEnumerable<Door> InitializeDoors
            (IEnumerable<DoorStartInfo> doorsStartInfo, Int32 openingWave, Int32? closesAt)
        {
            if (doorsStartInfo != null)
            {
                IEnumerable<Door> waveDoors = doorsStartInfo
                    .Where(dsi => dsi.ClosesAt == closesAt).Select(dsi => Convert(dsi))
                    .Select(asi => actorsFactory.ConstructDoor(asi, openingWave));
                return waveDoors;
            }
            else
                return Enumerable.Empty<Door>();
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

        internal Hitbox InitializeStartRegion(Int32 waveNumber)
        {
            return map.GetHitbox(levelData.EnemyWaves[waveNumber].StartRegion);
        }

        internal ValueTuple<List<IEnemy>, Queue<IEnemy>> InitializeEnemies(Int32 waveNumber)
        {
            EnemyWave wave = levelData.EnemyWaves[waveNumber];
            IEnumerable<ActorStartInfo> enemiesStartInfos = wave.Enemies.Select(asi => Convert(asi, wave));
            IEnumerable<IEnemy> enemies = actorsFactory.ConstructEnemies(enemiesStartInfos);
            return (new List<IEnemy>(enemies.Take(wave.MaxEnemiesAtOnce)), 
                new Queue<IEnemy>(enemies.Skip(wave.MaxEnemiesAtOnce)));
        }

        private ActorStartInfo Convert(Data.Level.ActorStartInfo dataLayerStartInfo, EnemyWave enemyWave = null)
        {
            return new ActorStartInfo
            {
                BlueprintType = dataLayerStartInfo.BlueprintType,
                Position = map.GetPosition(dataLayerStartInfo.TilePosition),
                Angle = AngleConverter.ToRadians(dataLayerStartInfo.Angle),
                TrajectoryTargets = dataLayerStartInfo.TrajectoryTargets,
                AppearancePhaseDuration = dataLayerStartInfo.AppearancePhaseDuration,
                SpawnPoints = enemyWave?.SpawnPoints?.Select(sp => map.GetPosition(sp)).ToArray()
            };
        }
    }
}
