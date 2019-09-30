using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class LoopingTargetSelector : IMoveTargetSelector
    {
        private Queue<Vector2> targets;
        private Vector2 startPosition;

        internal LoopingTargetSelector(List<Vector2> targetsList, Vector2 startPosition)
        {
            this.targets = new Queue<Vector2>(targetsList);
            this.startPosition = startPosition;
        }

        public Vector2 GetTarget()
        {
            return targets.Peek() + startPosition;
        }

        public void SwitchToNextTarget()
        {
            Vector2 formerTarget = targets.Dequeue();
            targets.Enqueue(formerTarget);
        }

        public void UnpackTargets(List<Vector2> trajectoryParams, Vector2 startPosition)
        {
            this.targets = new Queue<Vector2>(trajectoryParams);
            this.startPosition = startPosition;
        }
    }
}
