﻿using Microsoft.Xna.Framework;
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

            IEnumerable<ICollidable> enemies = activeObjects.Enemies.OfType<ICollidable>().ToArray();
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
            foreach (ICollidable wall in activeObjects.Walls.OfType<ICollidable>())
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