using System;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal class PulseChannel
    {
        private const Int16 oneVolumeLevelAmplitude = (Int16.MaxValue - Int16.MinValue) / 15;
        private const Byte clockWaveGeneratorCycleStart = 7;

        private readonly Boolean[][] waveForms = new Boolean[][] {
            new [] { false, false, false, false, false, false, false, true },
            new [] { false, false, false, false, false, false, true, true },
            new [] { false, false, false, false, true, true, true, true },
            new [] { true, true, true, true, true, true, false, false }
        };

        public Byte[] GetNote(Byte volume, Byte duty, UInt16 timer, Int32 durationInSamples)
        {
            Byte[] result = new Byte[durationInSamples * 2];
            Int32 currentTimerValue = timer;
            Int32 currentWavePhase = 0;
            foreach (Int32 bufferIndex in Enumerable.Range(0, durationInSamples))
            {
                Int16 outputValue;
                if (waveForms[duty][currentWavePhase])
                    outputValue = (Int16)(Int16.MinValue + volume * oneVolumeLevelAmplitude);
                else
                    outputValue = Int16.MinValue;
                PutSample(result, bufferIndex, outputValue);

                Byte waveGeneratorClockCyclesSwitched = Countdown(ref currentTimerValue, Constants.RarifiyngRate, timer);
                if (waveGeneratorClockCyclesSwitched > 0)
                    Countdown(ref currentWavePhase, waveGeneratorClockCyclesSwitched, clockWaveGeneratorCycleStart);
            }
            return result;
        }

        private Byte Countdown(ref Int32 currentValue, Int32 step, Int32 startCycleAt)
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

        private void PutSample(Byte[] buffer, Int32 position, Int16 value)
        {
            (Byte, Byte) amplitude = ((Byte)value, (Byte)(value >> 8));
            buffer[position * 2] = amplitude.Item1;
            buffer[position * 2 + 1] = amplitude.Item2;
        }
    }
}
