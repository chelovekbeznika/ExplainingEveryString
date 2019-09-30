using System;

namespace ExplainingEveryString.Core.Math
{
    internal static class RandomUtility
    {
        private static Random random = new Random();

        internal static Single Next()
        {
            return (Single)random.NextDouble();
        }

        internal static Int32 NextInt(Int32 range)
        {
            return random.Next(range);
        }
    }
}
