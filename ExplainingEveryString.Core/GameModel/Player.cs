using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal sealed class Player : GameObject<PlayerBlueprint>, IUpdatable, ITouchableByBullets
    {
        internal event EventHandler<EpicEventArgs> DamageTaken;

        private Vector2 speed = new Vector2(0, 0);
        private Single maxSpeed;
        private Single maxAcceleration;
        private IPlayerInput input;
        private SpecEffectSpecification damageEffect;

        internal PlayerWeapon Weapon { get; private set; }

        protected override void Construct(PlayerBlueprint blueprint, Level level)
        {
            base.Construct(blueprint, level);
            input = PlayerInputFactory.Create();
            maxSpeed = blueprint.MaxSpeed;
            maxAcceleration = blueprint.MaxAcceleration;
            damageEffect = blueprint.DamageEffect;
            DamageTaken += level.EpicEventOccured;
            
            Weapon = new PlayerWeapon(blueprint.Weapon, input, () => Position);
            Weapon.Shoot += level.PlayerShoot;
            Weapon.WeaponFired += level.EpicEventOccured;
        }

        public void Update(Single elapsedSeconds)
        {
            Weapon.Check(elapsedSeconds);
            Move(elapsedSeconds);
        }

        internal void Move(Single elapsedSeconds)
        {
            Vector2 speed = GetCurrentSpeed(elapsedSeconds);
            Vector2 positionChange = speed * elapsedSeconds;
            Position += positionChange;
        }

        public override void TakeDamage(Single damage)
        {
            base.TakeDamage(damage);
            DamageTaken?.Invoke(this, new EpicEventArgs
            {
                Position = this.Position,
                SpecEffectSpecification = damageEffect
            });
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
            Vector2 direction = input.GetMoveDirection();
            return direction * maxAcceleration;
        }
    }

    internal class PlayerShootEventArgs : EventArgs
    {
        internal PlayerBullet PlayerBullet { get; set; }
        internal Single FirstUpdateTime { get; set; }
    }
}
