using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Music
{
    internal class NoiseChannel : SoundChannel
    {
        private readonly Int16[] timerLookupTable = 
            new Int16[] { 4, 8, 16, 32, 64, 96, 128, 160, 202, 254, 380, 508, 762, 1016, 2034, 4068 };

        private UInt16 lfsrValue = 1;
        private Int32 currentTimerValue = 0;

        private Int16 Timer => timerLookupTable[ChannelParameters[SoundChannelParameter.Timer]];

        private Boolean ModeFlagSet => ChannelParameters[SoundChannelParameter.NoiseMode] != 0;

        internal NoiseChannel(FrameCounter frameCounter) : base(frameCounter)
        {
            ChannelParameters = new Dictionary<SoundChannelParameter, Int32>()
            {
                { SoundChannelParameter.Timer, 0 },
                { SoundChannelParameter.NoiseMode, 0 },
                { SoundChannelParameter.Volume, 0 },
                { SoundChannelParameter.LengthCounter, 0 },
                { SoundChannelParameter.HaltLoopFlag, 1 },
                { SoundChannelParameter.EnvelopeConstant, 1 }
            };
            FrameCounter.HalfFrame += LengthCounterDecrement;
            FrameCounter.QuarterFrame += DividerDecrement;
        }

        internal override Byte GetOutputValue()
        {
            if ((lfsrValue & 0b1) != 0 && !SilencedByLengthCounter)
                return 0;
            else
                return EnvelopeOutput;
        }

        public override void MoveEmulationTowardNextSample()
        {
            Int32 shiftBetweenSamples = Countdown(ref currentTimerValue, Constants.ApuTicksBetweenSamples, Timer);
            foreach (var shiftNumber in Enumerable.Range(0, shiftBetweenSamples))
            {
                ShiftLfsr();
            }
        }

        private void ShiftLfsr()
        {
            Boolean feedback = ((ModeFlagSet ? 0b100_0000 : 0b10) & lfsrValue) != 0 ^ (lfsrValue & 1) != 0;
            lfsrValue >>= 1;
            if (feedback)
                lfsrValue |= 0b0100_0000_0000_0000;
        }
    }
}
