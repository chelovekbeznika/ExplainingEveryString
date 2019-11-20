using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Music
{
    internal class Constants
    {
        //ApuFrequency should be close to NES NTSC sound chip frequency as possible
        internal const Int32 ApuFrequency = SampleRate * RarifiyngRate;
        internal const Int32 SampleRate = 47099;
        internal const Int32 RarifiyngRate = 19;
    }
}
