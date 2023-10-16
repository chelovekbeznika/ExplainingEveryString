using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class KeyPointsTrajectory : BulletTrajectory
    {
        private readonly List<(Single time, Single x, Single y)> keyPoints = new List<(Single time, Single x, Single y)>();

        internal KeyPointsTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(startPosition, fireDirection, parameters) { }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            var index = 0;
            while (parameters.ContainsKey($"{index}_time"))
            {
                keyPoints.Add((parameters[$"{index}_time"], parameters[$"{index}_x"], parameters[$"{index}_y"]));
                index += 1;
            }
            keyPoints.Sort((a, b) => a.time.CompareTo(b.time));
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            var index = 0;
            while (index < keyPoints.Count - 2 && keyPoints[index + 1].time < time)
            {
                index += 1;
            }

            var a = keyPoints[index];
            var b = keyPoints[index + 1];
            var coeff = (time - a.time) / (b.time - a.time);
            return new Vector2
            {
                X = a.x + coeff * (b.x - a.x),
                Y = a.y + coeff * (b.y - a.y)
            };
        }
    }
}
