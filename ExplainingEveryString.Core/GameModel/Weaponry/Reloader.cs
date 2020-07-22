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
        internal Int32 MaxAmmo { get; private set; }
        internal Int32 CurrentAmmo { get; private set; }
        private Func<Boolean> isOn;
        private Action<Single> onReloadEnd;

        internal Boolean AmmoLimited => MaxAmmo > 1;

        internal Reloader(ReloaderSpecification specification, Func<Boolean> isOn, Action<Single> onReloadEnd, Boolean fullAmmoAtStart = false)
        { 
            this.isOn = isOn;
            this.onReloadEnd = onReloadEnd;
            this.MaxAmmo = specification.Ammo;
            this.shootCooldown = 1 / specification.FireRate;
            this.reloadTime = specification.ReloadTime;
            this.reloadScatter = specification.ReloadScatter;
            this.CurrentAmmo = fullAmmoAtStart ? this.MaxAmmo : 0;

            timeTillNextShoot = AmmoLimited ? reloadTime : shootCooldown;
        }

        internal void TryReload(Single elapsedSeconds, out Boolean weaponFired)
        {
            if (timeTillNextShoot > Constants.Epsilon)
                timeTillNextShoot -= elapsedSeconds;
            else if (CurrentAmmo == 0)
                CurrentAmmo = this.MaxAmmo;
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
            if (CurrentAmmo == 1)
                reloadAfterThisBullet = true;
            if (CurrentAmmo == 0)
                CurrentAmmo = this.MaxAmmo;
            CurrentAmmo -= 1;
            if (reloadAfterThisBullet)
                betweenShoots = reloadTime + RandomUtility.Next(-reloadScatter, reloadScatter);
        }
    }
}
