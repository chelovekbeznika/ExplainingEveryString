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

        internal Barrel(IAimer aimer, Func<Vector2> findOutWhereIAm, 
            BulletSpecification bulletSpecification, Single length)
        {
            this.length = length;
            this.aimer = aimer;
            this.findOutWhereIAm = findOutWhereIAm;
            this.bulletSpecification = bulletSpecification;
        }

        internal void OnShoot(Single bulletFirstFrameUpdateTime)
        {
            Vector2 direction = aimer.GetFireDirection();
            Vector2 position = findOutWhereIAm() + direction * length;
            Bullet bullet = new Bullet(position, direction, bulletSpecification);
            ShootEventArgs eventArgs = new ShootEventArgs
            {
                Bullet = bullet,
                FirstUpdateTime = bulletFirstFrameUpdateTime
            };
            Shoot?.Invoke(this, eventArgs);
        }
    }
}
