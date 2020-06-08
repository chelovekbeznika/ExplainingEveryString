using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class AccelerationMover : IMover
    {
        private const Single SecondsTillTargetSwitchAllowed = 0.3F;
        private Single acceleration;
        private Single startSpeed;
        private Single maxSpeed;
        private Single approachedMinimum = Single.MaxValue;
        private Single fromLastTargetSwitch = 0;

        private Vector2 currentSpeed = new Vector2(0, 0);
        private Boolean moving = false;

        internal AccelerationMover(Single acceleration, Single startSpeed, Single maxSpeed)
        {
            this.acceleration = acceleration;
            this.startSpeed = startSpeed;
            this.maxSpeed = maxSpeed;
        }

        public Boolean IsTeleporting => false;

        public Vector2 GetPositionChange(Vector2 lineToTarget, ref Single timeRemained)
        {
            var currentDistance = lineToTarget.Length();
            if (currentDistance <= approachedMinimum)
            {
                var unitVectorTowardTarget = lineToTarget / currentDistance;
                if (!moving)
                    StartMove(unitVectorTowardTarget);

                var positionChange = CalculatePositionChange(unitVectorTowardTarget, timeRemained);

                fromLastTargetSwitch += timeRemained;
                timeRemained = currentDistance <= positionChange.Length() ? timeRemained : 0;
                if (timeRemained > 0)
                    ResetCounters();
                
                if (currentDistance <= approachedMinimum && fromLastTargetSwitch > SecondsTillTargetSwitchAllowed)
                    approachedMinimum = currentDistance;

                return positionChange;
            }
            else
            {
                var positionChange = currentSpeed * timeRemained;
                timeRemained = 0;
                ResetCounters();
                return positionChange;
            }
        }

        private void StartMove(Vector2 unitVectorTowardTarget)
        {
            moving = true;
            currentSpeed = unitVectorTowardTarget * startSpeed;
        }

        private Vector2 CalculatePositionChange(Vector2 unitVectorTowardTarget, Single elapsedSeconds)
        {
            var oneSecondSpeedChange = unitVectorTowardTarget * acceleration;
            var speedChange = oneSecondSpeedChange * elapsedSeconds;
            currentSpeed += speedChange;
            if (currentSpeed.Length() > maxSpeed)
                currentSpeed = currentSpeed / currentSpeed.Length() * maxSpeed;
            return currentSpeed * elapsedSeconds;
        }

        private void ResetCounters()
        {
            approachedMinimum = Single.MaxValue;
            fromLastTargetSwitch = 0;
        }
    }
}
