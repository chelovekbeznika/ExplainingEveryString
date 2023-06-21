using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal interface IMenuItemDisplayble
    {
        void Draw(SpriteBatch spriteBatch, Vector2 position);
        Point GetSize();
    }
}
