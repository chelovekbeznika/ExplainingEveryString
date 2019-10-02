using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal class ScreenFogOfWarDetector : IScreenFogOfWarDetector
    {
        private ILevelCoordinatesMaster levelCoordinatesMaster;
        private IScreenCoordinatesMaster screenCoordinatesMaster;

        internal ScreenFogOfWarDetector(ILevelCoordinatesMaster levelCoordinatesMaster, IScreenCoordinatesMaster screenCoordinatesMaster)
        {
            this.levelCoordinatesMaster = levelCoordinatesMaster;
            this.screenCoordinatesMaster = screenCoordinatesMaster;
        }

        public FogOfWarScreenRegion[] GetFogOfWarRegions(Rectangle[] levelFogOfWarRegions)
        {
            Rectangle levelScreenBounds = levelCoordinatesMaster.ScreenCovers;
            Rectangle screenBounds = screenCoordinatesMaster.ScreenCovers;
            List<FogOfWarScreenRegion> result = new List<FogOfWarScreenRegion>();
            foreach(Rectangle levelRegion in levelFogOfWarRegions)
            {
                Rectangle coveredByScreenRegion = Rectangle.Intersect(levelRegion, levelScreenBounds);
                if (!coveredByScreenRegion.IsEmpty)
                {
                    Rectangle onScreenRegion = GetOnScreenRegion(coveredByScreenRegion);
                    FogOfWarScreenRegion fogOfWarScreenRegion = new FogOfWarScreenRegion
                    {
                        Rectangle = onScreenRegion,
                        TouchesScreenAtBottom = Touches(onScreenRegion.Bottom, screenBounds.Bottom),
                        TouchesScreenAtTop = Touches(onScreenRegion.Top, screenBounds.Top),
                        TouchesScreenAtLeft = Touches(onScreenRegion.Left, screenBounds.Left),
                        TouchesScreenAtRight = Touches(onScreenRegion.Right, screenBounds.Right)
                    };
                    fogOfWarScreenRegion.MakeConsistent(screenBounds);
                    result.Add(fogOfWarScreenRegion);
                }
            }
            return result.ToArray();
        }

        private Boolean Touches(Int32 x, Int32 y)
        {
            return System.Math.Abs(x - y) <= 2;
        }

        private Rectangle GetOnScreenRegion(Rectangle coveredByScreenRegion)
        {
            Vector2 bottomLeftCorner = screenCoordinatesMaster.ConvertToScreenPosition(
                new Vector2(coveredByScreenRegion.Left, coveredByScreenRegion.Top));
            Vector2 topRightCorner = screenCoordinatesMaster.ConvertToScreenPosition(
                new Vector2(coveredByScreenRegion.Right, coveredByScreenRegion.Bottom));
            return new Rectangle
            {
                X = (Int32)bottomLeftCorner.X,
                Y = (Int32)topRightCorner.Y,
                Width = (Int32)(topRightCorner.X - bottomLeftCorner.X),
                Height = (Int32)(bottomLeftCorner.Y - topRightCorner.Y)
            };
        }
    }
}
