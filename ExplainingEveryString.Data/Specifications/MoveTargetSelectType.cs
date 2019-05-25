using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public enum MoveTargetSelectType
    {
        NoTarget,
        TargetsList,
        RandomTargets,
        MoveTowardPlayer
    }
}
