using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
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

        internal Boolean Collides(Hitbox hitbox, Vector2 point)
        {
            return point.X >= hitbox.Left && point.X <= hitbox.Right
                && point.Y <= hitbox.Top && point.Y >= hitbox.Bottom;
        }
    }
}
