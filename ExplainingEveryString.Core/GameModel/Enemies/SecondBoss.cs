using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class SecondBoss : Enemy<SecondBossBlueprint>
    {
        private Player player;
        private DeathZoneParameters deathZone;
        private SecondBossPowerKeepersSpecification powerKeepersMovement;
        private CompositeSpawnedActorsController actorsController;
        private SpawnedActorsController deathZoneBorderActors;
        private OneTimeSpawnedActorsController powerKeepersActors;

        private Single patrolCycleTime;
        private Single timePassed;

        public override ISpawnedActorsController SpawnedActors => actorsController;

        protected override void Construct(SecondBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.player = level.Player;
            var a = blueprint.DeathEllipseX;
            var b = blueprint.DeathEllipseY;
            this.deathZone = new DeathZoneParameters
            {
                FocusRadius = (Single)System.Math.Sqrt(a * a - b * b),
                BigHalfAxe = a,
                SmallHalfAxe = b,
                FocusDistanceSum = 2 * a,
                Damage = blueprint.DeathZoneDamage,
                TimeSpent = 0
            };
            this.patrolCycleTime = blueprint.DeathZonePatrolCycleTime;
            this.deathZoneBorderActors = new SpawnedActorsController(blueprint.DeathZoneBorderSpawner, this, startInfo.BehaviorParameters, factory);
            this.powerKeepersActors = new OneTimeSpawnedActorsController(blueprint.PowerKeepersSpawner, this, factory);
            this.powerKeepersMovement = blueprint.PowerKeepersMovement;
            this.actorsController = new CompositeSpawnedActorsController(Behavior.SpawnedActors, deathZoneBorderActors, powerKeepersActors);
        }

        public override void Update(Single elapsedSeconds)
        {
            timePassed += elapsedSeconds;
            DamagingPlayerInDeathZone(elapsedSeconds);
            DeathZonePatrolMovement(elapsedSeconds);
            PowerKeepersMovement(elapsedSeconds);
            base.Update(elapsedSeconds);
        }

        private void DamagingPlayerInDeathZone(Single elapsedSeconds)
        {
            var focus1 = Position + new Vector2(-deathZone.FocusRadius, 0);
            var focus2 = Position + new Vector2(deathZone.FocusRadius, 0);
            var playerInDeathZone = (player.Position - focus1).Length() + (player.Position - focus2).Length() > deathZone.FocusDistanceSum;

            if (playerInDeathZone)
            {
                deathZone.TimeSpent += elapsedSeconds;
                player.TakeDamageSoftly(deathZone.CurrentFrameDamage(elapsedSeconds));
            }
            else
                deathZone.TimeSpent = 0;
        }

        private void DeathZonePatrolMovement(Single elapsedSeconds)
        {
            for (var i = 0; i < deathZoneBorderActors.SpawnedEnemies.Count; i++)
            {
                ICollidable patrol = deathZoneBorderActors.SpawnedEnemies[i];
                var patrolCount = deathZoneBorderActors.Specification.MaxSpawned;
                var patrolAngle = System.Math.PI * 2 * (1.0 / patrolCount * i + timePassed / patrolCycleTime);
                var patrolX = (Single)(deathZone.BigHalfAxe * System.Math.Cos(patrolAngle));
                var patrolY = (Single)(deathZone.SmallHalfAxe * System.Math.Sin(patrolAngle));
                patrol.Position = Position + new Vector2(patrolX, patrolY);
            }
        }

        private void PowerKeepersMovement(Single elapsedSeconds)
        {
            var heartBeatCycles = timePassed / powerKeepersMovement.HeartBeatTime;
            var heartBeatCyclePart = heartBeatCycles - System.Math.Floor(heartBeatCycles);
            var expandCoeff = heartBeatCyclePart < 0.5 ? heartBeatCyclePart * 2 : (1 - heartBeatCyclePart) * 2;
            var expandBigAxeBy = 1 + expandCoeff * powerKeepersMovement.BigHalfAxeExpand;
            var expandSmallAxeBy = 1 + expandCoeff * powerKeepersMovement.SmallHalfAxeExpand;
            for (var i = 0; i < powerKeepersActors.SpawnedEnemies.Count; i++)
            {
                ICollidable powerKeeper = powerKeepersActors.SpawnedEnemies[i];
                var powerKeepersCount = powerKeepersActors.Specification.MaxSpawned;
                var powerKeeperAngle = System.Math.PI * 2 * (1.0 / powerKeepersCount * i + timePassed / powerKeepersMovement.PowerKeeperCycleTime);
                var powerKeeperX = (Single)(powerKeepersMovement.InnerBigHalfAxe * expandBigAxeBy * System.Math.Cos(powerKeeperAngle));
                var powerKeeperY = (Single)(powerKeepersMovement.InnerSmallHalfAxe * expandSmallAxeBy * System.Math.Sin(powerKeeperAngle));
                powerKeeper.Position = Position + new Vector2(powerKeeperX, powerKeeperY);
            }
        }

        private class DeathZoneParameters
        {
            internal Single FocusRadius { get; set; }
            internal Single BigHalfAxe { get; set; }
            internal Single SmallHalfAxe { get; set; }
            internal Single FocusDistanceSum { get; set; }
            internal Single Damage { get; set; }
            internal Single TimeSpent { get; set; }

            internal Single CurrentFrameDamage(Single elapsedSeconds) =>
                Damage * (TimeSpent * TimeSpent - (TimeSpent - elapsedSeconds) * (TimeSpent - elapsedSeconds));
        }
    }
}
