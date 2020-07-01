using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Reloader
    {
        private Single shootCooldown;
        private Single reloadTime;
        private Single reloadScatter;
        private Single timeTillNextShoot;
        private Single nextBulletFirstUpdateTime = 0;
        private Int32 maxAmmo;
        private Int32 currentAmmo;
        private Func<Boolean> isOn;
        private Action<Single> onReloadEnd;

        private Boolean AmmoLimited => maxAmmo > 1;

        internal Reloader(ReloaderSpecification specification, Func<Boolean> isOn, Action<Single> onReloadEnd)
        { 
            this.isOn = isOn;
            this.onReloadEnd = onReloadEnd;
            this.maxAmmo = specification.Ammo;
            this.shootCooldown = 1 / specification.FireRate;
            this.reloadTime = specification.ReloadTime;
            this.reloadScatter = specification.ReloadScatter;
            this.currentAmmo = 0;

            timeTillNextShoot = AmmoLimited ? reloadTime : shootCooldown;
        }

        internal void TryReload(Single elapsedSeconds, out Boolean weaponFired)
        {
            if (timeTillNextShoot > Constants.Epsilon)
                timeTillNextShoot -= elapsedSeconds;
            if (isOn())
            {
                weaponFired = false;
                if (timeTillNextShoot <= Constants.Epsilon)
                    nextBulletFirstUpdateTime += elapsedSeconds;
                while (timeTillNextShoot <= Constants.Epsilon)
                {
                    var betweenShoots = shootCooldown;
                    if (AmmoLimited)
                        ProcessReloadForLimitedAmmo(ref betweenShoots);

                    timeTillNextShoot += betweenShoots;
                    nextBulletFirstUpdateTime -= betweenShoots;
                    if (nextBulletFirstUpdateTime < -Constants.Epsilon)
                        nextBulletFirstUpdateTime = 0;
                    onReloadEnd(nextBulletFirstUpdateTime);
                    weaponFired = true;
                }
            }
            else
                weaponFired = false;
        }

        private void ProcessReloadForLimitedAmmo(ref Single betweenShoots)
        {
            var reloadAfterThisBullet = false;
            if (currentAmmo == 1)
                reloadAfterThisBullet = true;
            if (currentAmmo == 0)
                currentAmmo = this.maxAmmo;
            currentAmmo -= 1;
            if (reloadAfterThisBullet)
                betweenShoots = reloadTime + RandomUtility.Next(-reloadScatter, reloadScatter);
        }
    }
}
