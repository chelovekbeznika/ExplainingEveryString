using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Movement;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class Enemy<TBlueprint> : Actor<TBlueprint>, IInterfaceAccessable, ICrashable, ITouchableByBullets, IMultiPartDisplayble 
        where TBlueprint : EnemyBlueprint
    {
        internal event EventHandler<EpicEventArgs> Death;
        private Boolean deathHandled = false;
        private Weapon weapon;
        private Single startAngle;

        public Single CollisionDamage { get; set; }
        protected IMoveTargetSelector MoveTargetSelector { private get; set; }
        protected IMover Mover { private get; set; }
        protected Func<Vector2> PlayerLocator { get; private set; }
        protected Vector2 PlayerPosition => PlayerLocator();

        private SpecEffectSpecification deathEffect;

        public override Single HitPoints
        {
            get => base.HitPoints;
            set
            {
                base.HitPoints = value;
                if (value < Constants.Epsilon)
                {
                    if (!deathHandled)
                        Death?.Invoke(this, new EpicEventArgs
                        {
                            Position = this.Position,
                            SpecEffectSpecification = deathEffect
                        });
                    deathHandled = true;
                }
            }
        }

        public Single MaxHitPoints { get; private set; }

        protected override void PlaceOnLevel(ActorStartInfo startInfo)
        {
            base.PlaceOnLevel(startInfo);
            startAngle = startInfo.Angle;
        }

        protected override void Construct(TBlueprint blueprint, ActorStartInfo startInfo, Level level)
        {
            this.PlayerLocator = () => level.PlayerPosition;
            base.Construct(blueprint, startInfo, level);
            this.MaxHitPoints = blueprint.Hitpoints;
            this.CollisionDamage = blueprint.CollisionDamage;
            this.deathEffect = blueprint.DeathEffect;
            this.Death += level.EpicEventOccured;
            ConstructMovement(blueprint, startInfo);
            ConstructWeaponry(blueprint, startInfo, level);
        }

        private void ConstructMovement(TBlueprint blueprint, ActorStartInfo startInfo)
        {
            this.MoveTargetSelector = MoveTargetSelectorFactory.Get(
                blueprint.MoveTargetSelectType, startInfo.TrajectoryTargets, PlayerLocator, () => Position);
            this.Mover = MoverFactory.Get(blueprint.Mover);
        }

        private void ConstructWeaponry(TBlueprint blueprint, ActorStartInfo startInfo, Level level)
        {
            if (blueprint.Weapon != null)
            {
                IAimer aimer = AimersFactory.Get(blueprint.Weapon, startInfo, () => this.Position, PlayerLocator);
                weapon = new Weapon(blueprint.Weapon, aimer, () => this.Position, PlayerLocator, level);
                weapon.Shoot += level.EnemyShoot;
            }
        }

        public IEnumerable<IDisplayble> GetParts()
        {
            if (weapon != null)
                return new IDisplayble[] { weapon };
            else
                return Enumerable.Empty<IDisplayble>();
        }

        public override void Update(Single elapsedSeconds)
        {
            Move(elapsedSeconds);
            UseWeapon(elapsedSeconds);
            base.Update(elapsedSeconds);
        }

        private void Move(Single elapsedSeconds)
        {
            Vector2 target = MoveTargetSelector.GetTarget();
            Vector2 lineToTarget = target - Position;
            Vector2 positionChange = Mover.GetPositionChange(lineToTarget, elapsedSeconds, out Boolean goalReached);
            Position += positionChange;
            if (goalReached)
                MoveTargetSelector.SwitchToNextTarget();
        }

        private void UseWeapon(Single elapsedSeconds)
        {
            if (weapon != null)
            {
                weapon.Update(elapsedSeconds);
                if (weapon.IsFiring() && !weapon.IsVisible)
                    SpriteState.Angle = AngleConverter.ToRadians(weapon.GetFireDirection());
            }
        }
    }
}
