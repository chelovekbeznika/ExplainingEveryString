using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Text
{
    internal class FontsStorage
    {
        internal CustomFont SmallNumbers { get; private set; }

        internal void LoadContent(ContentManager content)
        {
            SmallNumbers = new SmallNumbersFont();
            SmallNumbers.Load(content);
        }
    }
}
