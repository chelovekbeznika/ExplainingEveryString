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
    internal class SpawnedActorsController : IUpdateable
    {
        private String enemyType;
        private readonly Int32 maxSpawned;
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
            this.spawnPositionSelector = SpawnPositionSelectorsFactory.Get(
                specification.PositionSelector, () => (spawner as ICollidable).Position, levelSpawnPoints);
        }

        public void Update(Single elapsedSeconds)
        {
            reloader.TryReload(elapsedSeconds, out Boolean enemySpawned);
        }

        internal void SendDeadToHeaven(List<IEnemy> avengers)
        {
            SpawnedEnemies = EnemyDeathProcessor.SendDeadToHeaven(SpawnedEnemies, avengers);
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
