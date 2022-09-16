using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class SpawnedActorsController : ISpawnedActorsController
    {
        internal SpawnerSpecification Specification { get; private set; }
        private Reloader reloader;
        private ISpawnPositionSelector spawnPositionSelector;
        private ActorsFactory factory;
        private IActor spawner;
        private Vector2 spawnerStartPosition;
        private Boolean active = false;
        private Boolean despawnAfterDeath;

        public event EventHandler<EnemySpawnedEventArgs> EnemySpawned;

        public List<IEnemy> SpawnedEnemies { get; private set; } = new List<IEnemy>();

        public Int32 MaxSpawned => Specification.MaxSpawned;

        internal SpawnedActorsController(SpawnerSpecification specification, IActor spawner, 
            BehaviorParameters spawnerBehaviorParameters, ActorsFactory factory)
        {
            this.Specification = specification;
            this.reloader = new Reloader(specification.Reloader, CanSpawnEnemy, SpawnEnemy);
            this.spawner = spawner;
            this.spawnerStartPosition = (spawner as ICollidable).Position;
            this.factory = factory;
            this.spawnPositionSelector = SpawnPositionSelectorsFactory.Get(
                specification.PositionSelector, spawnerBehaviorParameters.CustomSpawns);
            this.despawnAfterDeath = specification.DespawnAfterDeath;
        }

        public void Update(Single elapsedSeconds)
        {
            if (active)
                reloader.Update(elapsedSeconds, out _);
        }

        public void DivideAliveAndDead(List<IEnemy> avengers)
        {
            SpawnedEnemies = EnemiesDeathProcessor.DivideAliveAndDead(SpawnedEnemies, avengers);
        }

        public void TurnOn()
        {
            active = true;
        }

        public void TurnOff()
        {
            active = false;
        }

        public void DespawnRoutine()
        {
            if (despawnAfterDeath)
                foreach (var enemy in SpawnedEnemies)
                    enemy.Despawn();
        }

        private void SpawnEnemy(Single firstUpdateTime)
        {
            var spawnSpecification = spawnPositionSelector.GetNextSpawnSpecification();
            var asi = new ActorStartInfo
            {
                BlueprintType = Specification.BlueprintType,
                Position = Specification.SpawnPositionRelativeToCurrentPosition
                    ? spawnSpecification.SpawnPoint + (spawner as ICollidable).Position
                    : spawnSpecification.SpawnPoint + spawnerStartPosition,
                BehaviorParameters = new BehaviorParameters
                {
                    TrajectoryParameters = spawnSpecification.TrajectoryParameters?.ToArray(),
                    Angle = AngleConverter.ToRadians(spawnSpecification.Angle)
                },
                AppearancePhaseDuration = Specification.AppearancePhase
            };
            var enemy = factory.ConstructEnemy(asi);
            enemy.Update(firstUpdateTime);
            SpawnedEnemies.Add(enemy);
            EnemySpawned?.Invoke(this, new EnemySpawnedEventArgs { Enemy = enemy });
        }

        private Boolean CanSpawnEnemy() => active && spawner.IsAlive() && SpawnedEnemies.Count < Specification.MaxSpawned;
    }
}
