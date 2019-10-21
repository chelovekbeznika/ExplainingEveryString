using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class RandomFromListTargetsSelector : IMoveTargetSelector
    {
        private List<Vector2> targets;
        private Vector2 startPosition;
        private Int32 currentTargetNumber;

        internal RandomFromListTargetsSelector(List<Vector2> targetsList, Vector2 startPosition)
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
            currentTargetNumber = RandomUtility.NextInt(targets.Count);
        }
    }
}
