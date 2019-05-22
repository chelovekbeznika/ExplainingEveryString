using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Movement;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class Enemy<TBlueprint> : Actor<TBlueprint>, IInterfaceAccessable, ICrashable, ITouchableByBullets 
        where TBlueprint : EnemyBlueprint
    {
        internal event EventHandler<EpicEventArgs> Death;
        private Boolean deathHandled = false;

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

        protected override void PlaceOnLevel(ActorStartInfo info)
        {
            base.PlaceOnLevel(info);
        }

        protected override void Construct(TBlueprint blueprint, ActorStartInfo startInfo, Level level)
        {
            this.PlayerLocator = () => level.PlayerPosition;
            base.Construct(blueprint, startInfo, level);
            this.MaxHitPoints = blueprint.Hitpoints;
            this.CollisionDamage = blueprint.CollisionDamage;
            this.deathEffect = blueprint.DeathEffect;
            this.Death += level.EpicEventOccured;
            this.MoveTargetSelector = MoveTargetSelectorFactory.Get(
                blueprint.MoveTargetSelectType, startInfo.TrajectoryTargets, PlayerLocator, () => Position);
            this.Mover = MoverFactory.Get(blueprint.Mover);
        }

        public override void Update(Single elapsedSeconds)
        {
            Vector2 target = MoveTargetSelector.GetTarget();
            Vector2 lineToTarget = target - Position;
            Vector2 positionChange = Mover.GetPositionChange(lineToTarget, elapsedSeconds, out Boolean goalReached);
            Position += positionChange;
            if (goalReached)
                MoveTargetSelector.SwitchToNextTarget();
            base.Update(elapsedSeconds);
        }
    }
}
