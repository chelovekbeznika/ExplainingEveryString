using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class ClosingSpiralTrajectory : BulletTrajectory
    {
        private Single startRadius;
        private Single lockingSpeed;
        private Single angularSpeed;

        internal ClosingSpiralTrajectory(Vector2 center, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(center, fireDirection, parameters)
        {
        }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            startRadius = parameters["startRadius"];
            lockingSpeed = parameters["lockingSpeed"];
            angularSpeed = AngleConverter.ToRadians(parameters["angularSpeed"]);
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            var radius = startRadius - lockingSpeed * time;
            if (radius < 0)
                radius = 0;
            var angle = angularSpeed * time;
            return GeometryHelper.RotateVector(new Vector2(radius, 0), angle);
        }
    }
}
