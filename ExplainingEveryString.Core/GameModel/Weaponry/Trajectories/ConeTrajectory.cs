using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class ConeTrajectory : BulletTrajectory
    {
        private Single speed;
        private Single angularVelocity;
        private Single coneExtension;
        private Single phaseShift;

        internal ConeTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(startPosition, fireDirection, parameters)
        {
        }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            this.speed = parameters[nameof(speed)];
            this.angularVelocity = parameters[nameof(angularVelocity)];
            this.coneExtension = parameters[nameof(coneExtension)];
            this.phaseShift = parameters[nameof(phaseShift)];
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            return new Vector2(time * speed, (Single)System.Math.Sin(phaseShift + time * angularVelocity) * time * coneExtension);
        }
    }
}
