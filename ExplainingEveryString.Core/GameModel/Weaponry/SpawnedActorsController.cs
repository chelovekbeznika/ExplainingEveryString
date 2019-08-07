using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class SpawnedActorsController : IUpdatable
    {
        private String enemyType;
        private Int32 maxSpawned;
        private Reloader reloader;
        private Vector2[] levelSpawnPoints;
        private ISpawnPositionSelector spawnPositionSelector;
        private ActorsFactory factory;
        private IActor spawner;
        internal List<IEnemy> SpawnedEnemies { get; private set; } = new List<IEnemy>();

        internal SpawnedActorsController(SpawnerSpecification specification, IActor spawner, 
            Vector2[] spawnPoints, ActorsFactory factory)
        {
            this.enemyType = specification.BlueprintType;
            this.maxSpawned = specification.MaxSpawned;
            this.reloader = new Reloader(specification.Reloader, CanSpawnEnemy, SpawnEnemy);
            this.spawner = spawner;
            this.levelSpawnPoints = spawnPoints;
            this.factory = factory;
            InitializeSpawnPositionSelector(specification);
        }

        private void InitializeSpawnPositionSelector(SpawnerSpecification specification)
        {
            Func<Vector2> spawnerLocator = () => (spawner as ICollidable).Position;
            switch (specification.PositionSelectionType)
            {
                case SpawnPositionSelectionType.LevelSpawnPoints:
                    spawnPositionSelector = new LevelSpawnPositionSelector(levelSpawnPoints);
                    break;
                case SpawnPositionSelectionType.RandomInCircle:
                    Single maxRadius = (specification as RandomSpawnerSpecification).SpawnRadius;
                    spawnPositionSelector = new RandomSpawnPositionSelector(spawnerLocator, maxRadius);
                    break;
                case SpawnPositionSelectionType.RelativeToSpawner:
                    Vector2[] spawnPositions = (specification as RelativeSpawnerSpecificaton).SpawnPositions;
                    spawnPositionSelector = new RelativeSpawnPositionSelector(spawnerLocator, spawnPositions);
                    break;
            }
        }

        public void Update(Single elapsedSeconds)
        {
            reloader.TryReload(elapsedSeconds, out Boolean enemySpawned);
        }

        internal void SendDeadToHeaven()
        {
            SpawnedEnemies = SpawnedEnemies.Where(e => e.IsAlive()).ToList();
        }

        private void SpawnEnemy(Single firstUpdateTime)
        {
            ActorStartInfo asi = new ActorStartInfo
            {
                BlueprintType = enemyType,
                Position = spawnPositionSelector.GetNextSpawnPosition(),
                LevelSpawnPoints = levelSpawnPoints
            };
            IEnemy enemy = factory.ConstructEnemy(asi);
            enemy.Update(firstUpdateTime);
            SpawnedEnemies.Add(enemy);
        }

        private Boolean CanSpawnEnemy() => spawner.IsAlive() && SpawnedEnemies.Count < maxSpawned;
    }
}
