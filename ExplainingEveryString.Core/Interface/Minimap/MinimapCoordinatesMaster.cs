using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Interface.Minimap
{
    internal class MinimapCoordinatesMaster
    {
        private readonly Single scale;
        private readonly Vector2 topLeftMapCorner;
        private readonly Single mapHeight;
        private readonly Single mapWidth;
        internal (ISpritePartDisplayer, ISpritePartDisplayer, Single) BackgroundPartsToDraw { get; private set; }
        internal Matrix PositioningMatrix { get; }

        internal MinimapCoordinatesMaster(TileWrapper map)
        {
            this.scale = System.Math.Max(map.Bounds.Width / Constants.MinimapSize, map.Bounds.Height / Constants.MinimapSize);
            this.mapHeight = map.Bounds.Height / scale;
            this.mapWidth = map.Bounds.Width / scale;
            this.topLeftMapCorner = new Vector2(
                x: Constants.TargetWidth - Constants.MinimapSize / 2 - mapWidth / 2, 
                y: Constants.TargetHeight - Constants.MinimapSize / 2 - mapHeight / 2);
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

        private (ISpritePartDisplayer, ISpritePartDisplayer, Single) CalculateBackgroundPartsToDraw(TileWrapper map)
        {
            if (scale == map.Bounds.Height / Constants.MinimapSize)
            {
                var coeff = (1 - (mapWidth / Constants.MinimapSize)) / 2;
                return (new LeftPartDisplayer(), new RightPartDisplayer(), coeff);
            }
            else
            {
                var coeff = (1 - (mapHeight / Constants.MinimapSize)) / 2;
                return (new TopPartDisplayer(), new BottomPartDisplayer(), coeff);
            }    
        }
    }
}
