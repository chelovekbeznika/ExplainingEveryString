using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal abstract class SoundChannel
    {
        protected const Int16 OneVolumeLevelAmplitude = (Int16.MaxValue - Int16.MinValue) / 15;
        protected Dictionary<SoundChannelParameter, Int32> ChannelParameters { get; set; }
        protected FrameCounter FrameCounter { get; set; } = new FrameCounter();

        internal Byte[] GetMusic(List<SoundDirectingEvent> soundEvents, Int32 durationInSamples)
        {
            SoundDirectingEvent barrierEvent = new SoundDirectingEvent
            {
                Position = Int32.MaxValue,
                Value = 0,
                Parameter = SoundChannelParameter.Timer
            };
            soundEvents.Add(barrierEvent);
            Byte[] result = new Byte[durationInSamples * 2];
            Int32 nextEvent = 0;

            foreach (Int32 bufferIndex in Enumerable.Range(0, durationInSamples))
            {
                while (soundEvents[nextEvent].Position == bufferIndex)
                {
                    SoundDirectingEvent soundEvent = soundEvents[nextEvent];
                    ChannelParameters[soundEvent.Parameter] = soundEvent.Value;
                    nextEvent += 1;
                }

                PutSample(result, bufferIndex, GetOutputValue());
                FrameCounter.MoveEmulationForward(Constants.ApuTicksBetweenSamples);
                MoveEmulationTowardNextSample();
            }

            return result;
        }

        protected abstract Int16 GetOutputValue();

        protected abstract void MoveEmulationTowardNextSample();

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

        protected void PutSample(Byte[] buffer, Int32 position, Int16 value)
        {
            (Byte, Byte) amplitude = ((Byte)value, (Byte)(value >> 8));
            buffer[position * 2] = amplitude.Item1;
            buffer[position * 2 + 1] = amplitude.Item2;
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
