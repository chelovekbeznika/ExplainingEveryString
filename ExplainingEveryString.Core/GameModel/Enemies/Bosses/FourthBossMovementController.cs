using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossMovementController
    {
        private FourthBossMovementSpecification specification;
        private Single currentAngle;
        private Single angularVelocity;
        private Boolean clockWise = true;
        private Vector2 center;
        private Vector2 trajectoryHand;
        private Single tillNextDirectionSwitch;

        internal Vector2 Position { get; private set; }

        internal FourthBossMovementController(FourthBossMovementSpecification specification, Vector2 center)
        {
            this.specification = specification;
            this.center = center;
            this.currentAngle = 0;
            this.angularVelocity = (Single)(2 * System.Math.PI / specification.CircleTime);
            this.trajectoryHand = new Vector2(specification.Radius, 0);
            this.Position = center + trajectoryHand;
            this.tillNextDirectionSwitch = RandomUtility.NextGauss(specification.BetweenDirectionSwitches);
        }

        internal void Update(Single elapsedSeconds)
        {
            tillNextDirectionSwitch -= elapsedSeconds;
            if (tillNextDirectionSwitch < 0)
            {
                clockWise =! clockWise;
                tillNextDirectionSwitch = RandomUtility.NextGauss(specification.BetweenDirectionSwitches);
            }

            var directionCoeff = clockWise ? 1 : -1;
            currentAngle += directionCoeff * angularVelocity * elapsedSeconds;
            Position = center + GeometryHelper.RotateVector(trajectoryHand, currentAngle);
        }
    }
}
