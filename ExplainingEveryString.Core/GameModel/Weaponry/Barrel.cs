using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Barrel
    {
        internal event EventHandler<ShootEventArgs> Shoot;

        private IAimer aimer;
        private readonly Func<Vector2> findOutWhereIAm;
        private readonly Func<Vector2> targetLocator;
        private readonly BulletSpecification bulletSpecification;
        private readonly Vector2 offset;
        private readonly Single length;
        private readonly Single angleCorrection;
        private readonly Single accuracy;

        internal Barrel(IAimer aimer, Func<Vector2> findOutWhereIAm, Func<Vector2> targetLocator, BarrelSpecification specification)
        {
            this.offset = specification.Offset;
            this.length = specification.Length;
            this.aimer = aimer;
            this.findOutWhereIAm = findOutWhereIAm;
            this.targetLocator = targetLocator;
            this.bulletSpecification = specification.Bullet;
            this.angleCorrection = AngleConverter.ToRadians(specification.AngleCorrection);
            this.accuracy = AngleConverter.ToRadians(specification.Accuracy);
        }

        internal void OnShoot(Single bulletFirstFrameUpdateTime)
        {
            var direction = GetFireDirection();
            var position = findOutWhereIAm() + offset + direction * length;
            var bullet = new Bullet(position, direction, bulletSpecification, targetLocator);
            var eventArgs = new ShootEventArgs
            {
                Bullet = bullet,
                FirstUpdateTime = bulletFirstFrameUpdateTime
            };
            Shoot?.Invoke(this, eventArgs);
        }

        private Vector2 GetFireDirection()
        {
            var direction = aimer.GetFireDirection();
            var angle = AngleConverter.ToRadians(direction);
            if (angleCorrection != 0)
                angle += angleCorrection;
            if (accuracy != 0)
                angle += (RandomUtility.Next() - 0.5F) * accuracy;
            direction = AngleConverter.ToVector(angle);
            return direction;
        }
    }
}
