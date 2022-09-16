using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private Boolean randomOrder;

        internal CustomSpawnPositionSelector(SpawnSpecification[] customSpawns, Int32 betweenRepeats, Boolean randomOrder)
        {
            this.availableSpawns = new List<SpawnSpecification>(customSpawns);
            this.betweenRepeats = betweenRepeats;
            this.recentlyTaken = new Queue<SpawnSpecification>(betweenRepeats);
            this.randomOrder = randomOrder;
        }

        public SpawnSpecification GetNextSpawnSpecification()
        {
            SpawnSpecification nextSpawn;
            if (randomOrder)
            {
                while (recentlyTaken.Count > betweenRepeats && recentlyTaken.Any())
                    availableSpawns.Add(recentlyTaken.Dequeue());
                nextSpawn = availableSpawns[RandomUtility.NextInt(availableSpawns.Count)];
                if (recentlyTaken.Count <= betweenRepeats)
                {
                    availableSpawns.Remove(nextSpawn);
                    recentlyTaken.Enqueue(nextSpawn);
                }
            }
            else
            {
                nextSpawn = availableSpawns[0];
                availableSpawns.RemoveAt(0);
                availableSpawns.Add(nextSpawn);
            }
            return nextSpawn;
        }
    }
}
