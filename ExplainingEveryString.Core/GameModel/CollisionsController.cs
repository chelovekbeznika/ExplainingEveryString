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
                    crashable.Destroy();
                    player.TakeDamage(crashable.CollisionDamage);
                }
            }
        }

        private void PreventInterpenetrationOfActors()
        {
            AdjustObjectToWalls(activeObjects.Player, null, null);

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
                    AdjustObjectToWalls(movingEnemy, null, null);             
            }
        }

        private Boolean CollidableIsMoving(ICollidable collidable)
        {
            return !collidable.GetOldHitbox().Equals(collidable.GetCurrentHitbox());
        }

        private void AdjustObjectToWalls(ICollidable movingObject, 
            ICollidable tryVerticalMovePriorityForThis, Hitbox? previousOldHitbox)
        {
            Hitbox oldHitbox = previousOldHitbox == null ? movingObject.GetOldHitbox() : previousOldHitbox.Value;
            Vector2 savedMovingObjectPosition = movingObject.Position;
            ICollidable touchingToCorner = null;
            foreach (ICollidable wall in activeObjects.GetWalls().Where(c => c.Mode != CollidableMode.Ghost))
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
                AdjustObjectToWalls(movingObject, touchingToCorner, oldHitbox);
            }
        }

        private void CheckForBulletsCollisions()
        {
            foreach (Bullet bullet in activeObjects.PlayerBullets)
            {
                CheckBulletForCollisions(bullet, 
                    activeObjects.Enemies.Concat(activeObjects.GetWalls()).OfType<ICollidable>());
            }
            foreach (Bullet bullet in activeObjects.EnemyBullets)
            {
                CheckBulletForCollisions(bullet,
                    new ICollidable[] { activeObjects.Player }.Concat(activeObjects.GetWalls()).OfType<ICollidable>());
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
