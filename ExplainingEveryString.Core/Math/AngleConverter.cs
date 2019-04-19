using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Math
{
    internal static class AngleConverter
    {
        internal static Single ToRadians(Vector2 directionVector)
        {
            return (Single)System.Math.Atan2(directionVector.Y, directionVector.X);
        }

        internal static Vector2 ToVector(Single radians)
        {
            return new Vector2 { X = (Single)System.Math.Cos(radians), Y = (Single)System.Math.Sin(radians) };
        }
    }
}
