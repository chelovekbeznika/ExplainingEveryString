using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal abstract class GameObject
    {
        private readonly String spriteName;

        protected internal Vector2 Position { get; protected set; }
        protected abstract Single Width { get; }
        protected abstract Single Height { get; }

        internal String SpriteName { get => spriteName; }

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

        internal GameObject(String spriteName, Vector2 position)
        {
            this.spriteName = spriteName;
            this.Position = position;
        }
    }
}
