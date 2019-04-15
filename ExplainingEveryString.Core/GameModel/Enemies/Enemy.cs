using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal abstract class Enemy<TBlueprint> : GameObject<TBlueprint> where TBlueprint : EnemyBlueprint
    {
        internal event EventHandler<EpicEventArgs> Death;
        private Boolean deathHandled = false;

        public Single CollisionDamage { get; set; }
        internal Single MaxSpeed { get; set; }       
        private Func<Vector2> playerLocator;
        protected Vector2 PlayerPosition => playerLocator();

        private SpecEffectSpecification deathEffect;

        protected override float Hitpoints
        {
            get => base.Hitpoints;
            set
            {
                base.Hitpoints = value;
                if (value < MathConstants.Epsilon)
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

        protected override void Construct(TBlueprint blueprint, Level level)
        {
            base.Construct(blueprint, level);
            this.CollisionDamage = blueprint.CollisionDamage;
            this.MaxSpeed = blueprint.MaxSpeed;
            this.playerLocator = () => level.PlayerPosition;
            this.deathEffect = blueprint.DeathEffect;
            this.Death += level.EpicEventOccured;
        }
    }
}
