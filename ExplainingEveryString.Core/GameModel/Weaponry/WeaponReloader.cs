using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
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
        private Single reloadTime;
        private Single timeTillNextShoot;
        private Single nextBulletFirstUpdateTime = 0;
        private Int32 maxAmmo;
        private Int32 currentAmmo;
        private IAimer aimer;
        private Action<Single> onShoot;

        private Boolean AmmoLimited => maxAmmo > 1;

        internal WeaponReloader(WeaponSpecification blueprint, IAimer aimer, Action<Single> onShoot)
        { 
            this.aimer = aimer;
            this.onShoot = onShoot;
            this.maxAmmo = blueprint.Ammo;
            shootCooldown = 1 / blueprint.FireRate;
            this.reloadTime = blueprint.ReloadTime;
            this.currentAmmo = 0;

            timeTillNextShoot = AmmoLimited ? reloadTime : shootCooldown;
        }

        internal void TryReload(Single elapsedSeconds, out Boolean weaponFired)
        {
            if (timeTillNextShoot > Constants.Epsilon)
                timeTillNextShoot -= elapsedSeconds;
            if (aimer.IsFiring())
            {
                weaponFired = false;
                if (timeTillNextShoot <= Constants.Epsilon)
                    nextBulletFirstUpdateTime += elapsedSeconds;
                while (timeTillNextShoot <= Constants.Epsilon)
                {
                    Single betweenShoots = shootCooldown;
                    if (AmmoLimited)
                        ProcessReloadForLimitedAmmo(ref betweenShoots);

                    timeTillNextShoot += betweenShoots;
                    nextBulletFirstUpdateTime -= betweenShoots;
                    if (nextBulletFirstUpdateTime < -Constants.Epsilon)
                        nextBulletFirstUpdateTime = 0;
                    onShoot(nextBulletFirstUpdateTime);
                    weaponFired = true;
                }
            }
            else
                weaponFired = false;
        }

        private void ProcessReloadForLimitedAmmo(ref Single betweenShoots)
        {
            Boolean reloadAfterThisBullet = false;
            if (currentAmmo == 1)
                reloadAfterThisBullet = true;
            if (currentAmmo == 0)
                currentAmmo = this.maxAmmo;
            currentAmmo -= 1;
            if (reloadAfterThisBullet)
                betweenShoots = reloadTime;
        }
    }
}
