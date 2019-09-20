using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal interface ISpritePartDisplayer
    {
        Rectangle GetDrawPart(SpriteData spriteData, Single coeff);
        Vector2 GetDrawPosition(SpriteData spriteData, Single coeff, Vector2 wholeSpritePosition);
    }

    internal class LeftPartDisplayer : ISpritePartDisplayer
    {
        public Rectangle GetDrawPart(SpriteData spriteData, Single coeff)
        {
            return new Rectangle(0, 0, PixelsToDraw(spriteData.Width, coeff), spriteData.Height);
        }

        public Vector2 GetDrawPosition(SpriteData spriteData, Single coeff, Vector2 wholeSpritePosition)
        {
            return wholeSpritePosition;
        }

        private Int32 PixelsToDraw(Int32 width, Single coeff)
        {
            return (Int32)System.Math.Floor(coeff * width);
        }
    }

    internal class RightPartDisplayer : ISpritePartDisplayer
    {
        public Rectangle GetDrawPart(SpriteData spriteData, Single coeff)
        {
            Int32 pixelsToDraw = PixelsToDraw(spriteData.Width, coeff);
            Int32 leftMargin = LeftMargin(spriteData, coeff);
            return new Rectangle(leftMargin, 0, pixelsToDraw, spriteData.Sprite.Height);
        }

        public Vector2 GetDrawPosition(SpriteData spriteData, Single coeff, Vector2 wholeSpritePosition)
        {
            return wholeSpritePosition + new Vector2(LeftMargin(spriteData, coeff), 0);
        }

        private Int32 LeftMargin(SpriteData spriteData, Single coeff)
        {
            Int32 pixelsToDraw = PixelsToDraw(spriteData.Width, coeff);
            return spriteData.Width - pixelsToDraw;
        }

        private Int32 PixelsToDraw(Int32 width, Single coeff)
        {
            return (Int32)System.Math.Ceiling(coeff * width);
        }
    }

    internal class CenterPartDisplayer : ISpritePartDisplayer
    {
        public Rectangle GetDrawPart(SpriteData spriteData, Single coeff)
        {
            Int32 cutXPixels = CutXPixels(spriteData, coeff);
            Int32 cutYPixels = CutYPixels(spriteData, coeff);
            return new Rectangle(cutXPixels, cutYPixels, spriteData.Width - cutXPixels * 2, spriteData.Height - cutYPixels * 2);
        }

        public Vector2 GetDrawPosition(SpriteData spriteData, Single coeff, Vector2 wholeSpritePosition)
        {
            return wholeSpritePosition + new Vector2(CutXPixels(spriteData, coeff), CutYPixels(spriteData, coeff));
        }

        private Int32 CutXPixels(SpriteData spriteData, Single coeff)
        {
            return (Int32)(spriteData.Width / 2 * (1 - coeff));
        }

        private Int32 CutYPixels(SpriteData spriteData, Single coeff)
        {
            return (Int32)(spriteData.Height / 2 * (1 - coeff));
        }
    }

    internal class WholeSpriteDisplayer : ISpritePartDisplayer
    {
        public Rectangle GetDrawPart(SpriteData spriteData, Single coeff)
        {
            return new Rectangle(0, 0, spriteData.Width, spriteData.Height);
        }

        public Vector2 GetDrawPosition(SpriteData spriteData, Single coeff, Vector2 wholeSpritePosition)
        {
            return wholeSpritePosition;
        }
    }
}
