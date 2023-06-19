using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class DoubleSpriteMenuItemButton : MenuItemButton
    {
        private Texture2D spriteAbove;
        private Vector2 offset;

        internal DoubleSpriteMenuItemButton(Texture2D spriteAbove, Vector2 offset, Texture2D spriteBelow) : base(spriteBelow)
        {
            this.spriteAbove = spriteAbove;
            this.offset = offset;
        }

        internal override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            base.Draw(spriteBatch, position);
            spriteBatch.Draw(spriteAbove, position + offset, Color.White);
        }
    }
}
