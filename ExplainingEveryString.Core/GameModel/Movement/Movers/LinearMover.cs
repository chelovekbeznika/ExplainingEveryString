using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class LinearMover : IMover
    {
        private Single scalarSpeed;

        public LinearMover(Single scalarSpeed)
        {
            this.scalarSpeed = scalarSpeed;
        }

        public Boolean IsTeleporting => false;

        public Vector2 GetPositionChange(Vector2 lineToTarget, ref Single timeRemained)
        {
            if (lineToTarget.Length() >= Math.Constants.Epsilon)
            {
                var eta = lineToTarget.Length() / scalarSpeed;
                var speed = lineToTarget / lineToTarget.Length() * scalarSpeed;
                if (eta > timeRemained)
                {
                    var positionChange = speed * timeRemained;
                    timeRemained = 0;
                    return positionChange;
                }
                else
                {
                    timeRemained -= eta;
                    return lineToTarget;
                }
            }
            else
            {
                return Vector2.Zero;
            }
        }
    }
}
