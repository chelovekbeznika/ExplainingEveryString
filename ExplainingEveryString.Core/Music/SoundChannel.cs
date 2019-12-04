﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal abstract class SoundChannel
    {
        protected const Int16 OneVolumeLevelAmplitude = (Int16.MaxValue - Int16.MinValue) / 15;
        protected Dictionary<SoundChannelParameter, Int32> ChannelParameters { get; set; }
        protected FrameCounter FrameCounter { get; set; }

        protected SoundChannel(FrameCounter frameCounter)
        {
            this.FrameCounter = frameCounter;
        }

        internal void ProcessSoundDirectingEvent(SoundDirectingEvent soundEvent)
        {
            ChannelParameters[soundEvent.Parameter] = soundEvent.Value;
        }

        internal abstract Byte GetOutputValue();

        internal abstract void MoveEmulationTowardNextSample();

        protected Byte Countdown(ref Int32 currentValue, Int32 step, Int32 startCycleAt)
        {
            currentValue -= step;
            Byte result = 0;
            while (currentValue < 0)
            {
                currentValue += (startCycleAt + 1);
                result += 1;
            }
            return result;
        }

        #region Pulse and noise common section with length counter and envelope
        private Byte divider = 0;
        protected Byte Decay { get; set; } = 15;

        protected Boolean HaltFlag => ChannelParameters[SoundChannelParameter.HaltFlag] != 0;

        protected Int32 LengthCounter
        {
            get => ChannelParameters[SoundChannelParameter.LengthCounter];
            set => ChannelParameters[SoundChannelParameter.LengthCounter] = value;
        }

        protected Boolean SilencedByLengthCounter => !HaltFlag && LengthCounter == 0;

        protected Boolean EnvelopeLoopFlag => HaltFlag;

        protected Boolean EnvelopeConstant => ChannelParameters[SoundChannelParameter.EnvelopeConstant] != 0;

        protected Byte Volume => (Byte)ChannelParameters[SoundChannelParameter.Volume];

        protected Byte EnvelopeOutput => EnvelopeConstant ? Volume : Decay;

        protected void LengthCounterDecrement(Object sender, EventArgs e)
        {
            if (!HaltFlag && LengthCounter > 0)
                LengthCounter -= 1;
        }

        protected void DividerDecrement(Object sender, EventArgs e)
        {
            divider -= 1;
            if (divider < 0)
            {
                divider = Volume;
            }
        }

        protected void DecrementDecay()
        {
            Decay -= 1;
            if (Decay == 0 && EnvelopeLoopFlag)
                Decay = 15;
        }
        #endregion
    }
}
