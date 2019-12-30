using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;

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
                    var maxRadius = (specification as RandomSpawnPositionSelectorSpecification).SpawnRadius;
                    return new RandomSpawnPositionSelector(spawnerLocator, maxRadius);
                case SpawnPositionSelectionType.RelativeToSpawner:
                    var spawnPositions = (specification as RelativeSpawnPositionSelectorSpecificaton).SpawnPositions;
                    return new RelativeSpawnPositionSelector(spawnerLocator, spawnPositions);
                default:
                    throw new ArgumentException("Unknown SpawnPositionSelectionType");
            }
        }
    }
}
