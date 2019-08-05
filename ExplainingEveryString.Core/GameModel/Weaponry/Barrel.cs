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
        private Func<Vector2> findOutWhereIAm;
        private Func<Vector2> targetLocator;
        private BulletSpecification bulletSpecification;
        private Single length;
        private Single angleCorrection;
        private Single accuracy;

        internal Barrel(IAimer aimer, Func<Vector2> findOutWhereIAm, Func<Vector2> targetLocator, BarrelSpecification specification)
        {
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
            Vector2 direction = GetFireDirection();
            Vector2 position = findOutWhereIAm() + direction * length;
            Bullet bullet = new Bullet(position, direction, bulletSpecification, targetLocator);
            ShootEventArgs eventArgs = new ShootEventArgs
            {
                Bullet = bullet,
                FirstUpdateTime = bulletFirstFrameUpdateTime
            };
            Shoot?.Invoke(this, eventArgs);
        }

        private Vector2 GetFireDirection()
        {
            Vector2 direction = aimer.GetFireDirection();
            Single angle = AngleConverter.ToRadians(direction);
            if (angleCorrection != 0)
                angle += angleCorrection;
            if (accuracy != 0)
                angle += (RandomUtility.Next() - 0.5F) * accuracy;
            direction = AngleConverter.ToVector(angle);
            return direction;
        }
    }
}
