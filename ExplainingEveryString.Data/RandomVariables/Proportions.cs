using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Data.RandomVariables
{
    public class Proportions<T>
    {
        public List<T> PossibleValues { get; set; }
        public List<Int32> Weights { get; set; }
        [JsonIgnore]
        public Int32 Sum => Weights.Sum();
        [JsonIgnore]
        public Int32 Length => PossibleValues.Count;
    }
}
