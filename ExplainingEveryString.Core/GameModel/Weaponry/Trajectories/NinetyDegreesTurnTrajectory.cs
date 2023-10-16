using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.RandomVariables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class NinetyDegreesTurnTrajectory : BulletTrajectory
    {
        private Single turningPoint;
        private Boolean turningRight;
        private Single speed;

        internal NinetyDegreesTurnTrajectory(Vector2 center, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(center, fireDirection, parameters)
        {
        }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            var turningPoint = new GaussRandomVariable
            {
                ExpectedValue = parameters["expectedTurn"],
                Sigma = parameters["expectedSigma"]
            };
            this.turningPoint = RandomUtility.NextGauss(turningPoint);
            turningRight = RandomUtility.Next() < 0.5f;
            speed = parameters["speed"];
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            var flown = speed * time;
            if (flown < turningPoint)
                return new Vector2(flown, 0);
            else
                return new Vector2(turningPoint, turningRight ? - (flown - turningPoint) : flown - turningPoint);
        }
    }
}
