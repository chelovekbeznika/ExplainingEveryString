using Microsoft.Xna.Framework;
using ExplainingEveryString.Data.Blueprints;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal abstract class GameObject<TBlueprint> : IGameObject where TBlueprint : Blueprint
    {
        public Vector2 Position { get; protected set; }
        private Single Width { get; set; }
        private Single Height { get; set; }
        protected Single Hitpoints { get; set; }

        public String CurrentSpriteName { get; private set; }

        internal void Initialize(TBlueprint blueprint, Level level, Vector2 position)
        {
            PlaceOnLevel(position);
            Construct(blueprint, level);
        }
        
        private void PlaceOnLevel(Vector2 position)
        {
            this.Position = position;
        }

        protected virtual void Construct(TBlueprint blueprint, Level level)
        {
            this.CurrentSpriteName = blueprint.DefaultSpriteName;
            this.Height = blueprint.Height;
            this.Width = blueprint.Width;
            this.Hitpoints = blueprint.Hitpoints;
        }

        public void TakeDamage(Single damage)
        {
            Hitpoints -= damage;
        }

        public bool IsAlive()
        {
            return Hitpoints > MathConstants.Epsilon;
        }

        public void Destroy()
        {
            Hitpoints = 0;
        }

        public Hitbox GetHitbox()
        {
            return new Hitbox
            {
                Bottom = Position.Y - Height / 2,
                Top = Position.Y + Height / 2,
                Left = Position.X - Width / 2,
                Right = Position.X + Width / 2
            };
        }
    }
}
