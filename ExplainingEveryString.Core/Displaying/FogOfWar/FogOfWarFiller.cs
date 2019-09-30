using System;
using System.Collections.Generic;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal class FogOfWarFiller : IFogOfWarFiller
    {
        private readonly Int32 seed;
        private Int32 spritesNumber;
        private Int32 spriteWidth;
        private Int32 spriteHeight;
        private Random random;

        internal FogOfWarFiller()
        {
            seed = RandomUtility.NextInt(Int32.MaxValue);
        }

        public List<FogOfWarSpriteEntry> Fill(FogOfWarScreenRegion region, Int32 spritesNumber, Int32 spriteWidth, Int32 spriteHeight)
        {
            this.random = new Random(seed);
            this.spritesNumber = spritesNumber;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            if (BottomRightCornerAcceptable(region))
                return FromBottomRight(region);
            if (BottomLeftCornerAcceptable(region))
                return FromBottomLeft(region);
            if (TopRightCornerAcceptable(region))
                return FromTopRight(region);
            if (TopLeftCornerAcceptable(region))
                return FromTopLeft(region);
            throw new ArgumentException($"Unsupported type of region {region.Rectangle.Width}X{region.Rectangle.Height}");
        }

        private Boolean BottomRightCornerAcceptable(FogOfWarScreenRegion region)
        {
            return (!region.TouchesScreenAtBottom && !region.TouchesScreenAtRight);
        }

        private Boolean BottomLeftCornerAcceptable(FogOfWarScreenRegion region)
        {
            return (!region.TouchesScreenAtBottom && !region.TouchesScreenAtLeft) || region.ScreenFreeOnlyAtLeft || region.ScreenFreeOnlyAtBottom;
        }

        private Boolean TopRightCornerAcceptable(FogOfWarScreenRegion region)
        {
            return (!region.TouchesScreenAtTop && !region.TouchesScreenAtRight) || region.ScreenFreeOnlyAtRight || region.ScreenFreeOnlyAtTop;
        }

        private Boolean TopLeftCornerAcceptable(FogOfWarScreenRegion region)
        {
            return (!region.TouchesScreenAtTop && !region.TouchesScreenAtLeft);
        }

        private List<FogOfWarSpriteEntry> FromBottomRight(FogOfWarScreenRegion region)
        {
            return PopulateList(RightEdge(region), BottomEdge(region), LeftEdge(region), TopEdge(region), -spriteWidth, -spriteHeight);
        }

        private List<FogOfWarSpriteEntry> FromBottomLeft(FogOfWarScreenRegion region)
        {
            return PopulateList(LeftEdge(region), BottomEdge(region), RightEdge(region), TopEdge(region), spriteWidth, -spriteHeight);
        }

        private List<FogOfWarSpriteEntry> FromTopRight(FogOfWarScreenRegion region)
        {
            return PopulateList(RightEdge(region), TopEdge(region), LeftEdge(region), BottomEdge(region), -spriteWidth, spriteHeight);
        }

        private List<FogOfWarSpriteEntry> FromTopLeft(FogOfWarScreenRegion region)
        {
            return PopulateList(LeftEdge(region), TopEdge(region), RightEdge(region), BottomEdge(region), spriteWidth, spriteHeight);
        }

        private List<FogOfWarSpriteEntry> PopulateList(Int32 startX, Int32 startY, Int32 finishX, Int32 finishY, Int32 xStep, Int32 yStep)
        {
            Int32 currentX = startX;
            List<FogOfWarSpriteEntry> result = new List<FogOfWarSpriteEntry>();
            while (xStep > 0 ? currentX <= finishX : currentX >= finishX)
            {
                Int32 currentY = startY;
                while (yStep > 0 ? currentY <= finishY : currentY >= finishY)
                {
                    AddSprite(result, currentX, currentY);
                    currentY += yStep;
                }
                currentX += xStep;
            }
            return result;
        }

        private Int32 RightEdge(FogOfWarScreenRegion region) =>
            region.Rectangle.Right - Offset(spriteWidth, region.TouchesScreenAtRight);

        private Int32 LeftEdge(FogOfWarScreenRegion region) =>
            region.Rectangle.Left + Offset(spriteWidth, region.TouchesScreenAtLeft);

        private Int32 BottomEdge(FogOfWarScreenRegion region) =>
            region.Rectangle.Bottom - Offset(spriteHeight, region.TouchesScreenAtBottom);

        private Int32 TopEdge(FogOfWarScreenRegion region) => 
            region.Rectangle.Top + Offset( spriteHeight, region.TouchesScreenAtTop);

        private Int32 Offset(Int32 spriteMeasure, Boolean touchesScreen) =>
            touchesScreen ? -spriteMeasure / 2 : spriteMeasure / 2;

        private void AddSprite(List<FogOfWarSpriteEntry> list, Int32 x, Int32 y)
        {
            list.Add(new FogOfWarSpriteEntry
            {
                ScreenPosition = new Point(x, y),
                SpriteNumber = random.Next(spritesNumber)
            });
        }
    }
}
