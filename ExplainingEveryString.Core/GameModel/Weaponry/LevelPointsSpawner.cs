﻿using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal interface ISpawnPositionSelector
    {
        Vector2 GetNextSpawnPosition();
    }

    internal class LevelSpawnPositionSelector : ISpawnPositionSelector
    {
        private Vector2[] spawnPoints;

        internal LevelSpawnPositionSelector(Vector2[] levelSpawnPoints)
        {
            this.spawnPoints = levelSpawnPoints;
        }

        public Vector2 GetNextSpawnPosition()
        {
            return spawnPoints[RandomUtility.NextInt(spawnPoints.Length)];
        }
    }

    internal class RandomSpawnPositionSelector : ISpawnPositionSelector
    {
        private Func<Vector2> spawnerPositionLocator;
        private Single maxRadius;

        internal RandomSpawnPositionSelector(Func<Vector2> spawnerPositionLocator, Single maxRadius)
        {
            this.spawnerPositionLocator = spawnerPositionLocator;
            this.maxRadius = maxRadius;
        }

        public Vector2 GetNextSpawnPosition()
        {
            Single angle = RandomUtility.Next() * MathHelper.TwoPi;
            Single radius = RandomUtility.Next() * maxRadius;
            Vector2 relativeSpawnPosition = AngleConverter.ToVector(angle) * radius;
            return spawnerPositionLocator() + relativeSpawnPosition;
        }
    }

    internal class RelativeSpawnPositionSelector : ISpawnPositionSelector
    {
        private Func<Vector2> spawnerPositionLocator;
        private Vector2[] relativeSpawnPoints;
        private Int32 currentEnemySpawned = 0;

        internal RelativeSpawnPositionSelector(Func<Vector2> spawnerPositionLocator, Vector2[] relativeSpawnPoints)
        {
            this.spawnerPositionLocator = spawnerPositionLocator;
            this.relativeSpawnPoints = relativeSpawnPoints;
        }

        public Vector2 GetNextSpawnPosition()
        {
            Vector2 relativeSpawnPosition = relativeSpawnPoints[currentEnemySpawned % relativeSpawnPoints.Length];
            currentEnemySpawned += 1;
            return spawnerPositionLocator() + relativeSpawnPosition;
        }
    }
}