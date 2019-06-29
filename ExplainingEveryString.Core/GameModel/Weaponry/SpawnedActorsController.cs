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
        private Vector2[] spawnPoints;
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
            this.spawnPoints = spawnPoints;
            this.factory = factory;
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
                Position = spawnPoints[RandomUtility.NextInt(spawnPoints.Length)],
                SpawnPoints = spawnPoints
            };
            IEnemy enemy = factory.ConstructEnemy(asi);
            enemy.Update(firstUpdateTime);
            SpawnedEnemies.Add(enemy);
        }

        private Boolean CanSpawnEnemy() => spawner.IsAlive() && SpawnedEnemies.Count < maxSpawned;
    }
}
