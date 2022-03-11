using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExplainingEveryString.Data.Specifications
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MoveTargetSelectType
    {
        NoTarget,
        TargetsList,
        RandomTargets,
        RandomFromListTargets,
        MoveTowardPlayer,
        MoveTowardPlayerByWaypoints,
        MoveByPlayerTracks
    }
}
