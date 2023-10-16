using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal class OneSpriteDisplayer : IMenuItemDisplayble
    {
        private readonly Texture2D sprite;

        public OneSpriteDisplayer(Texture2D sprite)
        {
            this.sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        public Point GetSize()
        {
            return new Point(sprite.Width, sprite.Height);
        }
    }
}
