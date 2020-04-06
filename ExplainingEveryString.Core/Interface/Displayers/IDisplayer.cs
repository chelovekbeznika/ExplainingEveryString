using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal interface IDisplayer
    {
        String[] GetSpritesNames();
        void InitSprites(Dictionary<String, SpriteData> sprites);
    }
}
