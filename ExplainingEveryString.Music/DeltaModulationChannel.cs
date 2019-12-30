using ExplainingEveryString.Music.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Music
{
    internal class DeltaModulationChannel : SoundChannel
    {
        private Int16[] timerLookupTable = new Int16[] { 428, 380, 340, 320, 286, 254, 226, 214, 190, 160, 142, 128, 106, 84, 72, 54 };
        private Int32 currentTimerValue = 0;

        private Byte[] bitMasks = new Byte[]
        {
            0b0000_0001, 0b0000_0010, 0b0000_0100, 0b0000_1000, 0b0001_0000, 0b0010_0000, 0b0100_0000, 0b1000_0000
        };

        private List<Byte[]> deltaSamplesLibrary;

        private Int32 currentByte = 0;
        private Int32 currentBit = 0;
        private Boolean sampleIsPlaying = false;

        private Int16 Timer => timerLookupTable[ChannelParameters[SoundChannelParameter.Timer]];

        private Byte[] CurrentSample => deltaSamplesLibrary[ChannelParameters[SoundChannelParameter.CurrentSample]];

        internal DeltaModulationChannel(FrameCounter frameCounter, List<Byte[]> deltaSamplesLibrary) : base(frameCounter)
        {
            this.deltaSamplesLibrary = deltaSamplesLibrary;
            ChannelParameters = new Dictionary<SoundChannelParameter, Int32>
            {
                { SoundChannelParameter.Volume, 0 },
                { SoundChannelParameter.Timer, 0 },
                { SoundChannelParameter.CurrentSample, 0 }
            };
        }

        public override void ProcessSoundDirectingEvent(RawSoundDirectingEvent soundEvent)
        {
            base.ProcessSoundDirectingEvent(soundEvent);
            if (soundEvent.Parameter == SoundChannelParameter.CurrentSample)
            {
                currentBit = 0;
                currentByte = 0;
                sampleIsPlaying = true;
            }
        }

        public override void MoveEmulationTowardNextSample()
        {
            var sampleBitsToProcess = Countdown(ref currentTimerValue, Constants.CpuTicksBetweenSamples, Timer);
            if (sampleIsPlaying)
            {
                foreach (var sampleBitIndex in Enumerable.Range(0, sampleBitsToProcess))
                {
                    if (CurrentDeltaSampleBit())
                        VolumeUp();
                    else
                        VolumeDown();
                    ToNextDeltaSampleBit();
                }
            }
        }

        internal override Byte GetOutputValue()
        {
            return sampleIsPlaying ? Volume : (Byte)0;
        }

        private void ToNextDeltaSampleBit()
        {
            currentBit += 1;
            if (currentBit > 7)
            {
                currentBit = 0;
                currentByte += 1;
                if (currentByte >= CurrentSample.Length)
                {
                    currentByte = 0;
                    sampleIsPlaying = false;
                }    
            } 
        }

        private Boolean CurrentDeltaSampleBit()
        {
            return (CurrentSample[currentByte] & bitMasks[currentBit]) != 0;
        }

        private void VolumeUp()
        {
            if (ChannelParameters[SoundChannelParameter.Volume] + 2 < 128)
                ChannelParameters[SoundChannelParameter.Volume] += 2;
        }

        private void VolumeDown()
        {
            if (ChannelParameters[SoundChannelParameter.Volume] - 2 >= 0)
                ChannelParameters[SoundChannelParameter.Volume] -= 2;
        }
    }
}
