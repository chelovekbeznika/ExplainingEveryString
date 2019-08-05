using Microsoft.Xna.Framework;
using ExplainingEveryString.Data.Blueprints;
using System;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Core.GameModel
{
    internal abstract class Actor<TBlueprint> : IActor where TBlueprint : Blueprint
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
        public virtual Single HitPoints { get; set; }

        public virtual SpriteState SpriteState { get; private set; }
        public Boolean IsVisible => IsAlive();
        public virtual CollidableMode Mode => CollidableMode.Solid;

        internal void Initialize(TBlueprint blueprint, Level level, ActorStartInfo startInfo, ActorsFactory factory)
        {
            PlaceOnLevel(startInfo);
            Construct(blueprint, startInfo, level, factory);
        }
        
        protected virtual void PlaceOnLevel(ActorStartInfo info)
        {          
            this.Position = info.Position;
            this.OldPosition = info.Position;
        }

        protected virtual void Construct(TBlueprint blueprint, ActorStartInfo info, Level level, ActorsFactory factory)
        {
            this.SpriteState = new SpriteState(blueprint.DefaultSprite);
            this.Height = blueprint.Height;
            this.Width = blueprint.Width;
            this.HitPoints = blueprint.Hitpoints;
        }

        public virtual void Update(Single elapsedSeconds)
        {
            SpriteState.Update(elapsedSeconds);
        }

        public virtual void TakeDamage(Single damage)
        {
            HitPoints -= damage;
        }

        public bool IsAlive()
        {
            return HitPoints > Constants.Epsilon;
        }

        public virtual void Destroy()
        {
            HitPoints = 0;
        }

        public virtual Hitbox GetCurrentHitbox()
        {
            return GetHitboxWithCenterIn(Position);
        }

        public virtual Hitbox GetOldHitbox()
        {
            return GetHitboxWithCenterIn(OldPosition);
        }

        public virtual Hitbox GetBulletsHitbox()
        {
            return GetCurrentHitbox();
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
