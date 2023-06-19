using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemButton : MenuItem
    {
        protected Texture2D Sprite { get; private set; }

        internal override BorderType BorderType => BorderType.Button;

        internal event EventHandler<EventArgs> ItemCommandExecuteRequested;

        internal MenuItemButton(Texture2D sprite)
        {
            this.Sprite = sprite;
        }

        internal override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Sprite, position, Color.White);
        }

        internal override Point GetSize()
        {
            return new Point(Sprite.Width, Sprite.Height);
        }

        internal override void RequestCommandExecution()
        {
            ItemCommandExecuteRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
