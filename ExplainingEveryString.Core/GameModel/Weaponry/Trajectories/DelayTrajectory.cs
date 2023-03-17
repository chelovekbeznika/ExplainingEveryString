using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class DelayTrajectory : BulletTrajectory
    {
        private Single delayTime;
        private BulletTrajectory trajectoryAfterDelay;

        internal DelayTrajectory(Vector2 center, Vector2 fireDirection, Dictionary<String, Single> parameters) 
            : base(center, fireDirection, parameters)
        {
        }

        protected override void AssignParameters(Dictionary<String, Single> parameters)
        {
            delayTime = parameters["delay"];
            var typeKey = parameters.Keys.First(k => k.StartsWith("type_"));
            var trajectoryType = typeKey["type_".Length..];
            trajectoryAfterDelay = TrajectoryFactory.GetTrajectory(trajectoryType, Vector2.Zero, new Vector2(1, 0), parameters);
        }

        protected override Vector2 GetTrajectoryOffset(Single time)
        {
            if (time < delayTime)
                return trajectoryAfterDelay.GetBulletPosition(0);
            else
                return trajectoryAfterDelay.GetBulletPosition(time - delayTime);
        }
    }
}
