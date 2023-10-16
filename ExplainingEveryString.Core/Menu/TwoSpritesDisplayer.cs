using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal class TwoSpritesDisplayer : IMenuItemDisplayble
    {
        private readonly Texture2D baseSprite;
        internal Vector2 Offset { get; set; }
        internal Texture2D ChangeableSprite { get; set; }

        internal TwoSpritesDisplayer(Texture2D baseSprite, Vector2 offset, Texture2D changeableSprite)
        {
            this.baseSprite = baseSprite;
            this.Offset = offset;
            this.ChangeableSprite = changeableSprite;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(baseSprite, position, Color.White);
            if (ChangeableSprite != null )
                spriteBatch.Draw(ChangeableSprite, position + Offset, Color.White);
        }

        public Point GetSize()
        {
            return new Point(baseSprite.Width, baseSprite.Height);
        }
    }
}
