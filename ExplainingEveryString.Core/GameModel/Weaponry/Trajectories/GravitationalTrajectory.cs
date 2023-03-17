using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class GravitationalTrajectory : BulletTrajectory
    {
        private Vector2 startSpeed;
        private Single fallAcceleration;

        internal GravitationalTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters)
            : base(startPosition, fireDirection, parameters) { }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            startSpeed = new Vector2 { X = parameters["startFlySpeed"], Y = parameters["sideSpeed"] };
            fallAcceleration = parameters["fallAcceleration"];
        }

        protected override Vector2 GetTrajectoryOffset(Single time) => new Vector2
        {
            X = (startSpeed.X - fallAcceleration * time) * time,
            Y = startSpeed.Y * time
        };
    }
}
