using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class TwoSpritesDisplayer : IMenuItemDisplayble
    {
        private readonly Texture2D baseSprite;
        private readonly Vector2 offset;
        internal Texture2D ChangeableSprite { get; set; }

        internal TwoSpritesDisplayer(Texture2D baseSprite, Vector2 offset, Texture2D changeableSprite)
        {
            this.baseSprite = baseSprite;
            this.offset = offset;
            this.ChangeableSprite = changeableSprite;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(baseSprite, position, Color.White);
            if (ChangeableSprite != null )
                spriteBatch.Draw(ChangeableSprite, position + offset, Color.White);
        }

        public Point GetSize()
        {
            return new Point(baseSprite.Width, baseSprite.Height);
        }
    }
}
