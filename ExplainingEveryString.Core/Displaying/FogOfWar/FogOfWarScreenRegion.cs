using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal class FogOfWarScreenRegion
    {
        internal Rectangle Rectangle { get; set; }
        internal Boolean TouchesScreenAtLeft { get; set; }
        internal Boolean TouchesScreenAtRight { get; set; }
        internal Boolean TouchesScreenAtBottom { get; set; }
        internal Boolean TouchesScreenAtTop { get; set; }
        internal Boolean ScreenFreeOnlyAtLeft => TouchesScreenAtRight && TouchesScreenAtBottom && TouchesScreenAtTop;
        internal Boolean ScreenFreeOnlyAtRight => TouchesScreenAtLeft && TouchesScreenAtBottom && TouchesScreenAtTop;
        internal Boolean ScreenFreeOnlyAtTop => TouchesScreenAtBottom && TouchesScreenAtLeft && TouchesScreenAtRight;
        internal Boolean ScreenFreeOnlyAtBottom => TouchesScreenAtTop && TouchesScreenAtLeft && TouchesScreenAtRight;
    }
}
