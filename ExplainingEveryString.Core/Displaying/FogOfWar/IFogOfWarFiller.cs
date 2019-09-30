using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal interface IFogOfWarFiller
    {
        List<FogOfWarSpriteEntry> Fill(FogOfWarScreenRegion region, Int32 spritesNumbers, Int32 spriteWith, Int32 spriteHeight);
    }
}
