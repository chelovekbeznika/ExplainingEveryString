using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class LinearMover : IMover
    {
        private Single scalarSpeed;

        public LinearMover(Single scalarSpeed)
        {
            this.scalarSpeed = scalarSpeed;
        }

        public Vector2 GetPositionChange(Vector2 lineToTarget, Single elapsedSeconds, out Boolean goalReached)
        {
            if (lineToTarget.Length() >= Math.Constants.Epsilon)
            {
                Vector2 speed = lineToTarget / lineToTarget.Length() * scalarSpeed;
                Vector2 positionChange = speed * elapsedSeconds;
                goalReached = lineToTarget.Length() <= positionChange.Length();
                if (goalReached)
                    return lineToTarget;
                else
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
