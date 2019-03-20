using ExplainingEveryString.Core.Blueprints;
using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal sealed class Player : GameObject<PlayerBlueprint>
    {
        internal event EventHandler<PlayerShootEventArgs> PlayerShoot;

        private Vector2 speed = new Vector2(0, 0);
        private Single maxSpeed;
        private Single maxAcceleration;
        private Single shootCooldown;
        private Single timeTillNextShoot;
        private Single bulletSpeed;
        private String bulletSprite;
        private Single range;
        private IPlayerInput playerInput;

        protected override void Construct(PlayerBlueprint blueprint)
        {
            base.Construct(blueprint);
            playerInput = PlayerInputFactory.Create();
            maxSpeed = blueprint.MaxSpeed;
            maxAcceleration = blueprint.MaxAcceleration;
            shootCooldown = 1 / blueprint.FireRate;
            timeTillNextShoot = shootCooldown;
            bulletSpeed = blueprint.BulletSpeed;
            bulletSprite = blueprint.BulletSpriteName;
            range = blueprint.WeaponRange;
        }

        internal void Update(Single elapsedSeconds)
        {
            CheckWeapon(elapsedSeconds);
            Move(elapsedSeconds);
        }

        internal void CheckWeapon(Single elapsedSeconds)
        {
            timeTillNextShoot -= elapsedSeconds;
            if (timeTillNextShoot < 0 && playerInput.IsFiring())
            {
                Shoot();
                timeTillNextShoot += shootCooldown;
            }
        }

        private void Shoot()
        {
            Vector2 direction = playerInput.GetFireDirection();
            PlayerBullet bullet = new PlayerBullet(bulletSprite, Position, direction * bulletSpeed, range);
            PlayerShoot?.Invoke(this, new PlayerShootEventArgs { PlayerBullet = bullet });
        }

        internal void Move(Single elapsedSeconds)
        {
            Vector2 speed = GetCurrentSpeed(elapsedSeconds);
            Vector2 positionChange = speed * elapsedSeconds;
            Position += positionChange;
        }

        private Vector2 GetCurrentSpeed(Single elapsedSeconds)
        {
            Vector2 acceleration = GetAcceleration();
            speed += acceleration;
            if (speed.Length() > maxSpeed)
            {
                Single overcharge = speed.Length() / maxSpeed;
                speed /= overcharge;
            }
            if (acceleration.Length() == 0)
            {
                speed = FrictionCorrector.Correct(speed, elapsedSeconds);
            }

            return speed;
        }

        private Vector2 GetAcceleration()
        {
            Vector2 direction = playerInput.GetMoveDirection();
            return direction * maxAcceleration;
        }
    }

    internal class PlayerShootEventArgs : EventArgs
    {
        internal PlayerBullet PlayerBullet { get; set; }
    }
}
