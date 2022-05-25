using ExplainingEveryString.Core.Assets;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Math
{
    internal static class AnimationHelper
    {
        internal static Rectangle? GetDrawPart(SpriteData spriteData, Single elapsedTime)
        {
            return GetDrawPart(spriteData, spriteData.AnimationCycle, elapsedTime);
        }

        internal static Rectangle? GetDrawPart(SpriteData spriteData, Single animationCycle, Single elapsedTime)
        {
            if (spriteData.AnimationFrames == 1)
                return null;

            var frameWidth = spriteData.Width;
            var frameTime = animationCycle / spriteData.AnimationFrames;
            var globalFrameNumber = (Int32)(elapsedTime / frameTime);
            var frameNumber = globalFrameNumber % spriteData.AnimationFrames;
            return new Rectangle
            {
                X = frameNumber * frameWidth,
                Y = 0,
                Width = frameWidth,
                Height = spriteData.Height
            };
        }
    }
}
