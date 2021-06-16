using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Displaying
{
    internal static class Constants
    {
        //NES aspect ratio is 8:7. We're emulating this.
        internal const Int32 BaseWidth = 256;
        internal const Int32 BaseHeiht = 224;
        internal const Int32 TargetWidth = BaseWidth * 4;
        internal const Int32 TargetHeight = BaseHeiht * 4;
    }
}
