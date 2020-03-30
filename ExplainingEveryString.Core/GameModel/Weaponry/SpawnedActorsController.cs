using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class SpawnedActorsController : IUpdateable
    {
        private String enemyType;
        private readonly Int32 maxSpawned;
        private readonly Single appearancePhase;
        private Reloader reloader;
        private Vector2[] levelSpawnPoints;
        private ISpawnPositionSelector spawnPositionSelector;
        private ActorsFactory factory;
        private IActor spawner;
        private Boolean active = false;

        internal List<IEnemy> SpawnedEnemies { get; private set; } = new List<IEnemy>();

        internal SpawnedActorsController(SpawnerSpecification specification, IActor spawner, 
            BehaviorParameters spawnerBehaviorParameters, ActorsFactory factory)
        {
            this.enemyType = specification.BlueprintType;
            this.maxSpawned = specification.MaxSpawned;
            this.appearancePhase = specification.AppearancePhase;
            this.reloader = new Reloader(specification.Reloader, CanSpawnEnemy, SpawnEnemy);
            this.spawner = spawner;
            this.levelSpawnPoints = spawnerBehaviorParameters.LevelSpawnPoints;
            this.factory = factory;
            this.spawnPositionSelector = SpawnPositionSelectorsFactory.Get(specification.PositionSelector, 
                () => (spawner as ICollidable).Position, levelSpawnPoints, spawnerBehaviorParameters.CustomSpawns);
        }

        public void Update(Single elapsedSeconds)
        {
            if (active)
                reloader.TryReload(elapsedSeconds, out Boolean enemySpawned);
        }

        internal void SendDeadToHeaven(List<IEnemy> avengers)
        {
            SpawnedEnemies = EnemyDeathProcessor.SendDeadToHeaven(SpawnedEnemies, avengers);
        }

        internal void TurnOn()
        {
            active = true;
        }

        internal void TurnOff()
        {
            active = false;
        }

        private void SpawnEnemy(Single firstUpdateTime)
        {
            var spawnSpecification = spawnPositionSelector.GetNextSpawnSpecification();
            var asi = new ActorStartInfo
            {
                BlueprintType = enemyType,
                Position = spawnSpecification.SpawnPoint + (spawner as ICollidable).Position,
                BehaviorParameters = new BehaviorParameters
                {
                    LevelSpawnPoints = levelSpawnPoints,
                    TrajectoryParameters = spawnSpecification.TrajectoryParameters,
                    Angle = spawnSpecification.Angle
                },
                AppearancePhaseDuration = appearancePhase
            };
            var enemy = factory.ConstructEnemy(asi);
            enemy.Update(firstUpdateTime);
            SpawnedEnemies.Add(enemy);
        }

        private Boolean CanSpawnEnemy() => active && spawner.IsAlive() && SpawnedEnemies.Count < maxSpawned;
    }
}
