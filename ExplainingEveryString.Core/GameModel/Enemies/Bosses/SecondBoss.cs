using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class SecondBoss : Enemy<SecondBossBlueprint>
    {
        private Player player;
        private DeathZoneParameters deathZone;
        private SecondBossPowerKeepersSpecification powerKeepersMovementSpec;
        private CompositeSpawnedActorsController actorsController;
        private SpawnedActorsController deathZoneBorderActors;
        private SecondBossPowerKeepersSpawner powerKeepersActors;
        private SecondBossPhaseSpecification[] phases;
        private Int32 phasesPassed;

        private EllipticMovementControl deathZoneMovement;
        private EllipticMovementControl powerKeepersMovement;
        private Single timePassed = 0;

        public override CollidableMode CollidableMode => CollidableMode.Shadow;

        public override ISpawnedActorsController SpawnedActors => actorsController;

        protected override void Construct(SecondBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.player = level.Player;
            this.phases = blueprint.Phases;
            this.HitPoints = blueprint.PowerKeepersSpawner.MaxSpawned + blueprint.Phases.Select(phase => phase.PowerKeepersAmount).Sum();
            this.MaxHitPoints = HitPoints;

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
            this.deathZoneBorderActors = new SpawnedActorsController(blueprint.DeathZoneBorderSpawner, this, startInfo.BehaviorParameters, factory);
            this.deathZoneMovement = new EllipticMovementControl(this, deathZoneBorderActors, blueprint.DeathZonePatrolCycleTime, a, b);

            this.powerKeepersActors = new SecondBossPowerKeepersSpawner(blueprint.PowerKeepersSpawner, this, factory, PowerKeeperDied);
            this.powerKeepersMovementSpec = blueprint.PowerKeepersMovement;
            this.powerKeepersMovement = new EllipticMovementControl(this, powerKeepersActors, powerKeepersMovementSpec.PowerKeeperCycleTime, 
                powerKeepersMovementSpec.InnerBigHalfAxe, powerKeepersMovementSpec.InnerSmallHalfAxe);

            this.Behavior.SpawnedActors.TurnOff();
            this.actorsController = new CompositeSpawnedActorsController(Behavior.SpawnedActors, deathZoneBorderActors, powerKeepersActors);
            this.Died += SecondBoss_Died;
        }

        public override void Update(Single elapsedSeconds)
        {
            timePassed += elapsedSeconds;
            DamagingPlayerInDeathZone(elapsedSeconds);
            deathZoneMovement.MoveEnemiesInEllipse(elapsedSeconds, 1, 1);
            PowerKeepersMovement(elapsedSeconds);
            base.Update(elapsedSeconds);
            if (!IsInAppearancePhase && phasesPassed == 0)
                Behavior.SpawnedActors.TurnOff();
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

        private void PowerKeepersMovement(Single elapsedSeconds)
        {
            var heartBeatCycles = timePassed / powerKeepersMovementSpec.HeartBeatTime;
            var heartBeatCyclePart = heartBeatCycles - System.Math.Floor(heartBeatCycles);
            var expandCoeff = heartBeatCyclePart < 0.5 ? heartBeatCyclePart * 2 : (1 - heartBeatCyclePart) * 2;
            Single expandBigAxeBy = (Single)(1 + expandCoeff * powerKeepersMovementSpec.BigHalfAxeExpand);
            Single expandSmallAxeBy = (Single)(1 + expandCoeff * powerKeepersMovementSpec.SmallHalfAxeExpand);
            powerKeepersMovement.MoveEnemiesInEllipse(elapsedSeconds, expandBigAxeBy, expandSmallAxeBy);
        }

        private void SecondBoss_Died(Object sender, EventArgs e)
        {
            foreach (var enemy in deathZoneBorderActors.SpawnedEnemies)
                enemy.TakeDamage(Single.MaxValue);
            foreach (var enemy in Behavior.SpawnedActors.SpawnedEnemies)
                enemy.TakeDamage(Single.MaxValue);
        }

        private void PowerKeeperDied(Object sender, EventArgs e)
        {
            TakeDamage(1);
            if (!powerKeepersActors.SpawnedEnemies.Any(powerKeeper => powerKeeper.IsAlive()) 
                && powerKeepersActors.EveryoneSpawned && IsAlive())
            {
                phasesPassed += 1;
                var currentPhase = phases[phasesPassed - 1];
                ApplyPhaseChanges(currentPhase);
                this.powerKeepersActors.Reset();
            }
        }

        private void ApplyPhaseChanges(SecondBossPhaseSpecification currentPhase)
        {
            base.SpriteState = new Displaying.SpriteState(currentPhase.Sprite);
            this.Width = currentPhase.Width;
            this.Height = currentPhase.Height;
            this.powerKeepersMovementSpec = currentPhase.PowerKeepersMovement;
            this.powerKeepersMovement = new EllipticMovementControl(this, powerKeepersActors, powerKeepersMovementSpec.PowerKeeperCycleTime,
                powerKeepersMovementSpec.InnerBigHalfAxe, powerKeepersMovementSpec.InnerSmallHalfAxe);
            this.powerKeepersActors.MaxSpawned = currentPhase.PowerKeepersAmount;
            if (currentPhase.UseSupport)
                Behavior.SpawnedActors.TurnOn();
            else
                Behavior.SpawnedActors.TurnOff();
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

        private class EllipticMovementControl
        {
            private SecondBoss boss;
            private Single timePassed = 0;
            private Single cycleTime;
            private Single bigHalfAxe;
            private Single smallHalfAxe;
            private ISpawnedActorsController spawnedActorsController;
            private Dictionary<IEnemy, Int32> placeInElliplse = new Dictionary<IEnemy, Int32>();

            internal EllipticMovementControl(SecondBoss boss, ISpawnedActorsController spawnedActorsController, 
                Single cycleTime, Single bigHalfAxe, Single smallHalfAxe)
            {
                this.boss = boss;
                this.spawnedActorsController = spawnedActorsController;
                this.cycleTime = cycleTime;
                this.bigHalfAxe = bigHalfAxe;
                this.smallHalfAxe = smallHalfAxe;
            }

            internal void MoveEnemiesInEllipse(Single elapsedSeconds, Single xCoeff, Single yCoeff)
            {
                timePassed += elapsedSeconds;
                foreach (IEnemy patrol in spawnedActorsController.SpawnedEnemies)
                {
                    if (!placeInElliplse.ContainsKey(patrol))
                    {
                        var possiblePlaceInCircle = 0;
                        while (placeInElliplse.ContainsValue(possiblePlaceInCircle))
                            possiblePlaceInCircle += 1;
                        placeInElliplse.Add(patrol, possiblePlaceInCircle);
                        patrol.Died += Patrol_Died;
                    }
                    var i = placeInElliplse[patrol];
                    var patrolAngle = System.Math.PI * 2 * (1.0 / spawnedActorsController.MaxSpawned * i + timePassed / cycleTime);
                    var patrolX = (Single)(bigHalfAxe * xCoeff * System.Math.Cos(patrolAngle));
                    var patrolY = (Single)(smallHalfAxe * yCoeff * System.Math.Sin(patrolAngle));
                    (patrol as ICollidable).Position = boss.Position + new Vector2(patrolX, patrolY);
                }
            }

            private void Patrol_Died(Object sender, EventArgs e)
            {
                placeInElliplse.Remove(sender as IEnemy);
            }
        }
    }
}
