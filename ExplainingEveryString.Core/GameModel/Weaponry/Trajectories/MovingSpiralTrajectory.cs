using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class MovingSpiralTrajectory : BulletTrajectory
    {
        private Single linearSpeed;
        private Single expansionSpeed;
        private Single angularSpeed;
        private Single startingAngle;

        internal MovingSpiralTrajectory(Vector2 center, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(center, fireDirection, parameters) { }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            linearSpeed = parameters["linearSpeed"];
            expansionSpeed = parameters["expansionSpeed"];
            angularSpeed = AngleConverter.ToRadians(parameters["angularSpeed"]);
            startingAngle = AngleConverter.ToRadians(parameters["startingAngle"]);
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            var currentRotation = GeometryHelper.RotateVector(Vector2.UnitX, startingAngle + angularSpeed * time);
            return new Vector2(linearSpeed * time, 0) + currentRotation * expansionSpeed * time;
        }
    }
}
