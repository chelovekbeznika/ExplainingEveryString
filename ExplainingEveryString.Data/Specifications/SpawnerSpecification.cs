using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class SpawnerSpecification
    {
        public Int32 MaxSpawned { get; set; }
        public ReloaderSpecification Reloader { get; set; }
        public String BlueprintType { get; set; }
        [DefaultValue(0.0)]
        public Single AppearancePhase { get; set; }
        [DefaultValue(true)]
        public Boolean SpawnPositionRelativeToCurrentPosition { get; set; }
        public SpawnPositionSelectorSpecification PositionSelector { get; set; }
    }
}
