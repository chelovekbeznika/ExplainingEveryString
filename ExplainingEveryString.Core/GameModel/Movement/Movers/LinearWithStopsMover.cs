using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class LinearWithStopsMover : IMover
    {
        private Single scalarSpeed;
        private Single stopTime;
        private Single tillStopEnd;
        private Boolean stopped = true;

        public LinearWithStopsMover(Single scalarSpeed, Single stopTime)
        {
            this.scalarSpeed = scalarSpeed;
            this.stopTime = stopTime;
            this.tillStopEnd = stopTime;
        }

        public Boolean IsTeleporting => false;

        public Vector2 GetPositionChange(Vector2 lineToTarget, ref Single timeRemained)
        {
            var resultVector = Vector2.Zero;
            if (lineToTarget.Length() >= Math.Constants.Epsilon && !stopped)
            {
                var eta = lineToTarget.Length() / scalarSpeed;
                var speed = lineToTarget / lineToTarget.Length() * scalarSpeed;
                if (eta > timeRemained)
                {
                    var positionChange = speed * timeRemained;
                    timeRemained = 0;
                    resultVector = positionChange;
                }
                else
                {
                    timeRemained -= eta;
                    resultVector = lineToTarget;
                    stopped = true;
                }
            }
            if (stopped)
            {
                if (tillStopEnd > timeRemained)
                {
                    tillStopEnd -= timeRemained;
                    timeRemained = 0;
                }
                else
                {
                    timeRemained -= tillStopEnd;
                    stopped = false;
                    tillStopEnd = stopTime;
                }
            }
            return resultVector;
        }
    }
}
