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

        internal Boolean Collides(Hitbox hitbox, Vector2 oldPosition, Vector2 newPosition)
        {
            if (Collides(hitbox, newPosition))
                return true;

            return TrajectoryCrossesWithVerticalFringe(hitbox, true, oldPosition, newPosition)
                || TrajectoryCrossesWithVerticalFringe(hitbox, false, oldPosition, newPosition)
                || TrajectoryCrossesWithHorizontalFringe(hitbox, true, oldPosition, newPosition)
                || TrajectoryCrossesWithHorizontalFringe(hitbox, false, oldPosition, newPosition);
        }

        private Boolean TrajectoryCrossesWithVerticalFringe
            (Hitbox hitbox, Boolean left, Vector2 oldPosition, Vector2 newPosition)
        {
            Single fringeXPosition = left ? hitbox.Left : hitbox.Right;
            if ((oldPosition.X > fringeXPosition && newPosition.X > fringeXPosition)
                || (oldPosition.X < fringeXPosition && newPosition.X < fringeXPosition))
                return false;

            Single? fringeCrossing = GetTrajectoryCrossingWithVerticalFringe
                (fringeXPosition, newPosition, oldPosition);

            if (fringeCrossing != null)
                return fringeCrossing >= hitbox.Bottom && fringeCrossing <= hitbox.Top;
            else
                return newPosition.X == fringeXPosition
                    && Overlaps(oldPosition.Y, newPosition.Y, hitbox.Bottom, hitbox.Top);
        }

        private Boolean TrajectoryCrossesWithHorizontalFringe
            (Hitbox hitbox, Boolean bottom, Vector2 oldPosition, Vector2 newPosition)
        {
            Single fringeYPosition = bottom ? hitbox.Bottom : hitbox.Top;
            if ((oldPosition.Y > fringeYPosition && newPosition.Y > fringeYPosition)
                || (oldPosition.Y < fringeYPosition && newPosition.Y < fringeYPosition))
                return false;

            Single? fringeCrossing = GetTrajectoryCrossingWithHorizontalFringe
                (fringeYPosition, newPosition, oldPosition);

            if (fringeCrossing != null)
                return fringeCrossing >= hitbox.Left && fringeCrossing <= hitbox.Right;
            else
                return newPosition.Y == fringeYPosition
                    && Overlaps(oldPosition.X, newPosition.X, hitbox.Left, hitbox.Right);
        }

        private Single? GetTrajectoryCrossingWithVerticalFringe
            (Single fringeXPosition, Vector2 newPosition, Vector2 oldPosition)
        {
            if (newPosition.X == oldPosition.X)
            {
                return null;
            }

            Single deltaX = newPosition.X - oldPosition.X;
            Single deltaY = newPosition.Y - oldPosition.Y;
            Single deltaXTillFringe = fringeXPosition - oldPosition.X;
            Single deltaYTillFringe = deltaY * deltaXTillFringe / deltaX;
            return oldPosition.Y + deltaYTillFringe;
        }

        private Single? GetTrajectoryCrossingWithHorizontalFringe
            (Single fringeYPosition, Vector2 newPosition, Vector2 oldPosition)
        {
            if (newPosition.Y == oldPosition.Y)
            {
                return null;
            }

            Single deltaX = newPosition.X - oldPosition.X;
            Single deltaY = newPosition.Y - oldPosition.Y;
            Single deltaYTillFringe = fringeYPosition - oldPosition.Y;
            Single deltaXTillFringe = deltaX * deltaYTillFringe / deltaY;
            return oldPosition.X + deltaXTillFringe;
        }

        private Boolean Collides(Hitbox hitbox, Vector2 point)
        {
            return point.X >= hitbox.Left && point.X <= hitbox.Right
                && point.Y <= hitbox.Top && point.Y >= hitbox.Bottom;
        }

        private Boolean Overlaps(Single firstBegin, Single firstEnd, Single secondBegin, Single secondEnd)
        {
            if (firstBegin > firstEnd)
            {
                Single temporary = firstBegin;
                firstBegin = firstEnd;
                firstEnd = temporary;
            }

            return !(firstEnd < secondBegin && secondEnd < firstBegin);
        }
    }
}
