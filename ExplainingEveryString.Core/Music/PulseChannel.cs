using System;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal class PulseChannel
    {
        private Boolean[] waveForm = new Boolean[] { false, false, false, false, true, true, true, true };

        public Byte[] GetMusic()
        {
            Byte[] result = new Byte[Constants.SampleRate * 2];
            Int32 timer = 253;
            Int32 currentTimerValue = timer;
            Int32 currentWavePhase = 0;
            foreach (Int32 bufferIndex in Enumerable.Range(0, Constants.SampleRate))
            {
                if (waveForm[currentWavePhase])
                    PutSample(result, bufferIndex, Int16.MaxValue);
                else
                    PutSample(result, bufferIndex, Int16.MinValue);

                if (Countdown(ref currentTimerValue, Constants.RarifiyngRate, timer))
                    Countdown(ref currentWavePhase, 1, 7);
            }
            return result;
        }

        private Boolean Countdown(ref Int32 currentValue, Int32 step, Int32 startCycleAt)
        {
            currentValue -= step;
            if (currentValue < 0)
            {
                currentValue += (startCycleAt + 1);
                return true;
            }
            else
                return false;
        }

        private void PutSample(Byte[] buffer, Int32 position, Int16 value)
        {
            (Byte, Byte) amplitude = ((Byte)value, (Byte)(value >> 8));
            buffer[position * 2] = amplitude.Item1;
            buffer[position * 2 + 1] = amplitude.Item2;
        }
    }
}
