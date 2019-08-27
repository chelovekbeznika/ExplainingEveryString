using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class PostMortemSurprise
    {
        private Barrel[] barrels;

        private Int32 howMuchToSpawn;
        private String avengerType;
        private ISpawnPositionSelector positionSelector;
        private Vector2[] levelSpawnPoints;
        private ActorsFactory factory;

        private Boolean triggered = false;

        internal List<IEnemy> Avengers { get; private set; }
        private Boolean FiresWeapon => barrels != null;
        private Boolean SpawnsEnemies => avengerType != null;

        internal PostMortemSurprise(PostMortemSurpriseSpecification specification, Func<Vector2> currentPositionLocator, 
            Func<Vector2> playerLocator, Level level, Vector2[] levelSpawnPoints, ActorsFactory factory)
        {
            this.levelSpawnPoints = levelSpawnPoints;
            this.factory = factory;
            if (specification.Weapon != null)
            {
                InitializeWeapon(specification.Weapon, currentPositionLocator, playerLocator, level);
            }
            if (specification.Spawn != null)
            {
                InitializeSpawn(specification.Spawn, currentPositionLocator);
            }
        }

        private void InitializeWeapon(PostMortemWeaponSpecification specification,
            Func<Vector2> currentPositionLocator, Func<Vector2> playerLocator, Level level)
        {
            IAimer aimer = AimersFactory.Get(specification.AimType, 0, currentPositionLocator, playerLocator);
            barrels = specification.Barrels
                .Select(bs => new Barrel(aimer, currentPositionLocator, playerLocator, bs)).ToArray();
            foreach (Barrel barrel in barrels)
                barrel.Shoot += level.EnemyShoot;
        }

        private void InitializeSpawn(PostMortemSpawnSpecificaton specification, Func<Vector2> currentPositionLocator)
        {
            howMuchToSpawn = specification.AvengersAmount;
            avengerType = specification.AvengersType;
            positionSelector = SpawnPositionSelectorsFactory.Get(
                specification.PositionSelector, currentPositionLocator, levelSpawnPoints);
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
                foreach (Barrel barrel in barrels)
                    barrel.OnShoot(0);
            }
        }

        private void SpawnAvengers()
        {
            Avengers = new List<IEnemy>();
            foreach (Int32 index in Enumerable.Range(0, howMuchToSpawn))
            {
                ActorStartInfo asi = new ActorStartInfo
                {
                    BlueprintType = avengerType,
                    Position = positionSelector.GetNextSpawnPosition(),
                    LevelSpawnPoints = levelSpawnPoints
                };
                IEnemy enemy = factory.ConstructEnemy(asi);
                Avengers.Add(enemy);
            }
        }

        internal void Cancel()
        {
            triggered = true;
        }
    }
}
