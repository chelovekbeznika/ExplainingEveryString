﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal class PulseChannel : SoundChannel
    {
        private const Byte clockWaveGeneratorCycleStart = 7;

        private readonly Boolean[][] waveForms = new Boolean[][] {
            new [] { false, false, false, false, false, false, false, true },
            new [] { false, false, false, false, false, false, true, true },
            new [] { false, false, false, false, true, true, true, true },
            new [] { true, true, true, true, true, true, false, false }
        };

        private Int32 currentTimerValue = 0;
        private Int32 currentWavePhase = 0;

        internal PulseChannel()
        {
            ChannelParameters = new Dictionary<SoundChannelParameter, Int32>()
            {
                { SoundChannelParameter.Duty, 2 },
                { SoundChannelParameter.Timer, 0 },
                { SoundChannelParameter.Volume, 0 },
                { SoundChannelParameter.LengthCounter, 0 },
                { SoundChannelParameter.HaltFlag, 1 },
                { SoundChannelParameter.EnvelopeConstant, 1 }
            };
            FrameCounter.HalfFrame += LengthCounterDecrement;
            FrameCounter.QuarterFrame += DividerDecrement;
        }

        private Int16 Timer => (Int16)ChannelParameters[SoundChannelParameter.Timer];

        private Byte Duty => (Byte)ChannelParameters[SoundChannelParameter.Duty];

        protected override Int16 GetOutputValue()
        {
            if (Timer >= 8 && waveForms[Duty][currentWavePhase] && !SilencedByLengthCounter)
                return (Int16)(Int16.MinValue + EnvelopeOutput * OneVolumeLevelAmplitude);
            else
                return Int16.MinValue;
        }

        protected override void MoveEmulationTowardNextSample()
        {
            Byte waveGeneratorClockCyclesSwitched = Countdown(ref currentTimerValue, Constants.ApuTicksBetweenSamples, Timer);
            if (waveGeneratorClockCyclesSwitched > 0)
                Countdown(ref currentWavePhase, waveGeneratorClockCyclesSwitched, clockWaveGeneratorCycleStart);
        }
    }
}
