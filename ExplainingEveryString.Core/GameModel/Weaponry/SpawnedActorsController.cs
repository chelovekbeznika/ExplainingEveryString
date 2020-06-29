﻿using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class SpawnedActorsController : ISpawnedActorsController
    {
        private SpawnerSpecification specification;
        private Reloader reloader;
        private ISpawnPositionSelector spawnPositionSelector;
        private ActorsFactory factory;
        private IActor spawner;
        private Vector2 spawnerStartPosition;
        private Boolean active = false;

        public List<IEnemy> SpawnedEnemies { get; private set; } = new List<IEnemy>();

        internal SpawnedActorsController(SpawnerSpecification specification, IActor spawner, 
            BehaviorParameters spawnerBehaviorParameters, ActorsFactory factory)
        {
            this.specification = specification;
            this.reloader = new Reloader(specification.Reloader, CanSpawnEnemy, SpawnEnemy);
            this.spawner = spawner;
            this.spawnerStartPosition = (spawner as ICollidable).Position;
            this.factory = factory;
            this.spawnPositionSelector = SpawnPositionSelectorsFactory.Get(
                specification.PositionSelector, spawnerBehaviorParameters.CustomSpawns);
        }

        public void Update(Single elapsedSeconds)
        {
            if (active)
                reloader.TryReload(elapsedSeconds, out Boolean enemySpawned);
        }

        public void SendDeadToHeaven(List<IEnemy> avengers)
        {
            SpawnedEnemies = EnemyDeathProcessor.SendDeadToHeaven(SpawnedEnemies, avengers);
        }

        public void TurnOn()
        {
            active = true;
        }

        public void TurnOff()
        {
            active = false;
        }

        private void SpawnEnemy(Single firstUpdateTime)
        {
            var spawnSpecification = spawnPositionSelector.GetNextSpawnSpecification();
            var asi = new ActorStartInfo
            {
                BlueprintType = specification.BlueprintType,
                Position = specification.SpawnPositionRelativeToCurrentPosition
                    ? spawnSpecification.SpawnPoint + (spawner as ICollidable).Position
                    : spawnSpecification.SpawnPoint + spawnerStartPosition,
                BehaviorParameters = new BehaviorParameters
                {
                    TrajectoryParameters = spawnSpecification.TrajectoryParameters?.ToArray(),
                    Angle = spawnSpecification.Angle
                },
                AppearancePhaseDuration = specification.AppearancePhase
            };
            var enemy = factory.ConstructEnemy(asi);
            enemy.Update(firstUpdateTime);
            SpawnedEnemies.Add(enemy);
        }

        private Boolean CanSpawnEnemy() => active && spawner.IsAlive() && SpawnedEnemies.Count < specification.MaxSpawned;
    }
}
