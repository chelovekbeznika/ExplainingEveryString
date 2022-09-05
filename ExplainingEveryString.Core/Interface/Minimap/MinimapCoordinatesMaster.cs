using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Minimap
{
    internal class MinimapCoordinatesMaster
    {
        private readonly Single scale;
        private readonly Vector2 topLeftMapCorner;
        private readonly Single mapHeight;
        private readonly Single mapWidth;
        internal List<(ISpritePartDisplayer, Single)> BackgroundPartsToDraw { get; private set; }
        internal Matrix PositioningMatrix { get; }

        internal MinimapCoordinatesMaster(TileWrapper map)
        {
            this.scale = System.Math.Max((Single)map.Bounds.Width / Constants.MinimapSize, (Single)map.Bounds.Height / Constants.MinimapSize);
            this.mapHeight = map.Bounds.Height / scale;
            this.mapWidth = map.Bounds.Width / scale;
            this.topLeftMapCorner = new Vector2(
                x: Constants.TargetWidth - Constants.MinimapSize / 2 - mapWidth / 2 - Constants.MinimapFrameThickness, 
                y: Constants.TargetHeight - Constants.MinimapSize / 2 - mapHeight / 2 - Constants.MinimapFrameThickness);
            this.PositioningMatrix = Matrix.CreateScale(1F / scale) *
                Matrix.CreateTranslation(topLeftMapCorner.X, topLeftMapCorner.Y, 0);
            this.BackgroundPartsToDraw = CalculateBackgroundPartsToDraw(map);
        }

        internal Vector2 ToScreenMinimap(Vector2 levelCoordinates)
        {
            levelCoordinates /= scale;
            levelCoordinates.Y = mapHeight - levelCoordinates.Y;
            levelCoordinates += topLeftMapCorner;
            return levelCoordinates;
        }

        private List<(ISpritePartDisplayer, Single)> CalculateBackgroundPartsToDraw(TileWrapper map)
        {
            var frameCoeff = (Single)Constants.MinimapFrameThickness / (Constants.MinimapSize + Constants.MinimapFrameThickness * 2);
            if (System.Math.Abs(scale - (Single)map.Bounds.Height / Constants.MinimapSize) <= Math.Constants.Epsilon)
            {
                var coeff = (1 - (mapWidth / (Constants.MinimapSize + Constants.MinimapFrameThickness * 2))) / 2;
                return new List<(ISpritePartDisplayer, float)>
                {
                    (new LeftPartDisplayer(), coeff),
                    (new RightPartDisplayer(), coeff),
                    (new TopPartDisplayer(), frameCoeff),
                    (new BottomPartDisplayer(), frameCoeff)
                };
            }
            else
            {
                var coeff = (1 - (mapHeight / (Constants.MinimapSize + Constants.MinimapFrameThickness * 2))) / 2;
                return new List<(ISpritePartDisplayer, float)>
                {
                    (new TopPartDisplayer(), coeff), 
                    (new BottomPartDisplayer(), coeff),
                    (new LeftPartDisplayer(), frameCoeff),
                    (new RightPartDisplayer(), frameCoeff)
                };
            }    
        }
    }
}
