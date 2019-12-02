using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal class Mixer
    {
        private Single[] pulseTable;
        private Single[] tndTable;
        private Dictionary<SoundChannelType, SoundChannel> channels;
        private FrameCounter frameCounter;

        private Byte FirstPulseOutput => channels[SoundChannelType.Pulse1].GetOutputValue();
        private Byte SecondPulseOutput => channels[SoundChannelType.Pulse2].GetOutputValue();
        private Byte TriangleOutput => channels[SoundChannelType.Triangle].GetOutputValue();
        private Byte NoiseOutput => channels[SoundChannelType.Noise].GetOutputValue();

        internal Mixer()
        {
            this.pulseTable = Enumerable.Range(0, 31).Select(index => 95.52f / (8128.0f / index + 100.0f)).ToArray();
            this.tndTable = Enumerable.Range(0, 203).Select(index => 163.67f / (24329.0f / index + 100.0f)).ToArray();
            this.frameCounter = new FrameCounter();
            this.channels = new Dictionary<SoundChannelType, SoundChannel>()
            {
                { SoundChannelType.Pulse1, new PulseChannel(frameCounter) },
                { SoundChannelType.Pulse2, new PulseChannel(frameCounter) },
                { SoundChannelType.Triangle, new TriangleChannel(frameCounter) },
                { SoundChannelType.Noise, new NoiseChannel(frameCounter) }
            };
        }

        internal Byte[] GetMusic(List<SoundDirectingEvent> soundEvents, Single durationInSeconds)
        {
            Int32 durationInSamples = (Int32)System.Math.Floor(durationInSeconds * Constants.SampleRate);
            SoundDirectingEvent barrierEvent = new SoundDirectingEvent
            {
                Seconds = Int32.MaxValue / Constants.SampleRate,
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
                    channels[soundEvent.SoundChannel].ProcessSoundDirectingEvent(soundEvent);
                    nextEvent += 1;
                }

                PutSample(result, bufferIndex, GetOutputValue());
                MoveEmulationTowardNextSample();
            }

            return result;
        }

        private void MoveEmulationTowardNextSample()
        {
            frameCounter.MoveEmulationForward();
            foreach (SoundChannel channel in channels.Values)
            {
                channel.MoveEmulationTowardNextSample();
            }
        }

        private Single GetOutputValue()
        {
            Single pulseOutput = pulseTable[FirstPulseOutput + SecondPulseOutput];
            Single tndOutput = tndTable[3 * TriangleOutput + 2 * NoiseOutput];
            return pulseOutput + tndOutput;
        }

        private void PutSample(Byte[] buffer, Int32 position, Single value)
        {
            Int16 pcmValue = (Int16)(Int16.MinValue + (Int16.MaxValue - Int16.MinValue) * value);
            (Byte, Byte) amplitude = ((Byte)value, (Byte)(pcmValue >> 8));
            buffer[position * 2] = amplitude.Item1;
            buffer[position * 2 + 1] = amplitude.Item2;
        }
    }
}
