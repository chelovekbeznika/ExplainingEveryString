using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Barrel
    {
        internal event EventHandler<ShootEventArgs> Shoot;

        private IAimer aimer;
        private Func<Vector2> findOutWhereIAm;
        private BulletSpecification bulletSpecification;
        private Single length;
        private Single angleCorrection;

        internal Barrel(IAimer aimer, Func<Vector2> findOutWhereIAm, BarrelSpecification specification)
        {
            this.length = specification.Length;
            this.aimer = aimer;
            this.findOutWhereIAm = findOutWhereIAm;
            this.bulletSpecification = specification.Bullet;
            this.angleCorrection = AngleConverter.ToRadians(specification.AngleCorrection);
        }

        internal void OnShoot(Single bulletFirstFrameUpdateTime)
        {
            Vector2 direction = GetFireDirection();
            Vector2 position = findOutWhereIAm() + direction * length;
            Bullet bullet = new Bullet(position, direction, bulletSpecification);
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
            if (angleCorrection != 0)
            {
                Single resultAngle = AngleConverter.ToRadians(direction);
                resultAngle += angleCorrection;
                direction = AngleConverter.ToVector(resultAngle);
            }
            return direction;
        }
    }
}
