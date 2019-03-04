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
            Vector2 direction = new Vector2(0, 0);
            GamePadDPad dpad = GamePad.GetState(PlayerIndex.One).DPad;
            if (dpad.Down == ButtonState.Pressed)
                direction += new Vector2(0, 1);
            if (dpad.Up == ButtonState.Pressed)
                direction += new Vector2(0, -1);
            if (dpad.Left == ButtonState.Pressed)
                direction += new Vector2(-1, 0);
            if (dpad.Right == ButtonState.Pressed)
                direction += new Vector2(1, 0);
            Vector2 positionChange = direction * speed * (Single)gameTime.ElapsedGameTime.TotalSeconds;
            position += positionChange;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
