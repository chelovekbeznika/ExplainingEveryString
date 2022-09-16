using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal static class SpawnPositionSelectorsFactory
    {
        internal static ISpawnPositionSelector Get(SpawnPositionSelectorSpecification specification, 
            SpawnSpecification[] spawnSpecifications)
        {
            switch (specification.PositionSelectionType)
            {
                case SpawnPositionSelectionType.RandomInCircle:
                    var maxRadius = (specification as RandomSpawnPositionSelectorSpecification).SpawnRadius;
                    return new RandomSpawnPositionSelector(maxRadius);
                case SpawnPositionSelectionType.RelativeToSpawner:
                    var spawnPositions = (specification as RelativeSpawnPositionSelectorSpecificaton).SpawnPositions;
                    return new RelativeSpawnPositionSelector(spawnPositions);
                case SpawnPositionSelectionType.Custom:
                    var betweenRepeats = (specification as CustomSpawnPositionSelectorSpecification).BetweenRepeats;
                    var randomOrder = (specification as CustomSpawnPositionSelectorSpecification).RandomOrder;
                    return new CustomSpawnPositionSelector(spawnSpecifications, betweenRepeats, randomOrder);
                default:
                    throw new ArgumentException("Unknown SpawnPositionSelectionType");
            }
        }
    }
}
