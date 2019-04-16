using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class PlayerWeapon
    {
        internal event EventHandler<PlayerShootEventArgs> Shoot;
        internal event EventHandler<EpicEventArgs> WeaponFired;

        private Single shootCooldown;
        private Single timeTillNextShoot;
        private Single nextBulletFirstUpdateTime = 0;

        private Single bulletSpeed;
        private Single range;
        private Single Damage { get; set; }

        private SpriteSpecification bulletSprite;
        private SpecEffectSpecification shootingEffect;

        private IPlayerInput input;
        private Func<Vector2> findOutWhereIAm;

        internal PlayerWeapon(PlayerWeaponBlueprint blueprint, IPlayerInput input, Func<Vector2> findOutWhereIAm)
        {
            shootCooldown = 1 / blueprint.FireRate;
            timeTillNextShoot = shootCooldown;
            bulletSpeed = blueprint.BulletSpeed;
            bulletSprite = blueprint.BulletSprite;
            range = blueprint.WeaponRange;
            Damage = blueprint.Damage;
            shootingEffect = blueprint.ShootingEffect;
            this.input = input;
            this.findOutWhereIAm = findOutWhereIAm;
        }

        internal void Check(Single elapsedSeconds)
        {
            if (timeTillNextShoot > MathConstants.Epsilon)
                timeTillNextShoot -= elapsedSeconds;
            if (input.IsFiring())
            {
                Boolean weaponFired = false;
                nextBulletFirstUpdateTime += elapsedSeconds;
                while (timeTillNextShoot <= MathConstants.Epsilon)
                {
                    timeTillNextShoot += shootCooldown;
                    nextBulletFirstUpdateTime -= shootCooldown;
                    if (nextBulletFirstUpdateTime < -MathConstants.Epsilon)
                        nextBulletFirstUpdateTime = 0;
                    OnShoot(nextBulletFirstUpdateTime);
                    weaponFired = true;
                }
                if (weaponFired)
                    WeaponFired?.Invoke(this, new EpicEventArgs
                    {
                        Position = findOutWhereIAm(),
                        SpecEffectSpecification = shootingEffect
                    });
            }
        }

        private void OnShoot(Single bulletFirstFrameUpdateTime)
        {
            Vector2 direction = input.GetFireDirection();
            Vector2 position = findOutWhereIAm();
            PlayerBullet bullet = new PlayerBullet(bulletSprite, position, direction * bulletSpeed, Damage, range);
            PlayerShootEventArgs eventArgs = new PlayerShootEventArgs {
                PlayerBullet = bullet,
                FirstUpdateTime = bulletFirstFrameUpdateTime
            };
            Shoot?.Invoke(this, eventArgs);
        }
    }
}
