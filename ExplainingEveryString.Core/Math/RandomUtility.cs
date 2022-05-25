using ExplainingEveryString.Data.RandomVariables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Math
{
    internal static class RandomUtility
    {
        private static Random random = new Random();

        internal static Single Next()
        {
            return (Single)random.NextDouble();
        }

        internal static Single Next(Single min, Single max)
        {
            return (Single)random.NextDouble() * (max - min) + min;
        }

        internal static Int32 NextInt(Int32 range)
        {
            return random.Next(range);
        }

        internal static Single NextGauss(GaussRandomVariable randomVar, Boolean canBeNegative = false)
        {
            var normalGauss = System.Math.Cos(random.NextDouble() * 2 * System.Math.PI)
                * System.Math.Sqrt(-2 * System.Math.Log(random.NextDouble()));
            var result = (Single)(randomVar.ExpectedValue + randomVar.Sigma * normalGauss);
            return result > Constants.Epsilon || canBeNegative ? result : Constants.Epsilon;
        }

        internal static T SelectFromProportions<T>(Proportions<T> proportions)
        {
            var num = random.Next(proportions.Sum);
            foreach (var index in Enumerable.Range(0, proportions.Length))
            {
                num -= proportions.Weights[index];
                if (num < 0)
                    return proportions.PossibleValues[index];
            }
            return proportions.PossibleValues.Last();
        }

        internal static List<Int32> IntsFromRange(Int32 elementsToPick, Int32 range)
        {
            var result = new List<Int32>();
            var pool = Enumerable.Range(0, range).ToList();
            foreach (var _ in Enumerable.Range(0, elementsToPick))
            {
                var picked = NextInt(pool.Count);
                result.Add(pool[picked]);
                pool.RemoveAt(picked);
            }
            return result;
        }
    }
}
