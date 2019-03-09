using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core
{
    internal abstract class GameObject
    {
        private String spriteName;
        private EesGame game;

        protected Vector2 Position { get; set; }

        internal GameObject(EesGame game, String spriteName, Vector2 position)
        {
            this.game = game;
            this.spriteName = spriteName;
            this.Position = position;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Texture2D sprite = game.SpritesStorage[spriteName];
            Vector2 drawPosition = new Vector2() { X = Position.X - sprite.Width / 2, Y = Position.Y - sprite.Height / 2 };
            spriteBatch.Draw(sprite, drawPosition, Color.White);
        }
    }
}
