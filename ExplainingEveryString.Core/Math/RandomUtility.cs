using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Math
{
    internal static class RandomUtility
    {
        private static Random random = new Random();

        internal static Single Next()
        {
            return (Single)random.NextDouble();
        }
    }
}
