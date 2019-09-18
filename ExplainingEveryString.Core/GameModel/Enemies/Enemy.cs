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
        private EpicEvent death;
        private EpicEvent beforeAppearance;
        private EpicEvent afterAppearance;
        public SpawnedActorsController SpawnedActors { get; private set; }
        public List<IEnemy> Avengers => postMortemSurprise?.Avengers;

        private Single appearancePhaseRemained;
        private SpriteState appearanceSprite;

        internal Boolean IsInAppearancePhase => appearancePhaseRemained > -Constants.Epsilon;
        public override SpriteState SpriteState => IsInAppearancePhase ? appearanceSprite : base.SpriteState;
        public override CollidableMode CollidableMode => IsInAppearancePhase ? CollidableMode.Ghost : base.CollidableMode;
        private String collideTag;
        public String CollideTag => collideTag;
        public Single CollisionDamage { get; set; }

        private Weapon weapon;
        private PostMortemSurprise postMortemSurprise;
        protected IMoveTargetSelector MoveTargetSelector { private get; set; }
        protected IMover Mover { private get; set; }
        protected Func<Vector2> PlayerLocator { get; private set; }
        private Func<Vector2> CurrentPositionLocator => () => this.Position;
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
                    postMortemSurprise?.TryTrigger();
                }
            }
        }
        
        public Single MaxHitPoints { get; private set; }
        public virtual Boolean ShowInterfaceInfo => !IsInAppearancePhase;

        protected override void Construct(TBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            this.PlayerLocator = () => level.Player.Position;
            base.Construct(blueprint, startInfo, level, factory);
            this.MaxHitPoints = blueprint.Hitpoints;
            this.CollisionDamage = blueprint.CollisionDamage;
            this.death = new EpicEvent(level, blueprint.DeathEffect, true, this, true);
            this.afterAppearance = new EpicEvent(level, blueprint.AfterAppearanceEffect, true, this, false);
            this.beforeAppearance = new EpicEvent(level, blueprint.BeforeAppearanceEffect, true, this, false);
            this.appearanceSprite = new SpriteState(blueprint.AppearancePhaseSprite) { Angle = startInfo.Angle };
            this.appearancePhaseRemained = startInfo.AppearancePhaseDuration > 0 
                ? startInfo.AppearancePhaseDuration
                : blueprint.DefaultAppearancePhaseDuration;
            this.collideTag = blueprint.CollideTag;
            ConstructMovement(blueprint, startInfo);
            ConstructWeaponry(blueprint, startInfo, level, factory);
        }

        private void ConstructMovement(TBlueprint blueprint, ActorStartInfo startInfo)
        {
            this.MoveTargetSelector = MoveTargetSelectorFactory.Get(
                blueprint.MoveTargetSelectType, startInfo.TrajectoryTargets, PlayerLocator, CurrentPositionLocator);
            this.Mover = MoverFactory.Get(blueprint.Mover);
        }

        private void ConstructWeaponry(TBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            if (blueprint.Weapon != null)
            {
                IAimer aimer = AimersFactory.Get(
                    blueprint.Weapon.AimType, startInfo.Angle, CurrentPositionLocator, PlayerLocator);
                weapon = new Weapon(blueprint.Weapon, aimer, CurrentPositionLocator, PlayerLocator, level);
                weapon.Shoot += level.EnemyShoot;
            }
            if (blueprint.PostMortemSurprise != null)
            {
                postMortemSurprise = new PostMortemSurprise(blueprint.PostMortemSurprise, CurrentPositionLocator, 
                    PlayerLocator, level, startInfo.LevelSpawnPoints, factory);
            }
            if (blueprint.Spawner != null)
                this.SpawnedActors = new SpawnedActorsController(blueprint.Spawner, this, startInfo.LevelSpawnPoints, factory);
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

        public void Crash()
        {
            postMortemSurprise?.Cancel();
            Destroy();
        }
    }
}
