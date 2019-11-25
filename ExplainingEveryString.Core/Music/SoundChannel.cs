using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal abstract class SoundChannel
    {
        protected const Int16 OneVolumeLevelAmplitude = (Int16.MaxValue - Int16.MinValue) / 15;
        protected Dictionary<SoundChannelParameter, Int32> ChannelParameters { get; set; }

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
    }
}
