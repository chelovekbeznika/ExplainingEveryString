using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal interface IFogOfWarFiller
    {
        List<FogOfWarSpriteEntry> Fill(FogOfWarScreenRegion region, FogOfWarSpecification specification);
    }
}
