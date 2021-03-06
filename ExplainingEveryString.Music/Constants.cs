﻿using System;

namespace ExplainingEveryString.Music
{
    internal static class Constants
    {
        //ApuFrequency should be close to NES NTSC sound chip frequency as possible. Which is 1789773 / 2.
        internal const Int32 ApuFrequency = SampleRate * ApuTicksBetweenSamples;
        internal const Int32 CpuFrequency = SampleRate * CpuTicksBetweenSamples;
        internal const Int32 SampleRate = 47100;
        internal const Int32 ApuTicksBetweenSamples = 19;
        internal const Int32 CpuTicksBetweenSamples = ApuTicksBetweenSamples * 2;
    }
}
