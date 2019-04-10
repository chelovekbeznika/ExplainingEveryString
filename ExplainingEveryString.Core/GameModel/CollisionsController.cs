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
        private ActiveGameObjectsStorage activeObjects;
        private CollisionsChecker collisionsChecker = new CollisionsChecker();

        internal CollisionsController(ActiveGameObjectsStorage activeObjects)
        {
            this.activeObjects = activeObjects;
        }

        internal void CheckCollisions()
        {
            CheckEnemiesForCrashingIntoPlayer();
            PreventInterpenetrationOfGameObjects();
            CheckForBulletCollisions();
        }

        private void CheckEnemiesForCrashingIntoPlayer()
        {
            Player player = activeObjects.Player;
            foreach (ICrashable crashable in activeObjects.Enemies.OfType<ICrashable>())
            {
                if (collisionsChecker.Collides(crashable.GetCurrentHitbox(), player.GetCurrentHitbox()))
                {
                    crashable.Destroy();
                    player.TakeDamage(crashable.CollisionDamage);
                }
            }
        }

        private void PreventInterpenetrationOfGameObjects()
        {
            AdjustObjectToWalls(activeObjects.Player, null, null);
            foreach (ICollidable movingEnemy
                in activeObjects.Enemies.OfType<ICollidable>().Where(CollidableIsMoving))
            {
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
            foreach (ICollidable wall in activeObjects.Walls.Concat(activeObjects.Enemies).OfType<ICollidable>())
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

        private void CheckForBulletCollisions()
        {
            foreach (PlayerBullet playerBullet in activeObjects.PlayerBullets)
            {
                foreach (ICollidable collidable 
                    in activeObjects.Enemies.Concat(activeObjects.Walls).OfType<ICollidable>())
                {
                    if (collisionsChecker.Collides(collidable.GetCurrentHitbox(), playerBullet.OldPosition, playerBullet.Position))
                    {
                        if (collidable is ITouchableByBullets)
                            (collidable as ITouchableByBullets).TakeDamage(playerBullet.Damage);
                        playerBullet.RegisterCollision();
                    }
                }
            }
        }
    }
}
