using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal class Mixer
    {
        private Single[] pulseTable;
        private Single[] tndTable;
        private Dictionary<SoundComponentType, ISoundComponent> components;
        private FrameCounter frameCounter;

        private Byte FirstPulseOutput => (components[SoundComponentType.Pulse1] as SoundChannel).GetOutputValue();
        private Byte SecondPulseOutput => (components[SoundComponentType.Pulse2] as SoundChannel).GetOutputValue();
        private Byte TriangleOutput => (components[SoundComponentType.Triangle] as SoundChannel).GetOutputValue();
        private Byte NoiseOutput => (components[SoundComponentType.Noise] as SoundChannel).GetOutputValue();

        internal Mixer()
        {
            this.pulseTable = Enumerable.Range(0, 31).Select(index => 95.52f / (8128.0f / index + 100.0f)).ToArray();
            this.tndTable = Enumerable.Range(0, 203).Select(index => 163.67f / (24329.0f / index + 100.0f)).ToArray();
            this.frameCounter = new FrameCounter();
            this.components = new Dictionary<SoundComponentType, ISoundComponent>()
            {
                { SoundComponentType.FrameCounter, frameCounter },
                { SoundComponentType.Pulse1, new PulseChannel(frameCounter) },
                { SoundComponentType.Pulse2, new PulseChannel(frameCounter) },
                { SoundComponentType.Triangle, new TriangleChannel(frameCounter) },
                { SoundComponentType.Noise, new NoiseChannel(frameCounter) }
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
                    components[soundEvent.SoundComponent].ProcessSoundDirectingEvent(soundEvent);
                    nextEvent += 1;
                }

                PutSample(result, bufferIndex, GetOutputValue());
                MoveEmulationTowardNextSample();
            }

            return result;
        }

        private void MoveEmulationTowardNextSample()
        {
            foreach (ISoundComponent channel in components.Values)
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
