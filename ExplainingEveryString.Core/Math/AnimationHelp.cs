using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Math
{
    internal static class AnimationHelp
    {
        internal static Rectangle? GetDrawPart(SpriteData spriteData, Single elapsedTime)
        {
            return GetDrawPart(spriteData, spriteData.DefaultAnimationCycle, elapsedTime);
        }

        internal static Rectangle? GetDrawPart(SpriteData spriteData, Single animationCycle, Single elapsedTime)
        {
            if (spriteData.AnimationFrames == 1)
                return null;

            Int32 frameWidth = spriteData.Width;
            Single frameTime = animationCycle / spriteData.AnimationFrames;
            Int32 globalFrameNumber = (Int32)(elapsedTime / frameTime);
            Int32 frameNumber = globalFrameNumber % spriteData.AnimationFrames;
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
