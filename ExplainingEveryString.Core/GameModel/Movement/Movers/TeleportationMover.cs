using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class TeleportationMover : IMover
    {
        private Single minTillTeleport;
        private Single maxTillTeleport;
        private Single tillNextTeleport;

        internal TeleportationMover(Single minTillTeleport, Single maxTillTeleport)
        {
            this.minTillTeleport = minTillTeleport;
            this.maxTillTeleport = maxTillTeleport;
            tillNextTeleport = RandomUtility.Next(minTillTeleport, maxTillTeleport);
        }

        public Boolean IsTeleporting => true;

        public Vector2 GetPositionChange(Vector2 lineToTarget, ref Single timeRemained)
        {
            if (tillNextTeleport > timeRemained)
            {
                tillNextTeleport -= timeRemained;
                timeRemained = 0;
                return Vector2.Zero;
            }
            else
            {
                timeRemained -= tillNextTeleport;
                tillNextTeleport = RandomUtility.Next(minTillTeleport, maxTillTeleport);
                return lineToTarget;
            }
        }
    }
}
