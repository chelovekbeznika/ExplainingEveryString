using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossMovementController
    {
        private FourthBossMovementSpecification specification;
        private Single currentAngle;
        private Single angularVelocity;
        private Boolean clockWise = true;
        private Vector2 center;
        private Single tillNextDirectionSwitch;
        private Dictionary<String, Single> currentPulsePhase;

        internal Vector2 Position { get; private set; }
        internal Single Angle => (Single)(currentAngle - System.Math.PI);

        internal Single PulsationCoefficient(String tag) => (Single)((System.Math.Sin(currentPulsePhase[tag]) + 1) / 2);

        internal FourthBossMovementController(FourthBossMovementSpecification specification, Vector2 center)
        {
            this.specification = specification;
            this.center = center;
            this.currentAngle = 0;
            this.angularVelocity = (Single)(2 * System.Math.PI / specification.CircleTime);
            var trajectoryHand = new Vector2(specification.Radius, 0);
            this.Position = center + trajectoryHand;
            this.tillNextDirectionSwitch = RandomUtility.NextGauss(specification.BetweenDirectionSwitches);
            this.currentPulsePhase = specification.TimeBetweenPulses.ToDictionary(t => t.Key, t => 0f);
        }

        internal void Update(Single elapsedSeconds)
        {
            foreach (var tag in currentPulsePhase.Keys.ToList())
            {
                currentPulsePhase[tag] += (Single)(2 * System.Math.PI / specification.TimeBetweenPulses[tag] * elapsedSeconds);
            }

            tillNextDirectionSwitch -= elapsedSeconds;
            if (tillNextDirectionSwitch < 0)
            {
                clockWise =! clockWise;
                tillNextDirectionSwitch = RandomUtility.NextGauss(specification.BetweenDirectionSwitches);
            }

            var directionCoeff = clockWise ? 1 : -1;
            currentAngle += directionCoeff * angularVelocity * elapsedSeconds;
            var currentRadius = specification.Radius + specification.RadiusPulsationPart * PulsationCoefficient("Wings");
            var trajectoryHand = new Vector2(currentRadius, 0);
            Position = center + GeometryHelper.RotateVector(trajectoryHand, currentAngle);
        }
    }
}
