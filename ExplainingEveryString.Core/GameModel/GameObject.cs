using Microsoft.Xna.Framework;
using ExplainingEveryString.Data.Blueprints;
using System;
using ExplainingEveryString.Core.Displaying;

namespace ExplainingEveryString.Core.GameModel
{
    internal abstract class GameObject<TBlueprint> : IGameObject where TBlueprint : Blueprint
    {
        private Vector2 position;

        public Vector2 Position
        {
            get => position;
            set
            {
                OldPosition = position;
                position = value;
            }
        }
        public Vector2 OldPosition { get; private set; }
        private Single Width { get; set; }
        private Single Height { get; set; }
        protected virtual Single Hitpoints { get; set; }

        public SpriteState SpriteState { get; private set; }

        internal void Initialize(TBlueprint blueprint, Level level, Vector2 position)
        {
            PlaceOnLevel(position);
            Construct(blueprint, level);
        }
        
        private void PlaceOnLevel(Vector2 position)
        {          
            this.Position = position;
            this.OldPosition = position;
        }

        protected virtual void Construct(TBlueprint blueprint, Level level)
        {
            this.SpriteState = new SpriteState(blueprint.DefaultSprite);
            this.Height = blueprint.Height;
            this.Width = blueprint.Width;
            this.Hitpoints = blueprint.Hitpoints;
        }

        public virtual void Update(Single elapsedSeconds)
        {
            SpriteState.Update(elapsedSeconds);
        }

        public virtual void TakeDamage(Single damage)
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

        public Hitbox GetCurrentHitbox()
        {
            return GetHitboxWithCenterIn(Position);
        }

        public Hitbox GetOldHitbox()
        {
            return GetHitboxWithCenterIn(OldPosition);
        }

        private Hitbox GetHitboxWithCenterIn(Vector2 center)
        {
            return new Hitbox
            {
                Bottom = center.Y - Height / 2,
                Top = center.Y + Height / 2,
                Left = center.X - Width / 2,
                Right = center.X + Width / 2
            };
        }
    }
}
