using System;
using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal class LevelFogOfWarExtractor : ILevelFogOfWarExtractor
    {
        private const Int32 Infinity = 1000_000_000;
        private ITileCoordinatesMaster tileCoordinatesMaster;

        public LevelFogOfWarExtractor(ITileCoordinatesMaster tileCoordinatesMaster)
        {
            this.tileCoordinatesMaster = tileCoordinatesMaster;
        }

        public Rectangle[] GetFogOfWarRegions()
        {
            var levelBounds = tileCoordinatesMaster.Bounds;
            //0  1  2
            //7 L_B 3
            //6  5  4
            //In rectangle Bottom and Top inverted for screen coordinates =(
            return new Rectangle[]
            {
                new Rectangle(levelBounds.Left - Infinity, levelBounds.Bottom, Infinity, Infinity),
                new Rectangle(levelBounds.Left, levelBounds.Bottom, levelBounds.Width, Infinity),
                new Rectangle(levelBounds.Right, levelBounds.Bottom, Infinity, Infinity),
                new Rectangle(levelBounds.Right, levelBounds.Top, Infinity, levelBounds.Height),
                new Rectangle(levelBounds.Right, levelBounds.Top - Infinity, Infinity, Infinity),
                new Rectangle(levelBounds.Left, levelBounds.Top - Infinity, levelBounds.Width, Infinity),
                new Rectangle(levelBounds.Left - Infinity, levelBounds.Top - Infinity, Infinity, Infinity),
                new Rectangle(levelBounds.Left - Infinity, levelBounds.Top, Infinity, levelBounds.Height)
            };
        }
    }
}