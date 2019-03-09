using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core
{
    internal abstract class GameObject
    {
        private readonly String spriteName;

        protected internal Vector2 Position { get; protected set; }
        internal String SpriteName { get => spriteName; }

        internal GameObject(String spriteName, Vector2 position)
        {
            this.spriteName = spriteName;
            this.Position = position;
        }
    }
}
