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
    internal class EnemyBehavior
    {
        private const Int32 MaxMoveTargetsPerFrame = 32;

        private IEnemy enemy;
        private Weapon weapon;      
        private IMoveTargetSelector moveTargetSelector;
        private IMover mover;

        internal EventHandler MoveGoalReached;

        internal PostMortemSurprise PostMortemSurprise { get; private set; }
        internal SpawnedActorsController SpawnedActors { get; private set; }
        internal Single? EnemyAngle { get; private set; } = null;
        internal Boolean IsTeleporter => mover.IsTeleporting;

        private Vector2 CurrentPositionLocator() => (enemy as ICollidable).Position;
        private Vector2 Position { get => (enemy as ICollidable).Position; set => (enemy as ICollidable).Position = value; }
        private Func<Vector2> playerLocator;

        internal EnemyBehavior(IEnemy enemy, Func<Vector2> playerLocator)
        {
            this.enemy = enemy;
            this.playerLocator = playerLocator;
        }

        internal void Construct(EnemyBehaviorSpecification specification, BehaviorParameters parameters, Level level, ActorsFactory factory)
        {
            ConstructMovement(specification, parameters);
            ConstructWeaponry(specification, parameters, level, factory);
        }

        private void ConstructMovement(EnemyBehaviorSpecification specification, BehaviorParameters parameters)
        {
            this.moveTargetSelector = MoveTargetSelectorFactory.Get(
                specification.MoveTargetSelectType, parameters.TrajectoryParameters, playerLocator, enemy);
            this.mover = MoverFactory.Get(specification.Mover);
        }

        private void ConstructWeaponry(EnemyBehaviorSpecification specification, BehaviorParameters parameters, Level level, ActorsFactory factory)
        {
            if (specification.Weapon != null)
            {
                var aimer = AimersFactory.Get(
                    specification.Weapon.AimType, parameters.Angle, CurrentPositionLocator, playerLocator);
                weapon = new Weapon(specification.Weapon, aimer, CurrentPositionLocator, playerLocator, level, false);
                weapon.Shoot += level.EnemyShoot;
            }
            if (specification.PostMortemSurprise != null)
            {
                PostMortemSurprise = new PostMortemSurprise(specification.PostMortemSurprise, CurrentPositionLocator,
                    playerLocator, level, factory);
            }
            if (specification.Spawner != null)
                this.SpawnedActors = new SpawnedActorsController(specification.Spawner, enemy, parameters, factory);
        }

        internal void Update(Single elapsedSeconds)
        {
            Move(elapsedSeconds);
            UseWeapon(elapsedSeconds);
        }

        internal IEnumerable<IDisplayble> GetPartsToDisplay()
        {
            if (weapon != null)
                return new IDisplayble[] { weapon };
            else
                return Enumerable.Empty<IDisplayble>();
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
            if (weapon != null)
            {
                weapon.Update(elapsedSeconds);
                if (weapon.IsFiring() && !weapon.IsVisible)
                    EnemyAngle = AngleConverter.ToRadians(weapon.GetFireDirection());
                else
                    EnemyAngle = null;
            }
        }
    }
}
