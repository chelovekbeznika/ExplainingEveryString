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
            AdjustObjectToWalls(activeObjects.Player);
            foreach (ICollidable movingEnemy
                in activeObjects.Enemies.OfType<ICollidable>().Where(CollidableIsMoving))
            {
                AdjustObjectToWalls(movingEnemy);
            }
        }

        private Boolean CollidableIsMoving(ICollidable collidable)
        {
            return !collidable.GetOldHitbox().Equals(collidable.GetCurrentHitbox());
        }

        private void AdjustObjectToWalls(ICollidable movingObject)
        {
            Hitbox oldHitbox = movingObject.GetOldHitbox();
            foreach (ICollidable wall in activeObjects.Walls.Concat(activeObjects.Enemies).OfType<ICollidable>())
            {
                Vector2? wallCorrection = null;
                collisionsChecker.TryToBypassWall(oldHitbox, movingObject.GetCurrentHitbox(),
                    wall.GetCurrentHitbox(), out wallCorrection);
                if (wallCorrection != null)
                    movingObject.Position = wallCorrection.Value;
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
