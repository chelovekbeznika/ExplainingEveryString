using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal static class GameTimeHelper
    {
        internal static string ToTimeString(float time)
        {
            var timeSpan = TimeSpan.FromSeconds(time);
            return $"{timeSpan:h\\:mm\\:ss\\.ff}";
        }
    }
}
