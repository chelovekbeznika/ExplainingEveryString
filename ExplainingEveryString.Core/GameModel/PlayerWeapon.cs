using ExplainingEveryString.Core.Blueprints;
using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class PlayerWeapon
    {
        internal event EventHandler<PlayerShootEventArgs> Shoot;

        private Single shootCooldown;
        private Single timeTillNextShoot;
        private Single nextBulletFirstUpdateTime = 0;

        private Single bulletSpeed;
        private String bulletSprite;
        private Single range;

        private IPlayerInput input;
        private Func<Vector2> findOutWhereIAm;

        internal PlayerWeapon(PlayerWeaponBlueprint blueprint, IPlayerInput input, Func<Vector2> findOutWhereIAm)
        {
            shootCooldown = 1 / blueprint.FireRate;
            timeTillNextShoot = shootCooldown;
            bulletSpeed = blueprint.BulletSpeed;
            bulletSprite = blueprint.BulletSpriteName;
            range = blueprint.WeaponRange;
            this.input = input;
            this.findOutWhereIAm = findOutWhereIAm;
        }

        internal void Check(Single elapsedSeconds)
        {
            if (timeTillNextShoot > 0)
                timeTillNextShoot -= elapsedSeconds;
            if (input.IsFiring())
            {
                nextBulletFirstUpdateTime += elapsedSeconds;
                while (timeTillNextShoot <= 0)
                {
                    timeTillNextShoot += shootCooldown;
                    nextBulletFirstUpdateTime -= shootCooldown;
                    OnShoot(nextBulletFirstUpdateTime);
                }
            }
        }

        private void OnShoot(Single bulletFirstFrameUpdateTime)
        {
            Vector2 direction = input.GetFireDirection();
            Vector2 position = findOutWhereIAm();
            PlayerBullet bullet = new PlayerBullet(bulletSprite, position, direction * bulletSpeed, range);
            PlayerShootEventArgs eventArgs = new PlayerShootEventArgs {
                PlayerBullet = bullet,
                FirstUpdateTime = bulletFirstFrameUpdateTime
            };
            Shoot?.Invoke(this, eventArgs);
        }
    }
}
