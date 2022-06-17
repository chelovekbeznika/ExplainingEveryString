using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Movement;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class EnemyBehavior : IEnemyBehavior
    {
        private const Int32 MaxMoveTargetsPerFrame = 32;

        private IEnemy enemy;
        private IMoveTargetSelector moveTargetSelector;
        private IMover mover;

        public EventHandler MoveGoalReached { get; set; }
        public Weapon Weapon { get; private set; }
        public PostMortemSurprise PostMortemSurprise { get; private set; }
        public ISpawnedActorsController SpawnedActors { get; private set; }
        public Single? EnemyAngle { get; private set; } = null;
        public Boolean IsTeleporter => mover.IsTeleporting;

        private Vector2 CurrentPositionLocator() => (enemy as ICollidable).Position;
        private Vector2 Position { get => (enemy as ICollidable).Position; set => (enemy as ICollidable).Position = value; }
        private Func<Vector2> playerLocator;
        private Player player;
        private BehaviorParameters parameters;

        internal EnemyBehavior(IEnemy enemy, Player player, BehaviorParameters parameters)
        {
            this.enemy = enemy;
            this.player = player;
            this.playerLocator = () => player.Position;
            this.parameters = parameters;
        }

        public void Construct(EnemyBehaviorSpecification specification, Level level, ActorsFactory factory)
        {
            ConstructMovement(specification, level);
            ConstructWeaponry(specification, level, factory);
        }

        private void ConstructMovement(EnemyBehaviorSpecification specification, Level level)
        {
            this.moveTargetSelector = level.MoveTargetSelectorFactory.Get(
                specification.MoveTargetSelectType, parameters.TrajectoryParameters, player, enemy);
            ChangeMover(specification.Mover);
        }

        private void ConstructWeaponry(EnemyBehaviorSpecification specification, Level level, ActorsFactory factory)
        {
            ChangeWeapon(specification.Weapon, level);
            if (specification.PostMortemSurprise != null)
            {
                PostMortemSurprise = new PostMortemSurprise(specification.PostMortemSurprise, enemy,
                    player, level, factory);
            }
            if (specification.Spawner != null)
                this.SpawnedActors = new SpawnedActorsController(specification.Spawner, enemy, parameters, factory);
        }

        public void Update(Single elapsedSeconds)
        {
            Move(elapsedSeconds);
            UseWeapon(elapsedSeconds);
        }

        public IEnumerable<IDisplayble> GetPartsToDisplay()
        {
            if (Weapon != null)
                return new IDisplayble[] { Weapon };
            else
                return Enumerable.Empty<IDisplayble>();
        }

        internal void ChangeWeapon(WeaponSpecification specification, Level level)
        {
            if (Weapon != null)
                Weapon.Shoot -= level.EnemyShoot;
            if (specification != null)
            {
                var aimer = AimersFactory.Get(
                    specification.AimType, parameters.Angle, enemy, playerLocator, this);
                Weapon = new Weapon(specification, aimer, CurrentPositionLocator, () => player, level, false);
                Weapon.Shoot += level.EnemyShoot;
            }
        }

        internal void ChangeMover(MoverSpecification specification)
        {
            this.mover = MoverFactory.Get(specification);
        }

        private void Move(Single elapsedSeconds)
        {
            var remainedTime = elapsedSeconds;
            var targetsHitted = 0;
            while (remainedTime > 0 && targetsHitted < MaxMoveTargetsPerFrame)
            {
                var target = moveTargetSelector.GetTarget();
                var lineToTarget = target - Position;
                var positionChange = mover.GetPositionChange(lineToTarget, ref remainedTime);
                Position += positionChange;
                if (remainedTime > 0)
                {
                    targetsHitted += 1;
                    moveTargetSelector.SwitchToNextTarget();
                    MoveGoalReached?.Invoke(enemy, EventArgs.Empty);
                }
            }
        }

        private void UseWeapon(Single elapsedSeconds)
        {
            if (Weapon != null)
            {
                Weapon.Update(elapsedSeconds);
                if (Weapon.IsFiring() && !Weapon.IsVisible)
                    EnemyAngle = AngleConverter.ToRadians(Weapon.GetFireDirection());
                else
                    EnemyAngle = null;
            }
        }
    }
}
