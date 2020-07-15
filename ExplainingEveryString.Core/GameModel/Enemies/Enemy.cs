using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
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
        private EpicEvent goalAchieved;
        public virtual ISpawnedActorsController SpawnedActors => Behavior.SpawnedActors;
        public List<IEnemy> Avengers => Behavior.PostMortemSurprise?.Avengers;
        public event EventHandler<EnemyBehaviorChangedEventArgs> EnemyBehaviorChanged;
        public event EventHandler Died;

        private Single appearancePhaseRemained;
        private SpriteState appearanceSprite;

        internal Boolean IsInAppearancePhase => appearancePhaseRemained > -Constants.Epsilon;
        public override SpriteState SpriteState => IsInAppearancePhase ? appearanceSprite : base.SpriteState;
        public override CollidableMode CollidableMode => IsInAppearancePhase 
            ? CollidableMode.Ghost 
            : Behavior.IsTeleporter 
                ? CollidableMode.Teleporter 
                : base.CollidableMode;
        public String CollideTag { get; private set; }
        public Single CollisionDamage { get; set; }

        protected virtual EnemyBehavior Behavior { get; set; }

        public override Single HitPoints
        {
            get => base.HitPoints;
            set
            {
                base.HitPoints = value;
                if (value < Constants.Epsilon)
                {
                    death.TryHandle();
                    Behavior.PostMortemSurprise?.TryTrigger();
                    Died?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
        public Single MaxHitPoints { get; protected set; }

        private Boolean hideHealthBar;
        public virtual Boolean ShowInterfaceInfo => !IsInAppearancePhase && !hideHealthBar;

        protected override void Construct(TBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            this.Behavior = new EnemyBehavior(this, () => level.Player.Position);
            base.Construct(blueprint, startInfo, level, factory);
            this.MaxHitPoints = blueprint.Hitpoints;
            this.CollisionDamage = blueprint.CollisionDamage;
            
            this.death = new EpicEvent(level, blueprint.DeathEffect, true, this, true);
            this.afterAppearance = new EpicEvent(level, blueprint.AfterAppearanceEffect, true, this, false);
            this.beforeAppearance = new EpicEvent(level, blueprint.BeforeAppearanceEffect, true, this, false);
            this.goalAchieved = new EpicEvent(level, blueprint.GoalAchievedEffect, false, this, false);
            Behavior.MoveGoalReached += (sender, e) => goalAchieved.TryHandle();

            this.appearanceSprite = new SpriteState(blueprint.AppearancePhaseSprite)
            {
                Angle = startInfo.BehaviorParameters.Angle
            };
            this.appearancePhaseRemained = startInfo.AppearancePhaseDuration > 0 
                ? startInfo.AppearancePhaseDuration
                : blueprint.DefaultAppearancePhaseDuration;
            this.CollideTag = blueprint.CollideTag;
            this.hideHealthBar = blueprint.HideHealthBar;

            Behavior.Construct(blueprint.Behavior, startInfo.BehaviorParameters, level, factory);
        }

        public override IEnumerable<IDisplayble> GetParts()
        {
            if (!IsInAppearancePhase)
                return Behavior.GetPartsToDisplay();
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
                if (!afterAppearance.Handled)
                    SpawnedActors?.TurnOn();
                afterAppearance.TryHandle();
                Behavior.Update(elapsedSeconds);
                if (Behavior.EnemyAngle != null)
                    SpriteState.Angle = Behavior.EnemyAngle.Value;
            }
            base.Update(elapsedSeconds);
        }

        public void Crash()
        {
            Behavior.PostMortemSurprise?.Cancel();
            Destroy();
        }

        protected void OnBehaviorChanged(SpawnedActorsController oldSpawner, SpawnedActorsController newSpawner)
        {
            EnemyBehaviorChanged?.Invoke(this, new EnemyBehaviorChangedEventArgs()
            {
                OldSpawner = oldSpawner,
                NewSpawner = newSpawner
            });
        }
    }
}
