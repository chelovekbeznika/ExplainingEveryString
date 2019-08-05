using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class CollisionsController
    {
        private ActiveActorsStorage activeObjects;
        private CollisionsChecker collisionsChecker = new CollisionsChecker();

        internal CollisionsController(ActiveActorsStorage activeObjects)
        {
            this.activeObjects = activeObjects;
        }

        internal void CheckCollisions()
        {
            if (activeObjects.Player.IsAlive())
                CheckEnemiesForCrashingIntoPlayer();
            PreventInterpenetrationOfActors();
            CheckForBulletsCollisions();
        }

        private void CheckEnemiesForCrashingIntoPlayer()
        {
            Player player = activeObjects.Player;
            foreach (ICrashable crashable in activeObjects.Enemies.OfType<ICrashable>()
                .Where(c => c.Mode != CollidableMode.Ghost))
            {
                if (collisionsChecker.Collides(crashable.GetCurrentHitbox(), player.GetCurrentHitbox()))
                {
                    crashable.Crash();
                    player.TakeDamage(crashable.CollisionDamage);
                }
            }
        }

        private void PreventInterpenetrationOfActors()
        {
            AdjustObjectToWalls(activeObjects.Player, false, null, null);

            IEnumerable<ICollidable> enemies = activeObjects.Enemies.OfType<ICollidable>()
                .Where(c => c.Mode != CollidableMode.Ghost).ToArray();
            IEnumerable<ICollidable> movingEnemies = enemies.Where(CollidableIsMoving).ToArray();
            List<ICollidable> stoppedEnemies = new List<ICollidable>();
            foreach (ICollidable movingEnemy in movingEnemies)
            {
                Vector2 beforeMovePosition = movingEnemy.OldPosition;
                Boolean bumpedIntoOtherEnemy = false;
                foreach (ICollidable otherEnemy in enemies.Except(stoppedEnemies).Where(e => e != movingEnemy))
                {
                    if (collisionsChecker.Collides(otherEnemy.GetOldHitbox(), movingEnemy.GetCurrentHitbox())
                        || collisionsChecker.Collides(otherEnemy.GetCurrentHitbox(), movingEnemy.GetCurrentHitbox()))
                    {
                        bumpedIntoOtherEnemy = true;
                        break;
                    }
                }
                if (bumpedIntoOtherEnemy)
                {
                    movingEnemy.Position = beforeMovePosition;
                    stoppedEnemies.Add(movingEnemy);
                }
                else
                    AdjustObjectToWalls(movingEnemy, true, null, null);             
            }
        }

        private Boolean CollidableIsMoving(ICollidable collidable)
        {
            return !collidable.GetOldHitbox().Equals(collidable.GetCurrentHitbox());
        }

        private void AdjustObjectToWalls(ICollidable movingObject, Boolean ridesThroughPit,
            ICollidable tryVerticalMovePriorityForThis, Hitbox? previousOldHitbox)
        {
            Hitbox oldHitbox = previousOldHitbox == null ? movingObject.GetOldHitbox() : previousOldHitbox.Value;
            Vector2 savedMovingObjectPosition = movingObject.Position;
            ICollidable touchingToCorner = null;
            Func<ICollidable, Boolean> bumpIntoThisWall = ridesThroughPit
                ? new Func<ICollidable, Boolean>(c => c.Mode == CollidableMode.Solid)
                : new Func<ICollidable, Boolean>(c => c.Mode == CollidableMode.Solid || c.Mode == CollidableMode.Pit);
            foreach (ICollidable wall in activeObjects.GetWalls().Where(bumpIntoThisWall))
            {
                Vector2? wallCorrection;
                Boolean ridingIntoCorner;
                Boolean horizontalPriority = wall != tryVerticalMovePriorityForThis;
                collisionsChecker.TryToBypassWall(oldHitbox, movingObject.GetCurrentHitbox(),
                    wall.GetCurrentHitbox(), out wallCorrection, horizontalPriority, out ridingIntoCorner);
                
                if (wallCorrection != null)
                {
                    if (ridingIntoCorner && tryVerticalMovePriorityForThis == null)
                        touchingToCorner = wall;
                    movingObject.Position = wallCorrection.Value;                       
                }
            }

            if (touchingToCorner != null && oldHitbox.Equals(movingObject.GetCurrentHitbox()))
            {
                movingObject.Position = savedMovingObjectPosition;
                AdjustObjectToWalls(movingObject, ridesThroughPit, touchingToCorner, oldHitbox);
            }
        }

        private void CheckForBulletsCollisions()
        {
            CheckForBulletsCollisions(activeObjects.PlayerBullets, activeObjects.Enemies.Cast<ICollidable>());
            if (activeObjects.Player.IsAlive())
                CheckForBulletsCollisions(activeObjects.EnemyBullets, new ICollidable[] { activeObjects.Player });
        }

        private void CheckForBulletsCollisions(IEnumerable<Bullet> bullets, IEnumerable<ICollidable> targets)
        {
            foreach (Bullet bullet in bullets)
            {
                CheckBulletForCollisions(bullet,
                    targets.Concat(activeObjects.GetWalls()));
            }
        }

        private void CheckBulletForCollisions(Bullet bullet, IEnumerable<ICollidable> collidables)
        {
            foreach (ICollidable collidable in collidables.Where(c => c.Mode == CollidableMode.Solid))
            {
                Hitbox hitbox = collidable is ITouchableByBullets
                    ? (collidable as ITouchableByBullets).GetBulletsHitbox()
                    : collidable.GetCurrentHitbox();
                if (collisionsChecker.Collides(hitbox, bullet.OldPosition, bullet.Position))
                {
                    if (collidable is ITouchableByBullets)
                        (collidable as ITouchableByBullets).TakeDamage(bullet.Damage);
                    bullet.RegisterCollision();
                }
            }
        }
    }
}
