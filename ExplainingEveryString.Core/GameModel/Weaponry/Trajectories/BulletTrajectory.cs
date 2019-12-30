using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal abstract class BulletTrajectory
    {
        private Vector2 startPosition;
        internal Vector2 FireDirection { get; set; }

        internal BulletTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters)
        {
            this.startPosition = startPosition;
            this.FireDirection = fireDirection;
            AssignParameters(parameters);
        }

        internal Vector2 GetBulletPosition(Single time)
        {
            var rawPosition = GetTrajectoryOffset(time);
            var sinus = FireDirection.Y;
            var cosinus = FireDirection.X;
            var rotatedPosition = new Vector2
            {
                X = rawPosition.X * cosinus - rawPosition.Y * sinus,
                Y = rawPosition.X * sinus + rawPosition.Y * cosinus
            };
            return startPosition + rotatedPosition;
        }

        protected abstract void AssignParameters(Dictionary<String, Single> parameters);
        protected abstract Vector2 GetTrajectoryOffset(Single time);
    }
}
