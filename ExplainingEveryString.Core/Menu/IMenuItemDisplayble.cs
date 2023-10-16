using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal interface IMenuItemDisplayble
    {
        void Draw(SpriteBatch spriteBatch, Vector2 position);
        Point GetSize();
    }
}
