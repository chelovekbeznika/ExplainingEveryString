using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.RandomVariables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class RandomizedLinearTrajectory : BulletTrajectory
    {
        private Vector2 startPointOffset;
        private Vector2 speed;

        internal RandomizedLinearTrajectory(Vector2 center, Vector2 fireDirection, Dictionary<String, Single> parameters)
            : base(center, fireDirection, parameters)
        {
        }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            startPointOffset = new Vector2
            {
                X = RandomUtility.NextGauss(new GaussRandomVariable 
                { 
                    ExpectedValue = 0, 
                    Sigma = parameters["sigmaXStart"] 
                }, true),
                Y = RandomUtility.NextGauss(new GaussRandomVariable
                {
                    ExpectedValue = 0,
                    Sigma = parameters["sigmaYStart"]
                }, true)
            };
            var linearSpeed = RandomUtility.NextGauss(new GaussRandomVariable
            {
                ExpectedValue = parameters["expectedSpeed"],
                Sigma = parameters["sigmaSpeed"]
            });
            var angle = RandomUtility.NextGauss(new GaussRandomVariable
            {
                ExpectedValue = 0,
                Sigma = AngleConverter.ToRadians(parameters["sigmaAngle"])
            });
            speed = GeometryHelper.RotateVector(new Vector2(linearSpeed, 0), angle);
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            return startPointOffset + speed * time;
        }
    }
}
