using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{
    internal interface IEditableDisplayer
    {
        void Draw(SpriteBatch spriteBatch, String type, Vector2 positionOnScreen);
    }
}
