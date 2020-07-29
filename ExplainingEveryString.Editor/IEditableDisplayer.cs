using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Editor
{
    internal interface IEditableDisplayer
    {
        void Draw(SpriteBatch spriteBatch, String type, Vector2 positionOnScreen, Boolean selected);
    }
}
