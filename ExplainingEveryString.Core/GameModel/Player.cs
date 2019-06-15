using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using System;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Displaying;
using System.Collections.Generic;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Core.GameModel
{
    internal sealed class Player : Actor<PlayerBlueprint>, IMultiPartDisplayble, IUpdatable, ITouchableByBullets, IInterfaceAccessable
    {
        private event EventHandler<EpicEventArgs> DamageTaken;

        public Boolean ShowInterfaceInfo => false;
        public Single MaxHitPoints { get; private set; }
        internal Vector2 FireDirection => input.GetFireDirection();

        private Vector2 speed = new Vector2(0, 0);
        private Single maxSpeed;
        private Single maxAcceleration;
        private IPlayerInput input;
        private Single bulletHitboxWidth;
        private SpecEffectSpecification damageEffect;

        private Weapon Weapon { get; set; }

        protected override void Construct(PlayerBlueprint blueprint, ActorStartInfo startInfo, Level level)
        {
            base.Construct(blueprint, startInfo, level);
            input = level.PlayerInputFactory.Create();
            maxSpeed = blueprint.MaxSpeed;
            maxAcceleration = blueprint.MaxAcceleration;
            bulletHitboxWidth = blueprint.BulletHitboxWidth;
            damageEffect = blueprint.DamageEffect;
            DamageTaken += level.EpicEventOccured;
            MaxHitPoints = blueprint.Hitpoints;

            Weapon = new Weapon(blueprint.Weapon, input, () => Position, null, level);
            Weapon.Shoot += level.PlayerShoot;
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            Weapon.Update(elapsedSeconds);
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
            DamageTaken?.Invoke(this, StandardEpicEvent(damageEffect));
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

        public IEnumerable<IDisplayble> GetParts()
        {
            return new IDisplayble[] { Weapon };
        }

        public override Hitbox GetBulletsHitbox()
        {
            return new Hitbox
            {
                Left = Position.X - bulletHitboxWidth / 2,
                Right = Position.X + bulletHitboxWidth / 2,
                Bottom = Position.Y - bulletHitboxWidth / 2,
                Top = Position.Y + bulletHitboxWidth / 2
            };
        }
    }
}
