using System;
using System.Collections.Generic;
using System.Linq;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry.Trajectories;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Bullet : IDisplayble, IUpdateable
    {
        private static TrajectoryFactory trajectoryFactory = new TrajectoryFactory();

        private readonly Single timeToLive;
        private readonly Single homingSpeed;
        private readonly Func<Vector2> targetLocator;

        private Single bulletAge = 0;
        private Boolean alive = true;
        private BulletTrajectory trajectory;
        private Boolean considerAngle;

        public SpriteState SpriteState { get; private set; }
        public IEnumerable<IDisplayble> GetParts() => Enumerable.Empty<IDisplayble>();
        public Vector2 Position { get; private set; }
        internal Vector2 OldPosition { get; private set; }
        internal Single Damage { get; private set; }

        private Boolean IsHoming => targetLocator != null && homingSpeed > 0;
        public Boolean IsVisible => IsAlive();

        internal Bullet(Vector2 position, Vector2 fireDirection, 
            BulletSpecification specification, Func<Vector2> targetLocator)
        {
            this.SpriteState = new SpriteState(specification.Sprite);
            this.Position = position;
            this.OldPosition = position;
            this.Damage = specification.Damage;
            this.timeToLive = specification.TimeToLive;
            this.considerAngle = specification.ConsiderAngle;
            this.trajectory = trajectoryFactory.GetTrajectory(specification.TrajectoryType,
                position, fireDirection, specification.TrajectoryParameters);
            this.targetLocator = targetLocator;
            this.homingSpeed = AngleConverter.ToRadians(specification.HomingSpeed);
        }

        public void Update(Single elapsedSeconds)
        {
            if (IsHoming)
                trajectory.FireDirection = CorrectFireDirection(trajectory.FireDirection, elapsedSeconds);
            bulletAge += elapsedSeconds;
            OldPosition = Position;
            Position = trajectory.GetBulletPosition(bulletAge);
            if (considerAngle)
                SpriteState.Angle = AngleConverter.ToRadians(Position - OldPosition);
            SpriteState.Update(elapsedSeconds);
        }

        public void RegisterCollision()
        {
            alive = false;
        }

        public Boolean IsAlive()
        {
            return alive && bulletAge <= timeToLive;
        }

        private Vector2 CorrectFireDirection(Vector2 fireDirection, Single elapsedSeconds)
        {
            var directionToTarget = targetLocator() - Position;
            var targetAngle = AngleConverter.ToRadians(directionToTarget);
            var currentAngle = AngleConverter.ToRadians(fireDirection);
            var arcToTarget = AngleConverter.ClosestArc(currentAngle, targetAngle);
            Single resultAngle;
            if (System.Math.Abs(arcToTarget) < homingSpeed * elapsedSeconds)
                resultAngle = targetAngle;
            else if (arcToTarget > 0)
                resultAngle = currentAngle + homingSpeed * elapsedSeconds;
            else
                resultAngle = currentAngle - homingSpeed * elapsedSeconds;
            return AngleConverter.ToVector(resultAngle);
        }
    }
}
