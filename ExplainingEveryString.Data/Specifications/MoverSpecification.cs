using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Specifications
{
    public class MoverSpecification
    { 
        [JsonConverter(typeof(StringEnumConverter))]
        public MoveType Type { get; set; }
        public Dictionary<String, Single> Parameters { get; set; }
    }

    public enum MoveType
    {
        StayingStill,
        Linear,
        Acceleration
    }
}
