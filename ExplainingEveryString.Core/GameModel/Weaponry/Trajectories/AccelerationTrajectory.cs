using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class AccelerationTrajectory : BulletTrajectory
    {
        private Single speed;
        private Single acceleration;

        internal AccelerationTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(startPosition, fireDirection, parameters) { }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            this.speed = parameters[nameof(speed)];
            this.acceleration = parameters[nameof(acceleration)];
        }

        protected override Vector2 GetTrajectoryOffset(Single time) => 
            new Vector2(speed * time + acceleration * time * time / 2, 0);
    }
}
