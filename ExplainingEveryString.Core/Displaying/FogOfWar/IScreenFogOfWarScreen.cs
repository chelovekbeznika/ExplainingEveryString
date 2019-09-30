using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal interface IScreenFogOfWarDetector
    {
        FogOfWarScreenRegion[] GetFogOfWarRegions(Rectangle[] levelFogOfWarRegions);
    }
}
