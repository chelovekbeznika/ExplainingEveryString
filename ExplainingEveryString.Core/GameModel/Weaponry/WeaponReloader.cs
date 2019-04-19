using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class WeaponReloader
    {
        private Single shootCooldown;
        private Single timeTillNextShoot;
        private Single nextBulletFirstUpdateTime = 0;
        private IAimer aimer;
        private Action<Single> onShoot;

        internal WeaponReloader(WeaponSpecification blueprint, IAimer aimer, Action<Single> onShoot)
        {
            shootCooldown = 1 / blueprint.FireRate;
            timeTillNextShoot = shootCooldown;
            this.aimer = aimer;
            this.onShoot = onShoot;
        }

        internal void TryReload(Single elapsedSeconds, out Boolean weaponFired)
        {
            if (timeTillNextShoot > Constants.Epsilon)
                timeTillNextShoot -= elapsedSeconds;
            if (aimer.IsFiring())
            {
                weaponFired = false;
                nextBulletFirstUpdateTime += elapsedSeconds;
                while (timeTillNextShoot <= Constants.Epsilon)
                {
                    timeTillNextShoot += shootCooldown;
                    nextBulletFirstUpdateTime -= shootCooldown;
                    if (nextBulletFirstUpdateTime < -Constants.Epsilon)
                        nextBulletFirstUpdateTime = 0;
                    onShoot(nextBulletFirstUpdateTime);
                    weaponFired = true;
                }
            }
            else
                weaponFired = false;
        }
    }
}
