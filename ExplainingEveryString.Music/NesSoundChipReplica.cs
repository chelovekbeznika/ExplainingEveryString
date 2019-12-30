using ExplainingEveryString.Music.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExplainingEveryString.Music
{
    internal class NesSoundChipReplica
    {
        private const Int32 OnePartLength = Constants.SampleRate * 5;
        private Dictionary<SoundComponentType, ISoundComponent> components;
        private FrameCounter frameCounter;
        private StatusController statusController;

        private Queue<Byte[]> generatedSongParts = new Queue<Byte[]>();
        private readonly Object generatedSongPartsLock = new Object();
        private AutoResetEvent firstPartGenerated = new AutoResetEvent(false);

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

        internal NesSoundChipReplica(List<Byte[]> deltaSamplesLibrary)
        {
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
                { SoundComponentType.DeltaModulation, new DeltaModulationChannel(frameCounter, deltaSamplesLibrary) }
            };
        }

        internal List<Byte[]> GetGeneratedParts()
        {
            lock (generatedSongPartsLock)
            {
                if (generatedSongParts.Count == 0)
                    return null;

                var parts = new List<Byte[]>();
                while (generatedSongParts.Count > 0)
                    parts.Add(generatedSongParts.Dequeue());
                return parts;
            }
        }

        internal Byte[] StartMusicGeneration(SongSpecification songSpecification)
        {
            ThreadPool.QueueUserWorkItem(MusicGeneration, songSpecification);
            firstPartGenerated.WaitOne();
            lock (generatedSongPartsLock)
                return generatedSongParts.Dequeue();
        }

        private void MusicGeneration(Object songSpecificationObject)
        {
            var songSpecification = songSpecificationObject as SongSpecification;
            var soundEvents = GetRawEvents(songSpecification.Song);
            var durationInSamples = (Int32)Math.Round(songSpecification.Duration * Constants.SampleRate);
            Byte[] currentPart = null;
            var nextEvent = 0;

            foreach (var sampleIndex in Enumerable.Range(0, durationInSamples))
            {
                currentPart = GetCurrentSongPart(currentPart, sampleIndex, durationInSamples);
                nextEvent = MoveEventsQueue(soundEvents, nextEvent, sampleIndex);
                PutSample(currentPart, sampleIndex % OnePartLength, GetOutputValue());
                MoveEmulationTowardNextSample();
            }

            lock (generatedSongPartsLock)
                generatedSongParts.Enqueue(currentPart);        
        }

        private List<RawSoundDirectingEvent> GetRawEvents(List<ISoundDirectingSequence> soundSequences)
        {
            var soundEvents = soundSequences
                .SelectMany(sequence => sequence.GetEvents())
                .OrderBy(soundEvent => soundEvent.Position)
                .ToList();

            var barrierEvent = new RawSoundDirectingEvent
            {
                Seconds = Int32.MaxValue / Constants.SampleRate,
                Value = 0,
                Parameter = SoundChannelParameter.Timer
            };
            soundEvents.Add(barrierEvent);

            return soundEvents;
        }

        private Byte[] GetCurrentSongPart(Byte[] currentPart, Int32 sampleIndex, Int32 durationInSamples)
        {
            if (sampleIndex % OnePartLength == 0)
            {
                if (currentPart != null)
                {
                    lock (generatedSongPartsLock)
                    {
                        generatedSongParts.Enqueue(currentPart);
                    }
                    if (sampleIndex <= OnePartLength)
                        firstPartGenerated.Set();
                }
                currentPart = new Byte[Math.Min(durationInSamples - sampleIndex, OnePartLength) * 2];
            }
            return currentPart;
        }

        private Int32 MoveEventsQueue(List<RawSoundDirectingEvent> soundEvents, Int32 nextEvent, Int32 sampleIndex)
        {
            while (soundEvents[nextEvent].Position == sampleIndex)
            {
                var soundEvent = soundEvents[nextEvent];
                components[soundEvent.SoundComponent].ProcessSoundDirectingEvent(soundEvent);
                nextEvent += 1;
            }
            return nextEvent;
        }

        private Single GetOutputValue()
        {
            var pulseOutput = FirstPulseOutput + SecondPulseOutput > 0 
                ? (Single)(95.88 / (8128 / (FirstPulseOutput + SecondPulseOutput) + 100)) : 0;
            var tndOutput = TriangleOutput + NoiseOutput + DmcOutput > 0
                ? (Single)(159.79 / (1 / (TriangleOutput / 8227.0 + NoiseOutput / 12241.0 + DmcOutput / 22638.0) + 100)): 0;
            return pulseOutput + tndOutput;
        }

        private void PutSample(Byte[] buffer, Int32 position, Single value)
        {
            var pcmValue = (Int16)(Int16.MinValue + (Int16.MaxValue - Int16.MinValue) * value);
            var amplitude = ((Byte)value, (Byte)(pcmValue >> 8));
            buffer[position * 2] = amplitude.Item1;
            buffer[position * 2 + 1] = amplitude.Item2;
        }

        private void MoveEmulationTowardNextSample()
        {
            foreach (var channel in components.Values)
            {
                channel.MoveEmulationTowardNextSample();
            }
        }
    }
}
