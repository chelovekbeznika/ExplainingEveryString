using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemButton : MenuItem
    {
        private Texture2D sprite;
        internal event EventHandler<EventArgs> ItemCommandExecuteRequested;

        internal MenuItemButton(Texture2D sprite)
        {
            this.sprite = sprite;
        }

        internal override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        internal override Point GetSize()
        {
            return new Point(sprite.Width, sprite.Height);
        }

        internal override void RequestCommandExecution()
        {
            ItemCommandExecuteRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
