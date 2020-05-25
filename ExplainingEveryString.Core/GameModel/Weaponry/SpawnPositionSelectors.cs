using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal interface ISpawnPositionSelector
    {
        SpawnSpecification GetNextSpawnSpecification();
    }

    internal class RandomSpawnPositionSelector : ISpawnPositionSelector
    {
        private Single maxRadius;

        internal RandomSpawnPositionSelector(Single maxRadius)
        {
            this.maxRadius = maxRadius;
        }

        public SpawnSpecification GetNextSpawnSpecification()
        {
            var angle = RandomUtility.Next() * MathHelper.TwoPi;
            var radius = RandomUtility.Next() * maxRadius;
            var relativeSpawnPosition = AngleConverter.ToVector(angle) * radius;
            return new SpawnSpecification { SpawnPoint = relativeSpawnPosition };
        }
    }

    internal class RelativeSpawnPositionSelector : ISpawnPositionSelector
    {
        private Vector2[] relativeSpawnPoints;
        private Int32 currentEnemySpawned = 0;

        internal RelativeSpawnPositionSelector(Vector2[] relativeSpawnPoints)
        {
            this.relativeSpawnPoints = relativeSpawnPoints;
        }

        public SpawnSpecification GetNextSpawnSpecification()
        {
            var relativeSpawnPosition = relativeSpawnPoints[currentEnemySpawned % relativeSpawnPoints.Length];
            currentEnemySpawned += 1;
            return new SpawnSpecification { SpawnPoint = relativeSpawnPosition };
        }
    }

    internal class CustomSpawnPositionSelector : ISpawnPositionSelector
    {
        private List<SpawnSpecification> availableSpawns;
        private Queue<SpawnSpecification> recentlyTaken;
        private Int32 betweenRepeats;

        internal CustomSpawnPositionSelector(SpawnSpecification[] customSpawns, Int32 betweenRepeats)
        {
            this.availableSpawns = new List<SpawnSpecification>(customSpawns);
            this.betweenRepeats = betweenRepeats;
            this.recentlyTaken = new Queue<SpawnSpecification>(betweenRepeats);
        }

        public SpawnSpecification GetNextSpawnSpecification()
        {
            if (recentlyTaken.Count > betweenRepeats)
                availableSpawns.Add(recentlyTaken.Dequeue());
            var nextSpawn = availableSpawns[RandomUtility.NextInt(availableSpawns.Count)];
            if (recentlyTaken.Count < betweenRepeats)
            {
                availableSpawns.Remove(nextSpawn);
                recentlyTaken.Enqueue(nextSpawn);
            }
            return nextSpawn;
        }
    }
}
