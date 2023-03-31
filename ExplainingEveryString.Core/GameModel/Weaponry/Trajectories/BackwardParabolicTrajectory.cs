using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class BackwardParabolicTrajectory : BulletTrajectory
    {
        private Single speed;
        private Single innerCoeff = 1;
        private Single outerCoeff = 1;

        internal BackwardParabolicTrajectory(Vector2 center, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(center, fireDirection, parameters)
        {
        }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            this.speed = parameters["speed"];
            if (parameters.ContainsKey("outerCoefficient"))
                this.outerCoeff = parameters["outerCoefficient"];
            if (parameters.ContainsKey("innerCoefficient"))
                this.innerCoeff = parameters["innerCoefficient"];
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            var y = speed * time;
            var x = y * y / (outerCoeff * outerCoeff * innerCoeff);
            return new Vector2((Single)x, (Single)y);
        }
    }
}
