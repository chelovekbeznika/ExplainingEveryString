using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItem
    {
        internal Texture2D Sprite { get; private set; }
        internal Boolean Selected { get; set; } = false;
        internal EventHandler<EventArgs> ItemCommandExecuteRequested;

        internal MenuItem(Texture2D sprite)
        {
            this.Sprite = sprite;
        }

        internal Point GetSize()
        {
            return new Point(Sprite.Width, Sprite.Height);
        }

        internal void RequestCommandExecution()
        {
            ItemCommandExecuteRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
