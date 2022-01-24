using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal interface IScreenFogOfWarDetector
    {
        FogOfWarScreenRegion[] GetFogOfWarRegions(Rectangle[] levelFogOfWarRegions);
    }
}
