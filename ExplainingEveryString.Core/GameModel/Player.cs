using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal sealed class Player : Actor<PlayerBlueprint>, IDisplayble, IUpdateable, IMovableCollidable,
        ITouchableByBullets, IInterfaceAccessable
    {
        internal const String BlueprintType = "Player";

        private EpicEvent damageTaken;
        private EpicEvent softDamageTaken;
        private EpicEvent baseDestroyed;
        private EpicEvent cannonDestroyed;
        private EpicEvent weaponSwitched;
        private EpicEvent refreshed;

        public Boolean ShowInterfaceInfo => false;
        public Single MaxHitPoints { get; private set; }
        public override Single HitPoints
        {
            get => base.HitPoints;
            set
            {
                base.HitPoints = value;
                if (value < Math.Constants.Epsilon)
                {
                    baseDestroyed.TryHandle();
                    cannonDestroyed.TryHandle();
                }
            }
        }
        internal Single FromLastCheckpoint { get; private set; } = Single.NaN;
        internal Func<List<IEnemy>> CurrentEnemies { get; set; }
        public String CollideTag => null;
        public override CollidableMode CollidableMode => DashController.IsActive ? CollidableMode.Shadow : base.CollidableMode;

        internal IPlayerInput Input { get; private set; }
        internal PlayerDashController DashController { get; private set; }

        private Vector2 speed = new Vector2(0, 0);
        private Single maxSpeed;
        private Single maxAcceleration;
        private Single bulletHitboxWidth;

        private DashAcceleration dashAcceleration;

        internal IEnemy CurrentTarget { get; private set; }
        private Weapon[] weapons;
        private Int32 selectedWeapon = 0;
        internal Weapon Weapon => weapons[selectedWeapon];
        internal IEnumerable<String> AvailableWeapons => weapons.Where(weapon => weapon.Reloader.HasAmmo).Select(weapon => weapon.Name);

        protected override void Construct(PlayerBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            Input = level.PlayerInputFactory.Create();
            maxSpeed = blueprint.MaxSpeed;
            maxAcceleration = blueprint.MaxAcceleration;
            bulletHitboxWidth = blueprint.BulletHitboxWidth;
            
            MaxHitPoints = blueprint.Hitpoints;

            weapons = blueprint.Weapons.Select(spec => new Weapon(spec, Input, () => Position, () => CurrentTarget, level, true)).ToArray();
            foreach (var weapon in weapons)
                weapon.Shoot += level.PlayerShoot;

            damageTaken = new EpicEvent(level, blueprint.DamageEffect, false, this, true);
            softDamageTaken = new EpicEvent(level, blueprint.SoftDamageEffect, false, this, true);
            baseDestroyed = new EpicEvent(level, blueprint.BaseDestructionEffect, true, this, false);
            cannonDestroyed = new EpicEvent(level, blueprint.CannonDestructionEffect, true, this.Weapon, true);
            weaponSwitched = new EpicEvent(level, blueprint.WeaponSwitchEffect, false, this, true);
            refreshed = new EpicEvent(level, blueprint.RefreshEffect, false, this, true, true);
            ConstructDash(level, blueprint.Dash);
        }

        private void ConstructDash(Level level, DashSpecification specification)
        {
            this.dashAcceleration = new DashAcceleration(specification);
            Boolean dashIsAvailable() => speed.Length() >= maxSpeed * dashAcceleration.AvailabilityThreshold;
            this.DashController = new PlayerDashController(specification, dashIsAvailable, this, level);
            DashController.DashActivated += (sender, e) => speed *= dashAcceleration.SpeedIncrease;
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            Input.Update(elapsedSeconds);
            WeaponSelect();
            if (Input.IsTryingToReload())
                Weapon.Reloader.TryReload();
            TargetSelect();
            Weapon.Update(elapsedSeconds);
            DashController.Update(elapsedSeconds);
            Move(elapsedSeconds);
            FromLastCheckpoint += elapsedSeconds;
        }

        private void TargetSelect()
        {
            if (Weapon.IsHoming)
            {
                var fireAngle = AngleConverter.ToRadians(Weapon.GetFireDirection());
                Single angleBetween(IEnemy enemy)
                {
                    var angleToEnemy = AngleConverter.ToRadians((enemy as ICollidable).Position - Position);
                    return System.Math.Abs(AngleConverter.ClosestArc(fireAngle, angleToEnemy));
                };
                CurrentTarget = CurrentEnemies().OrderBy(enemy => angleBetween(enemy)).FirstOrDefault();
            }
            else
                CurrentTarget = null;
        }

        internal void CheckpointRefresh(ArsenalSpecification playerApsenal)
        {
            SupplyWeapons(playerApsenal);
            HitPoints = MaxHitPoints;
            refreshed.TryHandle();
            FromLastCheckpoint = 0;
        }

        internal void SupplyWeapons(ArsenalSpecification playerArsenal)
        {
            foreach (var weapon in weapons.Where(weapon => weapon.Name != Constants.DefaultPlayerWeapon))
            {
                if (playerArsenal?.AvailableWeapons.ContainsKey(weapon.Name) ?? false)
                    weapon.Reloader.SupplyLimitedAmmoStock(playerArsenal.AvailableWeapons[weapon.Name]);
                else
                    weapon.Reloader.SupplyLimitedAmmoStock(0);
            }
        }

        private void WeaponSelect()
        {
            var switchMeasure = Input.WeaponSwitchMeasure();
            var oldSelectedWeapon = selectedWeapon;
            if (switchMeasure == 0 && !Weapon.Reloader.HasAmmo)
                switchMeasure = -1;
            foreach (var _ in Enumerable.Range(0, System.Math.Abs(switchMeasure)))
            {
                do
                {
                    if (switchMeasure < 0)
                        selectedWeapon -= 1;
                    else
                        selectedWeapon += 1;
                    if (selectedWeapon < 0)
                        selectedWeapon = weapons.Length - 1;
                    if (selectedWeapon >= weapons.Length)
                        selectedWeapon = 0;
                }
                while (!Weapon.Reloader.HasAmmo);
            }
            if (oldSelectedWeapon != selectedWeapon)
                weaponSwitched.TryHandle();
        }

        private void Move(Single elapsedSeconds)
        {
            var speed = GetCurrentSpeed(elapsedSeconds);
            var positionChange = speed * elapsedSeconds;
            Position += positionChange;
        }

        public override void TakeDamage(Single damage)
        {
            base.TakeDamage(damage);
            damageTaken.TryHandle();
        }

        internal void TakeDamageSoftly(Single damage)
        {
            var savedValue = FromLastHit;
            base.TakeDamage(damage);
            softDamageTaken.TryHandle();
            FromLastHit = savedValue;
        }

        private Vector2 GetCurrentSpeed(Single elapsedSeconds)
        {
            var acceleration = GetAcceleration();
            speed += acceleration;
            var speedLimit = DashController.IsActive ? maxSpeed * dashAcceleration.MaxSpeedIncrease : maxSpeed;
            if (speed.Length() > speedLimit)
            {
                var overcharge = speed.Length() / speedLimit;
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
            var direction = Input.GetMoveDirection();
            var acceleration = direction * maxAcceleration;
            if (DashController.IsActive)
                return acceleration * dashAcceleration.AccelerationIncrease;
            else
                return acceleration;
        }

        public override IEnumerable<IDisplayble> GetParts()
        {
            return new IDisplayble[] { Weapon, DashController };
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
