using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class SinusTrajectory : BulletTrajectory
    {
        private Single speed;
        private Single amplitude;
        private Single angularVelocity;

        internal SinusTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters)
            : base(startPosition, fireDirection, parameters) { }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            this.speed = parameters[nameof(speed)];
            this.amplitude = parameters[nameof(amplitude)];
            this.angularVelocity = parameters[nameof(angularVelocity)];
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
            => new Vector2(time * speed, (Single)System.Math.Sin(time * angularVelocity) * amplitude);
    }
}
