using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class SecondBossPowerKeepersSpawner : ISpawnedActorsController
    {
        private SecondBoss spawner;
        private ActorsFactory factory;
        private EventHandler powerKeeperDeath;
        internal OneTimeSpawnerSpecification Specification { get; private set; }
        private Single tillNextSpawn = 0;
        private Int32 alreadySpawned = 0;

        public event EventHandler<EnemySpawnedEventArgs> EnemySpawned;

        public List<IEnemy> SpawnedEnemies { get; private set; }

        public Int32 MaxSpawned { get => Specification.MaxSpawned; set => Specification.MaxSpawned = value; }
        public Boolean EveryoneSpawned => alreadySpawned == MaxSpawned;

        internal SecondBossPowerKeepersSpawner(OneTimeSpawnerSpecification specification, SecondBoss spawner, 
            ActorsFactory factory, EventHandler powerKeeperDeath)
        {
            this.Specification = specification;
            this.spawner = spawner;
            this.factory = factory;
            this.powerKeeperDeath = powerKeeperDeath;
            this.SpawnedEnemies = new List<IEnemy>();
        }

        public void DivideAliveAndDead(List<IEnemy> avengers)
        {
            SpawnedEnemies = EnemiesDeathProcessor.DivideAliveAndDead(SpawnedEnemies, avengers);
        }

        public void TurnOff()
        {
        }

        public void TurnOn()
        {
        }

        public void Update(Single elapsedSeconds)
        {
            tillNextSpawn -= elapsedSeconds;
            while (tillNextSpawn <= 0 && alreadySpawned < Specification.MaxSpawned)
            {
                tillNextSpawn += Specification.Interval;
                alreadySpawned += 1;
                var enemy = factory.ConstructEnemy(new ActorStartInfo
                {
                    Position = (spawner as ICollidable).Position,
                    BlueprintType = Specification.BlueprintType,
                    BehaviorParameters = new BehaviorParameters { }
                });
                enemy.Died += powerKeeperDeath;
                SpawnedEnemies.Add(enemy);
                EnemySpawned?.Invoke(this, new EnemySpawnedEventArgs { Enemy = enemy });
            }
        }

        public void Reset()
        {
            alreadySpawned = 0;
            tillNextSpawn = 0;
        }

        public void DespawnRoutine()
        {
        }
    }
}
