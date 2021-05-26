using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal abstract class MenuItem
    {
        internal Boolean Selected { get; set; } = false;
        internal Func<Boolean> IsVisible { get; set; } = () => true;
        internal virtual MenuItemsContainer ParentContainer { get; set; }

        internal abstract void Draw(SpriteBatch spriteBatch, Vector2 position);
        internal abstract Point GetSize();
        internal abstract void RequestCommandExecution();
        internal virtual void Decrement() { }
        internal virtual void Increment() { }
    }
}
