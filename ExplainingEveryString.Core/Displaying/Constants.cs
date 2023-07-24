using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal static class Constants
    {
        //NES aspect ratio is 8:7. We're emulating this.
        internal const Int32 BaseWidth = 256;
        internal const Int32 BaseHeight = 224;
        internal const Int32 TargetWidth = BaseWidth * 4;
        internal const Int32 TargetHeight = BaseHeight * 4;

        internal static readonly Color NintendoGold = new Color { R = 255, G = 243, B = 146 };

        internal const Int32 MinimapSize = 192;
        internal const Int32 MinimapFrameThickness = 2;
    }
}
