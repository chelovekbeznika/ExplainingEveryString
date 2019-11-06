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
        public SpawnedActorsController SpawnedActors => behavior.SpawnedActors;
        public List<IEnemy> Avengers => behavior.PostMortemSurprise?.Avengers;

        private Single appearancePhaseRemained;
        private SpriteState appearanceSprite;

        internal Boolean IsInAppearancePhase => appearancePhaseRemained > -Constants.Epsilon;
        public override SpriteState SpriteState => IsInAppearancePhase ? appearanceSprite : base.SpriteState;
        public override CollidableMode CollidableMode => IsInAppearancePhase ? CollidableMode.Ghost : base.CollidableMode;
        private String collideTag;
        public String CollideTag => collideTag;
        public Single CollisionDamage { get; set; }

        private EnemyBehavior behavior;

        public override Single HitPoints
        {
            get => base.HitPoints;
            set
            {
                base.HitPoints = value;
                if (value < Constants.Epsilon)
                {
                    death.TryHandle();
                    behavior.PostMortemSurprise?.TryTrigger();
                }
            }
        }
        
        public Single MaxHitPoints { get; private set; }
        public virtual Boolean ShowInterfaceInfo => !IsInAppearancePhase;

        protected override void Construct(TBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            this.behavior = new EnemyBehavior(this, () => level.Player.Position);
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

            behavior.Construct(blueprint.Behavior, startInfo, level, factory);
        }

        public override IEnumerable<IDisplayble> GetParts()
        {
            if (!IsInAppearancePhase)
                return behavior.GetPartsToDisplay();
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
                behavior.Update(elapsedSeconds);
                if (behavior.EnemyAngle != null)
                    SpriteState.Angle = behavior.EnemyAngle.Value;
            }
            base.Update(elapsedSeconds);
        }

        public void Crash()
        {
            behavior.PostMortemSurprise?.Cancel();
            Destroy();
        }
    }
}
