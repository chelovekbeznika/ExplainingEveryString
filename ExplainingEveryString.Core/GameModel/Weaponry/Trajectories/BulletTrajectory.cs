using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal abstract class BulletTrajectory
    {
        internal Vector2 Center { get; set; }
        internal Vector2 FireDirection { get; set; }

        internal BulletTrajectory(Vector2 center, Vector2 fireDirection, Dictionary<String, Single> parameters)
        {
            this.Center = center;
            this.FireDirection = fireDirection;
            AssignParameters(parameters);
        }

        internal Vector2 GetBulletPosition(Single time)
        {
            var rawPosition = GetTrajectoryOffset(time);
            var rotatedPosition = GeometryHelper.RotateVector(rawPosition, FireDirection.Y, FireDirection.X);
            return Center + rotatedPosition;
        }

        protected abstract void AssignParameters(Dictionary<String, Single> parameters);
        protected abstract Vector2 GetTrajectoryOffset(Single time);
    }
}
