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
        internal Boolean IsHoming => bulletSpecification.HomingSpeed > 0;

        private readonly IAimer aimer;
        private readonly Level level;
        private readonly Func<Vector2> findOutWhereIAm;
        private readonly Func<IActor> targetSelector;
        private readonly BulletSpecification bulletSpecification;
        private readonly Vector2 baseOffset;
        private readonly Vector2 muzzleOffset;
        private readonly Single length;
        private readonly Single angleCorrection;
        private readonly Single accuracy;
        private readonly Int32 bulletsAtOnce;
        private readonly Single angleStep;
        private readonly Boolean keepAngleStep;
        private Single accuracyCorrection;

        internal Barrel(Level level, IAimer aimer, Func<Vector2> findOutWhereIAm, Func<IActor> targetSelector, BarrelSpecification specification)
        {
            this.baseOffset = specification.BaseOffset;
            this.muzzleOffset = specification.MuzzleOffset;
            this.length = specification.Length;
            this.aimer = aimer;
            this.level = level;
            this.bulletsAtOnce = specification.BulletsAtOnce;
            this.angleStep = AngleConverter.ToRadians(specification.AngleStep);
            this.keepAngleStep = specification.KeepAngleStep;
            this.findOutWhereIAm = findOutWhereIAm;
            this.targetSelector = targetSelector;
            this.bulletSpecification = specification.Bullet;
            this.angleCorrection = AngleConverter.ToRadians(specification.AngleCorrection);
            this.accuracy = AngleConverter.ToRadians(specification.Accuracy);
        }

        internal void OnShoot(Single bulletFirstFrameUpdateTime)
        {
            var barrelDirection = GetFireDirection(false);
            var position = findOutWhereIAm() + baseOffset + barrelDirection * length + GetMuzzleOffset();
            if (keepAngleStep)
                accuracyCorrection = (RandomUtility.Next() - 0.5F) * accuracy;
            foreach (var i in Enumerable.Range(0, bulletsAtOnce))
            {
                var direction = GetFireDirection(true, i * angleStep);
                var bullet = new Bullet(level, position, direction, bulletSpecification, targetSelector());
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

        private Vector2 GetFireDirection(Boolean includeInaccuracy, Single angleStepCorrection = 0)
        {
            var direction = aimer.GetFireDirection();
            var angle = AngleConverter.ToRadians(direction);
            angle += angleCorrection;
            angle += angleStepCorrection;
            if (accuracy != 0 && includeInaccuracy)
            {
                angle += keepAngleStep ? accuracyCorrection : (RandomUtility.Next() - 0.5F) * accuracy;
            }  
            direction = AngleConverter.ToVector(angle);
            return direction;
        }
    }
}
