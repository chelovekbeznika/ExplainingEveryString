using System;
using System.Collections.Generic;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class RandomFromListTargetsSelector : IMoveTargetSelector
    {
        private List<Vector2> targets;
        private Vector2 startPosition;
        private Int32 currentTargetNumber;

        internal RandomFromListTargetsSelector(Vector2[] targetsList, Vector2 startPosition)
        {
            this.targets = new List<Vector2>(targetsList);
            this.startPosition = startPosition;
            SwitchToNextTarget();
        }

        public Vector2 GetTarget()
        {
            return targets[currentTargetNumber] + startPosition;
        }

        public void SwitchToNextTarget()
        {
            Int32 previousTarget = currentTargetNumber;
            currentTargetNumber = RandomUtility.NextInt(targets.Count - 1);
            if (currentTargetNumber >= previousTarget)
                currentTargetNumber += 1;
        }
    }
}
