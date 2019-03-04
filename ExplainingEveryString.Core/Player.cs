using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core
{
    internal class Player
    {
        private Texture2D sprite;
        private Vector2 position = new Vector2(0, 0);
        private Int32 speed = 100;

        internal Player(Texture2D sprite)
        {
            this.sprite = sprite;
        }

        internal void Move(GameTime gameTime)
        {
            Vector2 direction = Input.GetMoveDirection();
            Vector2 positionChange = direction * speed * (Single)gameTime.ElapsedGameTime.TotalSeconds;
            position += positionChange;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
