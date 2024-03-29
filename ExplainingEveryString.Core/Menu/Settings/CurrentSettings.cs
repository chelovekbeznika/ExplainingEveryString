﻿using ExplainingEveryString.Data.Configuration;
using System;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal class CurrentSettings
    {
        internal const Int32 MaxSoundBars = 10;

        internal Int32 MusicVolume { get; set; }
        internal Int32 SoundVolume { get; set; }
        internal ControlDevice PreferrableControlDevice { get; set; }
        internal Resolution Resolution { get; set; }
        internal Boolean Fullscreen { get; set; }
    }
}
