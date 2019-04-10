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

        //In our game we are sliding along a walls
        internal void TryToBypassWall(Hitbox oldHitbox, Hitbox newHitbox, Hitbox wall, 
            out Vector2? positionAfterWallHit, Boolean horMovePriority, out Boolean movingIntoCorner)
        {
            if (MovingFromWall(oldHitbox, newHitbox, wall))
            {
                positionAfterWallHit = null;
                movingIntoCorner = false;
                return;
            }

            if (horMovePriority)
            {
                CheckHorizontalMove(oldHitbox, newHitbox, wall, out positionAfterWallHit, out movingIntoCorner);
                if (positionAfterWallHit == null)
                    CheckVerticalMove(oldHitbox, newHitbox, wall, out positionAfterWallHit, out movingIntoCorner);
            }
            else
            {
                CheckVerticalMove(oldHitbox, newHitbox, wall, out positionAfterWallHit, out movingIntoCorner);
                if (positionAfterWallHit == null)
                    CheckHorizontalMove(oldHitbox, newHitbox, wall, out positionAfterWallHit, out movingIntoCorner);
            }
        }



        private Boolean MovingFromWall(Hitbox oldHitbox, Hitbox newHitbox, Hitbox wall)
        {
            if ((oldHitbox.Right <= wall.Left) && (newHitbox.Right <= oldHitbox.Right))
                return true;
            if ((oldHitbox.Left >= wall.Right) && (newHitbox.Left >= oldHitbox.Left))
                return true;
            if ((oldHitbox.Top <= wall.Bottom) && (newHitbox.Bottom <= oldHitbox.Bottom))
                return true;
            if ((oldHitbox.Bottom >= wall.Top) && (newHitbox.Top >= oldHitbox.Top))
                return true;
            return false;
        }

        private void CheckHorizontalMove(Hitbox oldHitbox, Hitbox newHitbox, Hitbox wall,
            out Vector2? positionAfterWallHit, out Boolean touchingCorner)
        {
            Boolean touchingHorizontalFringes = (Math.Abs(oldHitbox.Left - wall.Right) < MathConstants.Epsilon
                    || Math.Abs(oldHitbox.Right - wall.Left) < MathConstants.Epsilon);
            if (CrossWallBottom(oldHitbox, newHitbox, wall))
            {
                positionAfterWallHit = new Vector2
                {
                    X = (newHitbox.Left + newHitbox.Right) / 2,
                    Y = wall.Bottom - (newHitbox.Top - newHitbox.Bottom) / 2
                };
                touchingCorner = Math.Abs(oldHitbox.Top - wall.Bottom) < MathConstants.Epsilon
                    && touchingHorizontalFringes;
;
                return;
            }
            if (CrossWallTop(oldHitbox, newHitbox, wall))
            {
                positionAfterWallHit = new Vector2
                {
                    X = (newHitbox.Left + newHitbox.Right) / 2,
                    Y = wall.Top + (newHitbox.Top - newHitbox.Bottom) / 2
                };
                touchingCorner = Math.Abs(oldHitbox.Bottom - wall.Top) < MathConstants.Epsilon
                    && touchingHorizontalFringes;
                return;
            }
            positionAfterWallHit = null;
            touchingCorner = false;
        }

        private void CheckVerticalMove(Hitbox oldHitbox, Hitbox newHitbox, Hitbox wall,
            out Vector2? positionAfterWallHit, out Boolean touchingCorner)
        {
            Boolean touchingVerticalFringes = (Math.Abs(oldHitbox.Top - wall.Bottom) < MathConstants.Epsilon
                    || Math.Abs(oldHitbox.Bottom - wall.Top) < MathConstants.Epsilon);
            if (CrossWallLeft(oldHitbox, newHitbox, wall))
            {
                positionAfterWallHit = new Vector2
                {
                    X = wall.Left - (newHitbox.Right - newHitbox.Left) / 2,
                    Y = (newHitbox.Top + newHitbox.Bottom) / 2
                };
                touchingCorner = Math.Abs(oldHitbox.Right - wall.Left) < MathConstants.Epsilon
                    && touchingVerticalFringes;
                return;
            }
            if (CrossWallRight(oldHitbox, newHitbox, wall))
            {
                positionAfterWallHit = new Vector2
                {
                    X = wall.Right + (newHitbox.Right - newHitbox.Left) / 2,
                    Y = (newHitbox.Top + newHitbox.Bottom) / 2
                };
                touchingCorner = Math.Abs(oldHitbox.Left - wall.Right) < MathConstants.Epsilon
                    && touchingVerticalFringes;
                return;
            }
            positionAfterWallHit = null;
            touchingCorner = false;
        }

        private Boolean CrossWallBottom(Hitbox oldHitbox, Hitbox newHitbox, Hitbox wall)
        {
            Boolean crossingWallBottomLine = (newHitbox.Top > oldHitbox.Top)
                && (newHitbox.Top > wall.Bottom) && (oldHitbox.Top <= wall.Bottom);
            if (crossingWallBottomLine)
            {
                Single partOfPathBeforeCrossing = (wall.Bottom - oldHitbox.Top) / (newHitbox.Top - oldHitbox.Top);
                Single horizontalDelta = (newHitbox.Right - oldHitbox.Right) * partOfPathBeforeCrossing;
                return Overlaps(oldHitbox.Left + horizontalDelta, oldHitbox.Right + horizontalDelta, 
                    wall.Left, wall.Right);
            }
            else
                return false;
        }

        private Boolean CrossWallTop(Hitbox oldHitbox, Hitbox newHitbox, Hitbox wall)
        {
            Boolean crossingWallTopLine = (newHitbox.Bottom < oldHitbox.Bottom) 
                && (newHitbox.Bottom < wall.Top) && (oldHitbox.Bottom >= wall.Top);
            if (crossingWallTopLine)
            {
                Single partOfPathBeforeCrossing = (wall.Top - oldHitbox.Bottom) / (newHitbox.Bottom - oldHitbox.Bottom);
                Single horizontalDelta = (newHitbox.Right - oldHitbox.Right) * partOfPathBeforeCrossing;
                return Overlaps(oldHitbox.Left + horizontalDelta, oldHitbox.Right + horizontalDelta,
                    wall.Left, wall.Right);
            }
            else
                return false;
        }

        private Boolean CrossWallLeft(Hitbox oldHitbox, Hitbox newHitbox, Hitbox wall)
        {
            Boolean crossingWallLeftLine = (newHitbox.Right > oldHitbox.Right) 
                && (newHitbox.Right > wall.Left) && (oldHitbox.Right <= wall.Left);
            if (crossingWallLeftLine)
            {
                Single partOfPathBeforeCrossing = (wall.Left - oldHitbox.Right) / (newHitbox.Right - oldHitbox.Right);
                Single verticalDelta = (newHitbox.Top - oldHitbox.Top) * partOfPathBeforeCrossing;
                return Overlaps(oldHitbox.Bottom + verticalDelta, oldHitbox.Top + verticalDelta,
                    wall.Bottom, wall.Top);
            }
            else
                return false;
        }

        private Boolean CrossWallRight(Hitbox oldHitbox, Hitbox newHitbox, Hitbox wall)
        {
            Boolean crossingWallRightLine = (newHitbox.Left < oldHitbox.Left) 
                && (newHitbox.Left < wall.Right) && (oldHitbox.Left >= wall.Right);
            if (crossingWallRightLine)
            {
                Single partOfPathBeforeCrossing = (wall.Right - oldHitbox.Left) / (newHitbox.Left - oldHitbox.Left);
                Single verticalDelta = (newHitbox.Top - oldHitbox.Top) * partOfPathBeforeCrossing;
                return Overlaps(oldHitbox.Bottom + verticalDelta, oldHitbox.Top + verticalDelta,
                    wall.Bottom, wall.Top);
            }
            else
                return false;
        }

        private Boolean Overlaps(Single firstBegin, Single firstEnd, Single secondBegin, Single secondEnd)
        {
            if (firstBegin > firstEnd)
            {
                Single temporary = firstBegin;
                firstBegin = firstEnd;
                firstEnd = temporary;
            }
            if (secondBegin > secondEnd)
            {
                Single temporary = secondBegin;
                secondBegin = secondEnd;
                secondEnd = temporary;
            }

            return (firstBegin <= secondEnd && firstBegin >= secondBegin)
                || (firstEnd <= secondEnd && firstEnd >= secondBegin)
                || (secondBegin <= firstEnd && secondBegin >= firstBegin)
                || (secondEnd <= firstEnd && secondEnd >= firstBegin);
        }
    }
}
