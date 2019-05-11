using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal abstract class Enemy<TBlueprint> : Actor<TBlueprint> where TBlueprint : EnemyBlueprint
    {
        internal event EventHandler<EpicEventArgs> Death;
        private Boolean deathHandled = false;

        public Single CollisionDamage { get; set; }
        internal Single MaxSpeed { get; set; }       
        protected Func<Vector2> PlayerLocator { get; private set; }
        protected Vector2 PlayerPosition => PlayerLocator();

        private SpecEffectSpecification deathEffect;

        protected override float Hitpoints
        {
            get => base.Hitpoints;
            set
            {
                base.Hitpoints = value;
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

        protected override void PlaceOnLevel(ActorStartPosition position)
        {
            base.PlaceOnLevel(position);
        }

        protected override void Construct(TBlueprint blueprint, Level level)
        {
            this.PlayerLocator = () => level.PlayerPosition;
            base.Construct(blueprint, level);
            this.CollisionDamage = blueprint.CollisionDamage;
            this.MaxSpeed = blueprint.MaxSpeed;
            this.deathEffect = blueprint.DeathEffect;
            this.Death += level.EpicEventOccured;
        }
    }
}
