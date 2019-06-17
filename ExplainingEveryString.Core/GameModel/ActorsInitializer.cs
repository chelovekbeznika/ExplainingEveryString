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

        internal List<Door> InitializeDoors()
        {
            List<Door> doors = new List<Door>();
            for (Int32 waveNumber = 0; waveNumber < levelData.EnemyWaves.Length; waveNumber++)
            {
                List<Data.Level.ActorStartInfo> doorsStartInfo = levelData.EnemyWaves[waveNumber].Doors;
                if (doorsStartInfo != null)
                {
                    IEnumerable<Door> waveDoors = doorsStartInfo.Select(d => Convert(d))
                        .Select(d => actorsFactory.ConstructDoor(d, waveNumber));
                    doors.AddRange(waveDoors);
                }
            }
            return doors;
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

        internal ValueTuple<List<IActor>, Queue<IActor>> InitializeEnemies(Int32 waveNumber)
        {
            EnemyWave wave = levelData.EnemyWaves[waveNumber];
            IEnumerable<ActorStartInfo> enemiesStartInfos = wave.Enemies.Select(asi => Convert(asi));
            IEnumerable<IActor> enemies = actorsFactory.ConstructEnemies(enemiesStartInfos);
            return (new List<IActor>(enemies.Take(wave.MaxEnemiesAtOnce)), 
                new Queue<IActor>(enemies.Skip(wave.MaxEnemiesAtOnce)));
        }

        private ActorStartInfo Convert(Data.Level.ActorStartInfo dataLayerStartInfo)
        {
            return new ActorStartInfo
            {
                BlueprintType = dataLayerStartInfo.BlueprintType,
                Position = map.GetPosition(dataLayerStartInfo.TilePosition),
                Angle = dataLayerStartInfo.Angle,
                TrajectoryTargets = dataLayerStartInfo.TrajectoryTargets,
                AppearancePhaseDuration = dataLayerStartInfo.AppearancePhaseDuration
            };
        }
    }
}
