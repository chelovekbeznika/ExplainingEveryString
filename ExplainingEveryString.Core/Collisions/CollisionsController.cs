﻿using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.GameModel.Weaponry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Collisions
{
    internal class CollisionsController
    {
        private readonly String[] stoppedByPitsCollidableTags = new[] { "Tank" };
        private ActiveActorsStorage activeObjects;
        private CollisionsChecker collisionsChecker = new CollisionsChecker();
        private Dictionary<ITouchableByBullets, Single> futureBlastWaveVictims = new Dictionary<ITouchableByBullets, Single>();

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

        internal Boolean IsItPossibleToRide(Vector2 a, Vector2 b, String collidableTag)
        {
            var sectorsOnTheWay = SpatialPartioningHelper.GetSectors(a, b);
            var obstacles = activeObjects.GetWalls(sectorsOnTheWay);
            if (!stoppedByPitsCollidableTags.Contains(collidableTag))
                obstacles = obstacles.Where(o => o.CollidableMode != CollidableMode.Pit);
            return obstacles.All(w => !collisionsChecker.Collides(w.GetCurrentHitbox(), a, b));
        }

        private void CheckEnemiesForCrashingIntoPlayer()
        {
            var player = activeObjects.Player;
            var dash = player.DashController;
            foreach (var crashable in activeObjects.Enemies.OfType<ICrashable>()
                .Where(c => c.CollidableMode != CollidableMode.Ghost)
                .Where(c => !(dash.IsActive && dash.CollideTagsDefense.Contains(c.CollideTag))))
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

            var enemies = activeObjects.Enemies.OfType<IMovableCollidable>()
                .Where(c => c.CollidableMode != CollidableMode.Ghost).ToArray();
            var movingEnemies = enemies.Where(CollidableIsMoving).ToArray();
            var stoppedEnemies = new List<IMovableCollidable>();
            foreach (var movingEnemy in movingEnemies)
            {
                var beforeMovePosition = movingEnemy.OldPosition;
                var bumpedIntoOtherEnemy = IsBumpedIntoOtherEnemies(movingEnemy, movingEnemies, stoppedEnemies);

                if (bumpedIntoOtherEnemy)
                {
                    movingEnemy.Position = beforeMovePosition;
                    stoppedEnemies.Add(movingEnemy);
                }
                else
                {
                    var ridesThroughWalls = !stoppedByPitsCollidableTags.Contains(movingEnemy.CollideTag);
                    AdjustObjectToWalls(movingEnemy, ridesThroughWalls, null, null);
                }                
            }
        }

        private Boolean IsBumpedIntoOtherEnemies(IMovableCollidable enemy, IEnumerable<IMovableCollidable> movingEnemies, 
            List<IMovableCollidable> stoppedEnemies)
        {
            if (enemy.CollideTag != null)
            {
                foreach (var otherEnemy in movingEnemies.Except(stoppedEnemies)
                    .Where(e => e.CollideTag == enemy.CollideTag && e != enemy))
                {
                    if (collisionsChecker.Collides(otherEnemy.GetOldHitbox(), enemy.GetCurrentHitbox())
                        || collisionsChecker.Collides(otherEnemy.GetCurrentHitbox(), enemy.GetCurrentHitbox()))
                        return true;
                }
                return false;
            }
            else
                return false;
        }

        private Boolean CollidableIsMoving(IMovableCollidable collidable)
        {
            return !collidable.GetOldHitbox().Equals(collidable.GetCurrentHitbox());
        }

        private void AdjustObjectToWalls(IMovableCollidable movingObject, Boolean ridesThroughPit,
            ICollidable tryVerticalMovePriorityForThis, Hitbox? previousOldHitbox)
        {
            if (movingObject.CollidableMode == CollidableMode.Teleporter)
                return;

            var oldHitbox = previousOldHitbox == null ? movingObject.GetOldHitbox() : previousOldHitbox.Value;
            var savedMovingObjectPosition = movingObject.Position;
            var bumpIntoThisWall = ridesThroughPit
                ? new Func<ICollidable, Boolean>(c => c.CollidableMode == CollidableMode.Solid)
                : new Func<ICollidable, Boolean>(c => c.CollidableMode == CollidableMode.Solid || c.CollidableMode == CollidableMode.Pit);
            var walls = activeObjects.GetWalls().Where(bumpIntoThisWall);

            WallsCheck(movingObject, walls, oldHitbox, tryVerticalMovePriorityForThis, out ICollidable touchingToCorner);

            if (touchingToCorner != null && oldHitbox.Equals(movingObject.GetCurrentHitbox()))
            {
                movingObject.Position = savedMovingObjectPosition;
                AdjustObjectToWalls(movingObject, ridesThroughPit, touchingToCorner, oldHitbox);
            }
        }

        private void WallsCheck(IMovableCollidable movingObject, IEnumerable<ICollidable> walls, Hitbox oldHitbox, 
            ICollidable tryVerticalMovePriorityForThis, out ICollidable touchingToCorner)
        {
            touchingToCorner = null;
            foreach (var wall in walls)
            {
                var horizontalPriority = wall != tryVerticalMovePriorityForThis;
                collisionsChecker.TryToBypassWall(oldHitbox, movingObject.GetCurrentHitbox(),
                    wall.GetCurrentHitbox(), out Vector2? wallCorrection,
                    horizontalPriority, out Boolean ridingIntoCorner);

                if (wallCorrection != null)
                {
                    if (ridingIntoCorner && tryVerticalMovePriorityForThis == null)
                        touchingToCorner = wall;
                    movingObject.Position = wallCorrection.Value;
                }
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
            foreach (var bullet in bullets)
            {
                var sector = SpatialPartioningHelper.GetSector(bullet.Position);
                CheckBulletForCollisions(bullet, targets.Concat(activeObjects.GetWalls(sector)));
            }
        }

        private void CheckBulletForCollisions(Bullet bullet, IEnumerable<ICollidable> collidables)
        {
            var bulletCollisionHappened = false;
            futureBlastWaveVictims.Clear();
            foreach (var collidable in collidables.Where(c => c.CollidableMode == CollidableMode.Solid || c.CollidableMode == CollidableMode.Teleporter))
            {
                var hitbox = collidable is ITouchableByBullets
                    ? (collidable as ITouchableByBullets).GetBulletsHitbox()
                    : collidable.GetCurrentHitbox();

                if (collisionsChecker.Collides(hitbox, bullet.OldPosition, bullet.CollisionCheckPosition))
                {
                    if (collidable is ITouchableByBullets)
                    {
                        if (!bullet.IsBlastsBefore)
                            (collidable as ITouchableByBullets).TakeDamage(bullet.Damage);
                        else
                            RegisterBlastVictim(collidable as ITouchableByBullets, bullet);
                    } 
                    bullet.RegisterCollision();
                    bulletCollisionHappened = true;
                }
                else if (bullet.BlastWaveRadius > 0 && collidable is ITouchableByBullets 
                    && (collidable.Position - bullet.Position).Length() < bullet.BlastWaveRadius)
                {
                    RegisterBlastVictim(collidable as ITouchableByBullets, bullet);
                }
            }
            if (bulletCollisionHappened && bullet.BlastWaveRadius > 0)
                foreach (var (victim, damage) in futureBlastWaveVictims)
                    victim.TakeDamage(damage);
        }

        private void RegisterBlastVictim(ITouchableByBullets blastVictim, Bullet bullet)
        {
            var damageCoeff = 1 - (blastVictim.Position - bullet.Position).Length() / bullet.BlastWaveRadius;
            futureBlastWaveVictims.Add(blastVictim, damageCoeff * bullet.Damage);
        }
    }
}
