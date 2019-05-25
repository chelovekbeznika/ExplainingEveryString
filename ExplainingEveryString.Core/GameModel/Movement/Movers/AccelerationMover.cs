using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class AccelerationMover : IMover
    {
        private Single acceleration;
        private Single startSpeed;
        private Single maxSpeed;

        private Vector2 currentSpeed = new Vector2(0, 0);
        private Boolean moving = false;

        internal AccelerationMover(Single acceleration, Single startSpeed, Single maxSpeed)
        {
            this.acceleration = acceleration;
            this.startSpeed = startSpeed;
            this.maxSpeed = maxSpeed;
        }

        public Vector2 GetPositionChange(Vector2 lineToTarget, Single elapsedSeconds, out Boolean goalReached)
        {
            if (lineToTarget.Length() >= Math.Constants.Epsilon)
            {
                if (!moving)
                {
                    moving = true;
                    currentSpeed = lineToTarget / lineToTarget.Length() * startSpeed;
                }
                Vector2 oneSecondSpeedChange = lineToTarget / lineToTarget.Length() * acceleration;
                Vector2 speedChange = oneSecondSpeedChange * elapsedSeconds;
                currentSpeed += speedChange;
                if (currentSpeed.Length() > maxSpeed)
                    currentSpeed = currentSpeed / currentSpeed.Length() * maxSpeed;
                Vector2 positionChange = currentSpeed * elapsedSeconds;
                goalReached = lineToTarget.Length() <= positionChange.Length();
                return positionChange;
            }
            else
            {
                goalReached = true;
                return new Vector2(0, 0);
            }
        }
    }
}
