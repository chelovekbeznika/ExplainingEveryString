using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class ObstacleBlueprint : Blueprint
    {
        [DefaultValue(false)]
        public Boolean JustDecoration { get; set; }
    }
}
