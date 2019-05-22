using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class NotATargetSelector : IMoveTargetSelector
    {
        private Func<Vector2> currentPositionLocator;

        internal NotATargetSelector(Func<Vector2> currentPositionLocator)
        {
            this.currentPositionLocator = currentPositionLocator;
        }

        public Vector2 GetTarget()
        {
            return currentPositionLocator();
        }

        public void SwitchToNextTarget()
        {
        }
    }
}
