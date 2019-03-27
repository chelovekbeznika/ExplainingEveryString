using Microsoft.Xna.Framework;
using ExplainingEveryString.Data.Blueprints;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal abstract class GameObject<TBlueprint> : IDisplayble, IUpdatable where TBlueprint : Blueprint
    {
        private String spriteName;

        public Vector2 Position { get; protected set; }
        private Single Width { get; set; }
        private Single Height { get; set; }
        protected Single Hitpoints { get; set; }

        public String CurrentSpriteName { get => spriteName; }

        internal void Initialize(TBlueprint blueprint, Vector2 position)
        {
            PlaceOnLevel(position);
            Construct(blueprint);
        }

        public abstract void Update(Single elapsedSeconds);
        
        private void PlaceOnLevel(Vector2 position)
        {
            this.Position = position;
        }

        protected virtual void Construct(TBlueprint blueprint)
        {
            this.spriteName = blueprint.DefaultSpriteName;
            this.Height = blueprint.Height;
            this.Width = blueprint.Width;
            this.Hitpoints = blueprint.Hitpoints;
        }

        internal void TakeDamage(Single damage)
        {
            Hitpoints -= damage;
        }

        internal Hitbox GetHitbox()
        {
            return new Hitbox
            {
                Bottom = Position.Y - Height / 2,
                Top = Position.Y + Height / 2,
                Left = Position.X - Width / 2,
                Right = Position.X + Width / 2
            };
        }

        public bool IsAlive()
        {
            return Hitpoints > MathConstants.Epsilon;
        }
    }
}
