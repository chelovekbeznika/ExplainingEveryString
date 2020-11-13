using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class PostMortemSurprise
    {
        private Barrel[] barrels;

        private Int32 howMuchToSpawn;
        private String avengerType;
        private ISpawnPositionSelector positionSelector;
        private ActorsFactory factory;
        private IMovableCollidable shooter;
        private Player player;

        private Boolean triggered = false;

        internal List<IEnemy> Avengers { get; private set; }
        private Boolean FiresWeapon => barrels != null;
        private Boolean SpawnsEnemies => avengerType != null;

        internal PostMortemSurprise(PostMortemSurpriseSpecification specification, IMovableCollidable shooter, 
            Player player, Level level, ActorsFactory factory)
        {
            this.factory = factory;
            this.shooter = shooter;
            this.player = player;
            if (specification.Weapon != null)
            {
                InitializeWeapon(specification.Weapon, level);
            }
            if (specification.Spawn != null)
            {
                InitializeSpawn(specification.Spawn);
            }
        }

        private void InitializeWeapon(PostMortemWeaponSpecification specification, Level level)
        {
            var aimer = AimersFactory.Get(specification.AimType, 0, shooter, () => player.Position);
            barrels = specification.Barrels
                .Select(bs => new Barrel(level, aimer, () => shooter.Position, () => player, bs)).ToArray();
            foreach (var barrel in barrels)
                barrel.Shoot += level.EnemyShoot;
        }

        private void InitializeSpawn(PostMortemSpawnSpecificaton specification)
        {
            howMuchToSpawn = specification.AvengersAmount;
            avengerType = specification.AvengersType;
            positionSelector = SpawnPositionSelectorsFactory.Get(specification.PositionSelector, null);
        }

        internal void TryTrigger()
        {
            if (!triggered)
            {
                if (FiresWeapon)
                    TriggerWeapon();
                if (SpawnsEnemies)
                    SpawnAvengers();
                triggered = true;
            }
        }

        private void TriggerWeapon()
        {
            if (barrels != null)
            {
                foreach (var barrel in barrels)
                    barrel.OnShoot(0);
            }
        }

        private void SpawnAvengers()
        {
            Avengers = new List<IEnemy>();
            foreach (var index in Enumerable.Range(0, howMuchToSpawn))
            {
                var spawnSpecification = positionSelector.GetNextSpawnSpecification();
                var asi = new ActorStartInfo
                {
                    BlueprintType = avengerType,
                    Position = spawnSpecification.SpawnPoint + shooter.Position,
                    BehaviorParameters = new BehaviorParameters
                    {
                        TrajectoryParameters = spawnSpecification.TrajectoryParameters?.ToArray(),
                        Angle = spawnSpecification.Angle
                    }
                };
                var enemy = factory.ConstructEnemy(asi);
                Avengers.Add(enemy);
            }
        }

        internal void Cancel()
        {
            triggered = true;
        }
    }
}
