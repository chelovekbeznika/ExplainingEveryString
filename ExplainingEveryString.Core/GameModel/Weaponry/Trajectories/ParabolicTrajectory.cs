using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class ParabolicTrajectory : BulletTrajectory
    {
        private Single speed;
        private Single outerCoefficient = 1;
        private Single innerCoefficient = 1;

        internal ParabolicTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters)
            : base(startPosition, fireDirection, parameters) { }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            this.speed = parameters["speed"];
            if (parameters.ContainsKey("outerCoefficient")) this.outerCoefficient = parameters["outerCoefficient"];
            if (parameters.ContainsKey("innerCoefficient")) this.innerCoefficient = parameters["innerCoefficient"];
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            return new Vector2
            {
                X = time * speed,
                Y = outerCoefficient * (Single)System.Math.Sqrt(time * speed * innerCoefficient)
            };
        }
    }
}
