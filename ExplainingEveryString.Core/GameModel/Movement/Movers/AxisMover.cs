using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class AxisMover : IMover
    {
        private enum Axe { Hor, Ver }

        private Single scalarSpeed;
        private Single minAxisChangeTime;
        private Single maxAxisChangeTime;

        private Axe CurrentAxe;
        private Single tillAxeSwitch;

        internal AxisMover(Single scalarSpeed, Single minAxisChangeTime, Single maxAxisChangeTime)
        {
            this.scalarSpeed = scalarSpeed;
            this.minAxisChangeTime = minAxisChangeTime;
            this.maxAxisChangeTime = maxAxisChangeTime;
            this.CurrentAxe = RandomUtility.Next() > 0.5 ? Axe.Hor : Axe.Ver;
            this.tillAxeSwitch = RandomUtility.Next(minAxisChangeTime, maxAxisChangeTime);
        }

        public Boolean IsTeleporting => false;

        public Vector2 GetPositionChange(Vector2 lineToTarget, ref Single timeRemained)
        {
            if (Length(lineToTarget) > Math.Constants.Epsilon)
            {
                if (System.Math.Abs(lineToTarget.X) <= Math.Constants.Epsilon)
                    CurrentAxe = Axe.Ver;
                else if (System.Math.Abs(lineToTarget.Y) <= Math.Constants.Epsilon)
                    CurrentAxe = Axe.Hor;
                else
                {
                    tillAxeSwitch -= timeRemained;
                    if (tillAxeSwitch < Math.Constants.Epsilon)
                        SwitchAxe();
                }
                switch (CurrentAxe)
                {
                    case Axe.Hor:
                        var eta = System.Math.Abs(lineToTarget.X) / scalarSpeed;
                        if (eta > timeRemained)
                        {
                            var positionChangeAbs = scalarSpeed * timeRemained;
                            timeRemained = 0;
                            return new Vector2(lineToTarget.X < 0 ? -positionChangeAbs : positionChangeAbs, 0);
                        }
                        else
                        {
                            timeRemained -= eta;
                            return new Vector2(lineToTarget.X, 0) + GetPositionChange(new Vector2(0, lineToTarget.Y), ref timeRemained);
                        }
                    case Axe.Ver:
                        eta = System.Math.Abs(lineToTarget.Y) / scalarSpeed;
                        if (eta > timeRemained)
                        {
                            var positionChangeAbs = scalarSpeed * timeRemained;
                            timeRemained = 0;
                            return new Vector2(0, lineToTarget.Y < 0 ? -positionChangeAbs : positionChangeAbs);
                        }
                        else
                        {
                            timeRemained -= eta;
                            return new Vector2(0, lineToTarget.Y) + GetPositionChange(new Vector2(lineToTarget.X, 0), ref timeRemained);
                        }
                    default:
                        throw new ArgumentException(nameof(CurrentAxe));
                }
            }
            else
                return Vector2.Zero;
        }

        private Single Length(Vector2 lineToTarget) => System.Math.Abs(lineToTarget.X) + System.Math.Abs(lineToTarget.Y);

        private void SwitchAxe()
        {
            if (CurrentAxe == Axe.Hor)
                CurrentAxe = Axe.Ver;
            else
                CurrentAxe = Axe.Hor;
            tillAxeSwitch = RandomUtility.Next(minAxisChangeTime, maxAxisChangeTime);
        }
    }
}
