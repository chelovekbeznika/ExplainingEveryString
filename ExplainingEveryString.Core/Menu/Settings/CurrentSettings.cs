using ExplainingEveryString.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal class CurrentSettings
    {
        internal const Int32 MaxSoundBars = 10;

        internal Int32 MusicVolume { get; set; }
        internal Int32 SoundVolume { get; set; }
        internal ControlDevice PreferrableControlDevice { get; set; }
        internal Resolution Resolution { get; set; }
    }
}
