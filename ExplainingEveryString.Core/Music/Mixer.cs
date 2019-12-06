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
        private StatusController statusController;

        private Byte FirstPulseOutput => statusController.Pulse1Enabled 
            ? (components[SoundComponentType.Pulse1] as SoundChannel).GetOutputValue() : (Byte)0;
        private Byte SecondPulseOutput => statusController.Pulse2Enabled 
            ? (components[SoundComponentType.Pulse2] as SoundChannel).GetOutputValue() : (Byte)0;
        private Byte TriangleOutput => statusController.TriangleEnabled
            ? (components[SoundComponentType.Triangle] as SoundChannel).GetOutputValue() : (Byte)0;
        private Byte NoiseOutput => statusController.NoiseEnabled
            ? (components[SoundComponentType.Noise] as SoundChannel).GetOutputValue() : (Byte)0;
        private Byte DmcOutput => statusController.DeltaEnabled
            ? (components[SoundComponentType.DeltaModulation] as SoundChannel).GetOutputValue() : (Byte)0;

        internal List<Byte[]> DeltaSamplesLibrary => 
            ((DeltaModulationChannel)components[SoundComponentType.DeltaModulation]).DeltaSamplesLibrary;

        internal Mixer()
        {
            this.pulseTable = Enumerable.Range(0, 31).Select(index => 95.52f / (8128.0f / index + 100.0f)).ToArray();
            this.tndTable = Enumerable.Range(0, 203).Select(index => 163.67f / (24329.0f / index + 100.0f)).ToArray();

            this.frameCounter = new FrameCounter();
            this.statusController = new StatusController();
            this.components = new Dictionary<SoundComponentType, ISoundComponent>()
            {
                { SoundComponentType.FrameCounter, frameCounter },
                { SoundComponentType.Status, statusController },
                { SoundComponentType.Pulse1, new PulseChannel(frameCounter, true) },
                { SoundComponentType.Pulse2, new PulseChannel(frameCounter, false) },
                { SoundComponentType.Triangle, new TriangleChannel(frameCounter) },
                { SoundComponentType.Noise, new NoiseChannel(frameCounter) },
                { SoundComponentType.DeltaModulation, new DeltaModulationChannel(frameCounter) }
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
            Single tndOutput = tndTable[3 * TriangleOutput + 2 * NoiseOutput + DmcOutput];
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
