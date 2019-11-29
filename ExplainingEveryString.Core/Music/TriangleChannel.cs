using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
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

        internal TriangleChannel()
        {
            ChannelParameters = new Dictionary<SoundChannelParameter, Int32>
            {
                { SoundChannelParameter.Timer, 0 }
            };
        }

        internal Int16 Timer => (Int16)ChannelParameters[SoundChannelParameter.Timer];

        protected override Int16 GetOutputValue()
        {
            Byte currentValue = Timer > 0 ? lookupTable[currentWavePhase] : (Byte)0;
            return (Int16)(Int16.MinValue + OneVolumeLevelAmplitude * currentValue);
        }

        protected override void MoveEmulationTowardNextSample()
        {
            Byte waveGeneratorClockCyclesSwitched = Countdown(ref currentTimerValue, Constants.ApuTicksBetweenSamples * 2, Timer);
            if (waveGeneratorClockCyclesSwitched > 0)
                Countdown(ref currentWavePhase, waveGeneratorClockCyclesSwitched, clockWaveGeneratorCycleStart);
        }
    }
}
