using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry.Trajectories;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Bullet : IDisplayble, IUpdatable
    {
        private static TrajectoryFactory trajectoryFactory = new TrajectoryFactory();

        private Single timeToLive;
        private Single bulletAge = 0;
        private Boolean alive = true;
        private Single homingSpeed;
        private Func<Vector2> targetLocator;
        private BulletTrajectory trajectory;

        public SpriteState SpriteState { get; private set; }
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
            this.trajectory = trajectoryFactory.GetTrajectory(specification.TrajectoryType,
                position, fireDirection, specification.TrajectoryParameters);
            this.targetLocator = targetLocator;
            this.homingSpeed = AngleConverter.ToRadians(specification.HomingSpeed);
        }

        public void Update(Single elapsedSeconds)
        {
            if (IsHoming)
                trajectory.FireDirection = CorrectFireDirection(trajectory.FireDirection, elapsedSeconds);
            OldPosition = Position;
            bulletAge += elapsedSeconds;
            Position = trajectory.GetBulletPosition(bulletAge);
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
            Vector2 directionToTarget = targetLocator() - Position;
            Single targetAngle = AngleConverter.ToRadians(directionToTarget);
            Single currentAngle = AngleConverter.ToRadians(fireDirection);
            Single arcToTarget = AngleConverter.ClosestArc(currentAngle, targetAngle);
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
