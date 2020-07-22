using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Barrel
    {
        internal event EventHandler<ShootEventArgs> Shoot;

        private IAimer aimer;
        private readonly Func<Vector2> findOutWhereIAm;
        private readonly Func<Vector2> targetLocator;
        private readonly BulletSpecification bulletSpecification;
        private readonly Vector2 baseOffset;
        private readonly Vector2 muzzleOffset;
        private readonly Single length;
        private readonly Single angleCorrection;
        private readonly Single accuracy;
        private readonly Int32 bulletsAtOnce;

        internal Barrel(IAimer aimer, Func<Vector2> findOutWhereIAm, Func<Vector2> targetLocator, BarrelSpecification specification)
        {
            this.baseOffset = specification.BaseOffset;
            this.muzzleOffset = specification.MuzzleOffset;
            this.length = specification.Length;
            this.aimer = aimer;
            this.bulletsAtOnce = specification.BulletsAtOnce;
            this.findOutWhereIAm = findOutWhereIAm;
            this.targetLocator = targetLocator;
            this.bulletSpecification = specification.Bullet;
            this.angleCorrection = AngleConverter.ToRadians(specification.AngleCorrection);
            this.accuracy = AngleConverter.ToRadians(specification.Accuracy);
        }

        internal void OnShoot(Single bulletFirstFrameUpdateTime)
        {
            var barrelDirection = aimer.GetFireDirection();
            var position = findOutWhereIAm() + baseOffset + barrelDirection * length + GetMuzzleOffset();
            foreach (var i in Enumerable.Range(0, bulletsAtOnce))
            {
                var direction = GetFireDirection();
                var bullet = new Bullet(position, direction, bulletSpecification, targetLocator);
                var eventArgs = new ShootEventArgs
                {
                    Bullet = bullet,
                    FirstUpdateTime = bulletFirstFrameUpdateTime
                };
                Shoot?.Invoke(this, eventArgs);
            }
        }

        private Vector2 GetMuzzleOffset()
        {
            var direction = aimer.GetFireDirection();
            return GeometryHelper.RotateVector(muzzleOffset, direction.Y, direction.X);
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
