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
    internal class Enemy<TBlueprint> : Actor<TBlueprint>, IEnemy
        where TBlueprint : EnemyBlueprint
    {
        private OneTimeEpicEvent death;
        internal OneTimeEpicEvent beforeAppearance;
        internal OneTimeEpicEvent afterAppearance;
        public SpawnedActorsController SpawnedActors { get; private set; }

        private Single startAngle;

        private Single appearancePhaseRemained;
        private SpriteState appearanceSprite;

        internal Boolean IsInAppearancePhase => appearancePhaseRemained > -Constants.Epsilon;
        public override SpriteState SpriteState => IsInAppearancePhase ? appearanceSprite : base.SpriteState;
        public override CollidableMode Mode => IsInAppearancePhase ? CollidableMode.Ghost : base.Mode;
        public Single CollisionDamage { get; set; }

        private Weapon weapon;
        protected IMoveTargetSelector MoveTargetSelector { private get; set; }
        protected IMover Mover { private get; set; }
        protected Func<Vector2> PlayerLocator { get; private set; }
        protected Vector2 PlayerPosition => PlayerLocator();

        public override Single HitPoints
        {
            get => base.HitPoints;
            set
            {
                base.HitPoints = value;
                if (value < Constants.Epsilon)
                {
                    death.TryHandle();
                }
            }
        }
        
        public Single MaxHitPoints { get; private set; }
        public virtual Boolean ShowInterfaceInfo => !IsInAppearancePhase;

        protected override void PlaceOnLevel(ActorStartInfo startInfo)
        {
            base.PlaceOnLevel(startInfo);
            startAngle = startInfo.Angle;
        }

        protected override void Construct(TBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            this.PlayerLocator = () => level.PlayerPosition;
            base.Construct(blueprint, startInfo, level, factory);
            this.MaxHitPoints = blueprint.Hitpoints;
            this.CollisionDamage = blueprint.CollisionDamage;
            this.death = new OneTimeEpicEvent(level, blueprint.DeathEffect, this);
            this.afterAppearance = new OneTimeEpicEvent(level, blueprint.AfterAppearanceEffect, this);
            this.beforeAppearance = new OneTimeEpicEvent(level, blueprint.BeforeAppearanceEffect, this);
            this.appearanceSprite = new SpriteState(blueprint.AppearancePhaseSprite);
            this.appearancePhaseRemained = startInfo.AppearancePhaseDuration > 0 
                ? startInfo.AppearancePhaseDuration
                : blueprint.DefaultAppearancePhaseDuration;
            ConstructMovement(blueprint, startInfo);
            ConstructWeaponry(blueprint, startInfo, level);
            if (blueprint.Spawner != null) 
                this.SpawnedActors = new SpawnedActorsController(blueprint.Spawner, this, startInfo.SpawnPoints, factory);
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
            if (weapon != null && !IsInAppearancePhase)
                return new IDisplayble[] { weapon };
            else
                return Enumerable.Empty<IDisplayble>();
        }

        public override void Update(Single elapsedSeconds)
        {
            if (IsInAppearancePhase)
            {
                appearancePhaseRemained -= elapsedSeconds;
                beforeAppearance.TryHandle();
            }   
            else
            {
                afterAppearance.TryHandle();
                Move(elapsedSeconds);
                UseWeapon(elapsedSeconds);
            }
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
