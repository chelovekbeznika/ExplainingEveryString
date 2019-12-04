using System;
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

        private Sweeper sweeper;
        private Int32 currentTimerValue = 0;
        private Int32 currentWavePhase = 0;

        internal PulseChannel(FrameCounter frameCounter, Boolean firstChannel) : base(frameCounter)
        {
            ChannelParameters = new Dictionary<SoundChannelParameter, Int32>()
            {
                { SoundChannelParameter.Duty, 2 },
                { SoundChannelParameter.Timer, 0 },
                { SoundChannelParameter.Volume, 0 },
                { SoundChannelParameter.SweepEnabled, 0 },
                { SoundChannelParameter.SweepNegate, 0 },
                { SoundChannelParameter.SweepPeriod, 0 },
                { SoundChannelParameter.SweepAmount, 0 },
                { SoundChannelParameter.LengthCounter, 0 },
                { SoundChannelParameter.HaltLoopFlag, 1 },
                { SoundChannelParameter.EnvelopeConstant, 1 }
            };
            FrameCounter.HalfFrame += LengthCounterDecrement;
            FrameCounter.QuarterFrame += DividerDecrement;

            sweeper = new Sweeper(ChannelParameters, firstChannel);
            FrameCounter.HalfFrame += (sender, e) => Timer = sweeper.SweepClock(Timer);
        }

        private Int16 Timer
        {
            get => (Int16) ChannelParameters[SoundChannelParameter.Timer];
            set => ChannelParameters[SoundChannelParameter.Timer] = value;
        }

        private Byte Duty => (Byte)ChannelParameters[SoundChannelParameter.Duty];

        internal override Byte GetOutputValue()
        {
            if (Timer >= 8 && waveForms[Duty][currentWavePhase] && !SilencedByLengthCounter)
                return EnvelopeOutput;
            else
                return 0;
        }

        public override void MoveEmulationTowardNextSample()
        {
            Byte waveGeneratorClockCyclesSwitched = Countdown(ref currentTimerValue, Constants.ApuTicksBetweenSamples, Timer);
            if (waveGeneratorClockCyclesSwitched > 0)
                Countdown(ref currentWavePhase, waveGeneratorClockCyclesSwitched, clockWaveGeneratorCycleStart);
        }

        private class Sweeper
        {
            private Dictionary<SoundChannelParameter, Int32> channelParameters;
            private Boolean firstChannel;
            private Byte currentPeriodValue = 0;

            internal Sweeper(Dictionary<SoundChannelParameter, Int32> channelParameters, Boolean firstChannel)
            {
                this.channelParameters = channelParameters;
                this.firstChannel = firstChannel;
            }

            internal Boolean Enabled => channelParameters[SoundChannelParameter.SweepEnabled] != 0;
            internal Byte Period => (Byte)channelParameters[SoundChannelParameter.SweepPeriod];
            internal Byte Amount => (Byte)channelParameters[SoundChannelParameter.SweepAmount];
            internal Boolean Negate => channelParameters[SoundChannelParameter.SweepNegate] != 0;

            internal Int16 SweepClock(Int16 timer)
            {
                if (Enabled)
                {
                    if (currentPeriodValue == 0)
                    {
                        currentPeriodValue = (Byte)(Period + 1);
                        Int16 change = (Int16)((timer >> Amount) & 0b0111_1111_1111);
                        if (Negate)
                            change = (Int16)(firstChannel ? -change - 1 : -change);
                        Int16 result = (Int16)(timer + change);

                        currentPeriodValue -= 1;
                        return result > 7 && result < 2048 ? result : timer;
                    }
                    currentPeriodValue -= 1;
                    return timer;
                }
                else
                    return timer;
            }
        }
    }
}
