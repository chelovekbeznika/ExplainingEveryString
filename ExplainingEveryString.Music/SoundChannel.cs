using ExplainingEveryString.Music.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Music
{
    internal abstract class SoundChannel : ISoundComponent
    {
        protected const Int16 OneVolumeLevelAmplitude = (Int16.MaxValue - Int16.MinValue) / 15;
        protected Dictionary<SoundChannelParameter, Int32> ChannelParameters { get; set; }
        protected FrameCounter FrameCounter { get; set; }

        protected SoundChannel(FrameCounter frameCounter)
        {
            this.FrameCounter = frameCounter;
        }

        public virtual void ProcessSoundDirectingEvent(RawSoundDirectingEvent soundEvent)
        {
            ChannelParameters[soundEvent.Parameter] = soundEvent.Value;
            if (soundEvent.Parameter == SoundChannelParameter.Timer || soundEvent.Parameter == SoundChannelParameter.LengthCounter)
                startFlag = true;
        }

        internal abstract Byte GetOutputValue();

        public abstract void MoveEmulationTowardNextSample();

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
        private Boolean startFlag = false;
        private Byte divider = 0;
        protected Byte Decay { get; set; } = 15;

        protected Boolean HaltFlag => ChannelParameters[SoundChannelParameter.HaltLoopFlag] != 0;

        protected Int32 LengthCounter
        {
            get => ChannelParameters[SoundChannelParameter.LengthCounter];
            set => ChannelParameters[SoundChannelParameter.LengthCounter] = value;
        }

        protected Boolean SilencedByLengthCounter => !HaltFlag && LengthCounter == 0;

        protected Boolean EnvelopeLoopFlag => ChannelParameters[SoundChannelParameter.HaltLoopFlag] != 0;

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
            if (startFlag)
            {
                divider = (Byte)(Volume + 1);
                Decay = 15;
                startFlag = false;
            }
            if (divider == 0)
            {
                DecrementDecay();
                divider = (Byte)(Volume + 1);
            }
            divider -= 1;
        }

        protected void DecrementDecay()
        {
            if (Decay == 0 && EnvelopeLoopFlag)
                Decay = 15;
            if (Decay > 0)
                Decay -= 1;
        }
        #endregion
    }
}
