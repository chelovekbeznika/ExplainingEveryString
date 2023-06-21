using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemButton : MenuItem
    {
        internal override IMenuItemDisplayble Displayble { get; }
        internal override BorderType BorderType => BorderType.Button;

        internal event EventHandler<EventArgs> ItemCommandExecuteRequested;

        internal MenuItemButton(IMenuItemDisplayble displayble)
        {
            Displayble = displayble;
        }

        internal override void RequestCommandExecution()
        {
            ItemCommandExecuteRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
