﻿using ExplainingEveryString.Music.Model;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Music
{
    /// <summary>
    /// https://wiki.nesdev.com/w/index.php/APU_Triangle
    /// Without linear lenght counter (too short and no point in usage of it)
    /// </summary>
    internal class TriangleChannel : SoundChannel
    {
        private const Byte clockWaveGeneratorCycleStart = 31;

        private readonly Byte[] lookupTable = new Byte[]
        {
            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
        };

        private Int32 currentWavePhase = clockWaveGeneratorCycleStart;
        private Int32 currentTimerValue = 0;

        internal TriangleChannel(FrameCounter frameCounter) : base(frameCounter)
        {
            ChannelParameters = new Dictionary<SoundChannelParameter, Int32>
            {
                { SoundChannelParameter.Timer, 0 }
            };
        }

        internal Int16 Timer => (Int16)ChannelParameters[SoundChannelParameter.Timer];

        internal override Byte GetOutputValue()
        {
            return Timer > 0 ? lookupTable[currentWavePhase] : (Byte)0;
        }

        public override void MoveEmulationTowardNextSample()
        {
            var waveGeneratorClockCyclesSwitched = Countdown(ref currentTimerValue, Constants.CpuTicksBetweenSamples, Timer);
            if (waveGeneratorClockCyclesSwitched > 0)
                Countdown(ref currentWavePhase, waveGeneratorClockCyclesSwitched, clockWaveGeneratorCycleStart);
        }
    }
}
