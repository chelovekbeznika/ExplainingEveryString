using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry.Trajectories;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Bullet : IDisplayble, IUpdateable
    {
        private static TrajectoryFactory trajectoryFactory = new TrajectoryFactory();

        private readonly Single timeToLive;
        private Single homingSpeed;
        private readonly IActor target;
        private readonly EpicEvent hit;

        private Single bulletAge = 0;
        private Boolean alive = true;
        private BulletTrajectory trajectory;
        private Boolean considerAngle;
        private Single prematureBlastInterval;

        public SpriteState SpriteState { get; private set; }
        public IEnumerable<IDisplayble> GetParts() => Enumerable.Empty<IDisplayble>();
        internal Vector2 CollisionCheckPosition { get; private set; }
        public Vector2 Position { get; private set; }
        internal Vector2 OldPosition { get; private set; }
        internal Single Damage { get; private set; }
        internal Single BlastWaveRadius { get; private set; }
        internal Boolean IsBlastsBefore => prematureBlastInterval > 0;
        public Boolean IsVisible => IsAlive();

        internal event EventHandler BulletHit;

        private Boolean IsHoming => target != null && homingSpeed > 0;

        internal Bullet(Level level, Vector2 position, Vector2 fireDirection, 
            BulletSpecification specification, IActor target)
        {
            this.SpriteState = new SpriteState(specification.Sprite);
            this.Position = position;
            this.OldPosition = position;
            this.Damage = specification.Damage;
            this.BlastWaveRadius = specification.BlastWaveRadius;
            this.prematureBlastInterval = specification.PrematureBlastInterval;
            this.timeToLive = specification.TimeToLive;
            this.considerAngle = specification.ConsiderAngle;
            this.trajectory = trajectoryFactory.GetTrajectory(specification.TrajectoryType,
                position, fireDirection, specification.TrajectoryParameters);
            this.target = target;
            this.homingSpeed = AngleConverter.ToRadians(specification.HomingSpeed);
            this.hit = new EpicEvent(level, specification.HitEffect, true, this, true);
        }

        public void Update(Single elapsedSeconds)
        {
            if (IsHoming)
            {
                if (target.IsAlive())
                {
                    trajectory.FireDirection = CorrectFireDirection(trajectory.FireDirection, elapsedSeconds);
                    trajectory.StartPosition = Position;
                }
                else
                    homingSpeed = 0;
            }
                
            bulletAge += elapsedSeconds;
            OldPosition = Position;
            var effectiveBulletTime = !IsHoming ? bulletAge : elapsedSeconds;
            Position = trajectory.GetBulletPosition(effectiveBulletTime);
            CollisionCheckPosition = prematureBlastInterval == 0 
                ? Position : trajectory.GetBulletPosition(effectiveBulletTime + prematureBlastInterval);
            if (considerAngle)
                SpriteState.Angle = AngleConverter.ToRadians(Position - OldPosition);
            SpriteState.Update(elapsedSeconds);
        }

        public void RegisterCollision()
        {
            if (alive)
            {
                alive = false;
                hit.TryHandle();
                BulletHit?.Invoke(this, EventArgs.Empty);
            }
        }

        public Boolean IsAlive()
        {
            return alive && bulletAge <= timeToLive;
        }

        private Vector2 CorrectFireDirection(Vector2 fireDirection, Single elapsedSeconds)
        {
            var directionToTarget = (target as ICollidable).Position - Position;
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
