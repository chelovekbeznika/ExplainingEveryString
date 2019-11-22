using System;

namespace ExplainingEveryString.Core.Music
{
    internal class Constants
    {
        //ApuFrequency should be close to NES NTSC sound chip frequency as possible
        internal const Int32 ApuFrequency = SampleRate * RarifiyngRate;
        internal const Int32 SampleRate = 47100;
        internal const Int32 RarifiyngRate = 19;
    }
}
