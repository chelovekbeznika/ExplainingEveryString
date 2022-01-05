using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class SpiralTrajectory : BulletTrajectory
    {
        private Single radialSpeed;
        private Single angularVelocity;

        internal SpiralTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters)
            : base(startPosition, fireDirection, parameters) { }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            this.radialSpeed = parameters["speed"];
            this.angularVelocity = parameters.ContainsKey("angularVelocity") ? AngleConverter.ToRadians(parameters["angularVelocity"]) : (Single)System.Math.PI / 2;
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            var unbentSpiral = new Vector2(radialSpeed, 0) * time;
            var angle = angularVelocity * time;
            return GeometryHelper.RotateVector(unbentSpiral, angle);
        }
    }
}
