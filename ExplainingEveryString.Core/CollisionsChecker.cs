using ExplainingEveryString.Core.GameModel;
using System;

namespace ExplainingEveryString.Core
{
    internal class CollisionsChecker
    {
        internal Boolean Collides(Hitbox first, Hitbox second)
        {
            Boolean secondLiesCompletelyAtLeft = second.Right < first.Left;
            Boolean secondLiesCompletelyAtRight = second.Left > first.Right;
            Boolean intersectsOnXAxis = !(secondLiesCompletelyAtLeft || secondLiesCompletelyAtRight);
            Boolean secondLiesCompletelyAtBottom = second.Top < first.Bottom;
            Boolean secondLiesCompletelyAtTop = second.Bottom > first.Top;
            Boolean intersectsOnYAxis = !(secondLiesCompletelyAtBottom || secondLiesCompletelyAtTop);
            return intersectsOnXAxis && intersectsOnYAxis;
        }
    }
}
