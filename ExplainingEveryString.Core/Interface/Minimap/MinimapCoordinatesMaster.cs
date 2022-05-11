using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Interface.Minimap
{
    internal class MinimapCoordinatesMaster
    {
        private const Int32 MinimapSize = 256;
        private readonly Single scale;
        private readonly Vector2 topLeftMapCorner;
        private readonly Single mapHeight;
        internal Matrix PositioningMatrix { get; }

        internal MinimapCoordinatesMaster(TileWrapper map)
        {
            this.scale = System.Math.Max(map.Bounds.Width / MinimapSize, map.Bounds.Height / MinimapSize);
            this.mapHeight = map.Bounds.Height / scale;
            this.topLeftMapCorner = new Vector2(Constants.TargetWidth / 2 - MinimapSize / 2, Constants.TargetHeight - mapHeight);
            this.PositioningMatrix = Matrix.CreateScale(1F / scale) *
                Matrix.CreateTranslation(topLeftMapCorner.X, topLeftMapCorner.Y, 0);
        }

        internal Vector2 ToScreenMinimap(Vector2 levelCoordinates)
        {
            levelCoordinates /= scale;
            levelCoordinates.Y = mapHeight - levelCoordinates.Y;
            levelCoordinates += topLeftMapCorner;
            return levelCoordinates;
        }
    }
}
