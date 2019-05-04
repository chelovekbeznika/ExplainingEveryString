using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class LinearTrajectory : BulletTrajectory
    {
        private Single speed;

        internal LinearTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(startPosition, fireDirection, parameters) { }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            this.speed = parameters[nameof(speed)];
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            return new Vector2(speed * time, 0);
        }
    }
}
