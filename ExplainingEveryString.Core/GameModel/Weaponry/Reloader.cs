using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Reloader
    {
        internal event EventHandler ReloadStarted;
        internal event EventHandler ReloadFinished;

        private readonly Single shootCooldown;
        private readonly Single reloadTime;
        private Single timeTillNextShoot;
        private Single nextBulletFirstUpdateTime = 0;
        internal Int32? AmmoStock { get; private set; }
        internal Int32 MaxAmmo { get; private set; }
        internal Int32 CurrentAmmo { get; private set; }
        internal Single? ReloadRemained => reloadingNow ? timeTillNextShoot / reloadTime : null as Single?;

        private Boolean reloadingNow;

        private readonly Func<Boolean> isOn;
        private readonly Action<Single> onShoot;

        internal Boolean AmmoLimited => MaxAmmo > 1;
        internal Boolean HasAmmo => AmmoStock is null || AmmoStock > 0;

        internal Reloader(ReloaderSpecification specification, Func<Boolean> isOn, Action<Single> onShoot, Boolean fullAmmoAtStart = false)
        {
            this.isOn = isOn;
            this.onShoot = onShoot;
            this.MaxAmmo = specification.Ammo;
            this.shootCooldown = 1 / specification.FireRate;
            this.reloadTime = specification.ReloadTime;
            if (fullAmmoAtStart)
                LoadAmmo();
            else
            {
                reloadingNow = true;
                CurrentAmmo = 0;
            }

            timeTillNextShoot = AmmoLimited && !fullAmmoAtStart ? reloadTime : shootCooldown;
        }

        internal void Update(Single elapsedSeconds, out Boolean weaponFired)
        {
            if (timeTillNextShoot > Math.Constants.Epsilon)
                timeTillNextShoot -= elapsedSeconds;
            else
            {
                if (CurrentAmmo == 0)
                {
                    LoadAmmo();
                    if (AmmoLimited)
                        ReloadFinished?.Invoke(this, EventArgs.Empty);
                }
                reloadingNow = false;
            }
            if (isOn() && HasAmmo)
            {
                weaponFired = false;
                if (timeTillNextShoot <= Math.Constants.Epsilon)
                    nextBulletFirstUpdateTime += elapsedSeconds;
                while (timeTillNextShoot <= Math.Constants.Epsilon)
                {
                    var betweenShoots = shootCooldown;
                    if (AmmoLimited)
                        ProcessReloadForLimitedAmmo(ref betweenShoots);

                    if (AmmoStock != null)
                        AmmoStock -= 1;

                    timeTillNextShoot += betweenShoots;
                    nextBulletFirstUpdateTime -= betweenShoots;
                    if (nextBulletFirstUpdateTime < -Math.Constants.Epsilon)
                        nextBulletFirstUpdateTime = 0;
                    onShoot(nextBulletFirstUpdateTime);
                    weaponFired = true;
                }
            }
            else
                weaponFired = false;
        }

        internal void TryReload()
        {
            if (!AmmoLimited || CurrentAmmo == MaxAmmo || CurrentAmmo == AmmoStock || reloadingNow)
                return;

            CurrentAmmo = 0;
            timeTillNextShoot = reloadTime;
            reloadingNow = true;
            ReloadStarted?.Invoke(this, EventArgs.Empty);
        }

        internal void SupplyLimitedAmmoStock(Int32 ammoStock)
        {
            AmmoStock = ammoStock;
            LoadAmmo();
            timeTillNextShoot = System.Math.Min(timeTillNextShoot, shootCooldown);
        }

        private void ProcessReloadForLimitedAmmo(ref Single betweenShoots)
        {
            var reloadAfterThisBullet = false;
            if (CurrentAmmo == 1)
                reloadAfterThisBullet = true;
            if (CurrentAmmo == 0)
            {
                LoadAmmo();
                ReloadFinished?.Invoke(this, EventArgs.Empty);
            }
            CurrentAmmo -= 1;
            if (reloadAfterThisBullet)
            {
                reloadingNow = AmmoStock is null || AmmoStock > 0;
                betweenShoots = reloadTime;
                if (reloadingNow)
                    ReloadStarted?.Invoke(this, EventArgs.Empty);
            }
            else
                reloadingNow = false;
        }

        private void LoadAmmo() => CurrentAmmo = System.Math.Min(this.MaxAmmo, this.AmmoStock ?? Int32.MaxValue);
    }
}
