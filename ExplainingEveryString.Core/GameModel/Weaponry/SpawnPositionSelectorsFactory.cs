using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal static class SpawnPositionSelectorsFactory
    {
        internal static ISpawnPositionSelector Get(SpawnPositionSelectorSpecification specification,
            Func<Vector2> spawnerLocator, Vector2[] levelSpawnPoints)
        {
            switch (specification.PositionSelectionType)
            {
                case SpawnPositionSelectionType.LevelSpawnPoints:
                    return new LevelSpawnPositionSelector(levelSpawnPoints);
                case SpawnPositionSelectionType.RandomInCircle:
                    Single maxRadius = (specification as RandomSpawnPositionSelectorSpecification).SpawnRadius;
                    return new RandomSpawnPositionSelector(spawnerLocator, maxRadius);
                case SpawnPositionSelectionType.RelativeToSpawner:
                    Vector2[] spawnPositions = (specification as RelativeSpawnPositionSelectorSpecificaton).SpawnPositions;
                    return new RelativeSpawnPositionSelector(spawnerLocator, spawnPositions);
                default:
                    throw new ArgumentException("Unknown SpawnPositionSelectionType");
            }
        }
    }
}
