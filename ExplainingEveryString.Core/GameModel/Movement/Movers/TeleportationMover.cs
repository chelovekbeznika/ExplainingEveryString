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

        public Vector2 GetPositionChange(Vector2 lineToTarget, Single elapsedSeconds, out Boolean goalReached)
        {
            if (DoTeleportNow(elapsedSeconds))
            {
                goalReached = true;
                return lineToTarget;
            }
            else
            {
                goalReached = false;
                return Vector2.Zero;
            }
        }

        private Boolean DoTeleportNow(Single elapsedSeconds)
        {
            tillNextTeleport -= elapsedSeconds;
            if (tillNextTeleport < 0)
            {
                tillNextTeleport += RandomUtility.Next(minTillTeleport, maxTillTeleport);
                return true;
            }
            else
                return false;
        }
    }
}
