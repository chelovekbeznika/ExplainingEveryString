﻿using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Reloader
    {
        private Single shootCooldown;
        private Single reloadTime;
        private Single timeTillNextShoot;
        private Single nextBulletFirstUpdateTime = 0;
        private Int32? ammoStock;
        internal Int32 MaxAmmo { get; private set; }
        internal Int32 CurrentAmmo { get; private set; }
        private Func<Boolean> isOn;
        private Action<Single> onReloadEnd;

        internal Boolean AmmoLimited => MaxAmmo > 1;
        internal Boolean HasAmmo => ammoStock is null || ammoStock > 0;

        internal Reloader(ReloaderSpecification specification, Func<Boolean> isOn, Action<Single> onReloadEnd, Boolean fullAmmoAtStart = false, Int32? ammoStock = null)
        { 
            this.isOn = isOn;
            this.onReloadEnd = onReloadEnd;
            this.MaxAmmo = specification.Ammo;
            this.shootCooldown = 1 / specification.FireRate;
            this.reloadTime = specification.ReloadTime;
            if (fullAmmoAtStart)
                Reload();
            else
                CurrentAmmo = 0;

            timeTillNextShoot = AmmoLimited ? reloadTime : shootCooldown;
        }

        internal void TryReload(Single elapsedSeconds, out Boolean weaponFired)
        {
            if (timeTillNextShoot > Constants.Epsilon)
                timeTillNextShoot -= elapsedSeconds;
            else if (CurrentAmmo == 0)
                Reload();
            if (isOn() && HasAmmo)
            {
                weaponFired = false;
                if (timeTillNextShoot <= Constants.Epsilon)
                    nextBulletFirstUpdateTime += elapsedSeconds;
                while (timeTillNextShoot <= Constants.Epsilon)
                {
                    var betweenShoots = shootCooldown;
                    if (AmmoLimited)
                        ProcessReloadForLimitedAmmo(ref betweenShoots);

                    if (ammoStock != null)
                        ammoStock -= 1;

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

        internal void SupplyLimitedAmmoStock(Int32 ammoStock)
        {
            this.ammoStock = ammoStock;
            Reload();
        }

        private void ProcessReloadForLimitedAmmo(ref Single betweenShoots)
        {
            var reloadAfterThisBullet = false;
            if (CurrentAmmo == 1)
                reloadAfterThisBullet = true;
            if (CurrentAmmo == 0)
                Reload();
            CurrentAmmo -= 1;
            if (reloadAfterThisBullet)
                betweenShoots = reloadTime;
        }

        private void Reload() => CurrentAmmo = System.Math.Min(this.MaxAmmo, this.ammoStock ?? Int32.MaxValue);
    }
}
