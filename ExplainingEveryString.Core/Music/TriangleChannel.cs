using System;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal class TriangleChannel : SoundChannel
    {
        private const Byte clockWaveGeneratorCycleStart = 31;

        private readonly Byte[] lookupTable = new Byte[]
        {
            15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
        };

        public Byte[] GetNote(UInt16 timer, Int32 durationInSamples)
        {
            Byte[] result = new Byte[durationInSamples * 2];
            Int32 currentTimerValue = timer;
            Int32 currentWavePhase = clockWaveGeneratorCycleStart;

            foreach (Int32 bufferIndex in Enumerable.Range(0, durationInSamples))
            {
                Int16 outputValue = (Int16)(OneVolumeLevelAmplitude * lookupTable[currentWavePhase]);
                PutSample(result, bufferIndex, outputValue);

                Byte waveGeneratorClockCyclesSwitched = Countdown(ref currentTimerValue, Constants.RarifiyngRate * 2, timer);
                if (waveGeneratorClockCyclesSwitched > 0)
                    Countdown(ref currentWavePhase, waveGeneratorClockCyclesSwitched, clockWaveGeneratorCycleStart);
            }

            return result;
        }
    }
}
