using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal abstract class MenuItem
    {
        internal Boolean Selected { get; set; } = false;
        internal Func<Boolean> IsVisible { get; set; } = () => true;
        internal virtual MenuItemsContainer ParentContainer { get; set; }

        internal abstract BorderType BorderType { get; }
        internal abstract IMenuItemDisplayble Displayble { get; }
        internal abstract void RequestCommandExecution();
        internal virtual void Decrement() { }
        internal virtual void Increment() { }
    }
}
