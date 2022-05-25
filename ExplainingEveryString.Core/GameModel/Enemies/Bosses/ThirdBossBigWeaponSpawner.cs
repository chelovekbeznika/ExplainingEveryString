using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class ThirdBossBigGunSpawner : ISpawnedActorsController
    {
        private ActorsFactory factory;
        private Single blastTime;
        private String blueprintType;
        private Boolean active = true;

        public List<IEnemy> SpawnedEnemies { get; private set; }

        public Int32 MaxSpawned { get; private set; }

        internal ThirdBossBigGunSpawner(ThirdBossBigGunSpawnSpecification specification, ActorsFactory factory, Weapon bigWeapon, Single blastTime)
        {
            this.factory = factory;
            this.SpawnedEnemies = new List<IEnemy>();
            this.blueprintType = specification.BlueprintType;
            this.MaxSpawned = specification.MaxSpawned;
            this.blastTime = blastTime;

            bigWeapon.Shoot += (sender, e) =>
            {
                e.Bullet.BulletHit += ProcessSphereHit;
            };
        }

        public void DivideAliveAndDead(List<IEnemy> avengers)
        {
            SpawnedEnemies = EnemiesDeathProcessor.DivideAliveAndDead(SpawnedEnemies, avengers);
        }

        public void TurnOff()
        {
            active = false;
        }

        public void TurnOn()
        {
            active = true;
        }

        public void Update(Single elapsedSeconds)
        {
        }

        private void ProcessSphereHit(Object sender, EventArgs e)
        {
            Bullet bullet = sender as Bullet;
            if (active && SpawnedEnemies.Count < MaxSpawned)
            {
                var asi = new ActorStartInfo
                {
                    BlueprintType = blueprintType,
                    AppearancePhaseDuration = blastTime,
                    BehaviorParameters = new BehaviorParameters { },
                    Position = bullet.Position
                };
                SpawnedEnemies.Add(factory.ConstructEnemy(asi));
            }
            bullet.BulletHit -= ProcessSphereHit;
        }

        public void DespawnRoutine()
        {
            foreach (var enemy in SpawnedEnemies)
                enemy.Despawn();
        }
    }
}
