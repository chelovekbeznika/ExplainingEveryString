﻿using ExplainingEveryString.Core.Blueprints;
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
        private Single range;
        private Single Damage { get; set; }
        private String bulletSprite;

        private IPlayerInput input;
        private Func<Vector2> findOutWhereIAm;

        internal PlayerWeapon(PlayerWeaponBlueprint blueprint, IPlayerInput input, Func<Vector2> findOutWhereIAm)
        {
            shootCooldown = 1 / blueprint.FireRate;
            timeTillNextShoot = shootCooldown;
            bulletSpeed = blueprint.BulletSpeed;
            bulletSprite = blueprint.BulletSpriteName;
            range = blueprint.WeaponRange;
            Damage = blueprint.Damage;
            this.input = input;
            this.findOutWhereIAm = findOutWhereIAm;
        }

        internal void Check(Single elapsedSeconds)
        {
            if (timeTillNextShoot > MathConstants.Epsilon)
                timeTillNextShoot -= elapsedSeconds;
            if (input.IsFiring())
            {
                nextBulletFirstUpdateTime += elapsedSeconds;
                while (timeTillNextShoot <= MathConstants.Epsilon)
                {
                    timeTillNextShoot += shootCooldown;
                    nextBulletFirstUpdateTime -= shootCooldown;
                    if (nextBulletFirstUpdateTime < -MathConstants.Epsilon)
                        nextBulletFirstUpdateTime = 0;
                    OnShoot(nextBulletFirstUpdateTime);
                }
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
