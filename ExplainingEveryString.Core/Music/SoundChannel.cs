using System;

namespace ExplainingEveryString.Core.Music
{
    internal abstract class SoundChannel
    {
        protected const Int16 OneVolumeLevelAmplitude = (Int16.MaxValue - Int16.MinValue) / 15;

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
