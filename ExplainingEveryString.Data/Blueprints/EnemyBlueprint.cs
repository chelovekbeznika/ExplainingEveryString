using ExplainingEveryString.Data.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class EnemyBlueprint : Blueprint
    {
        [DefaultValue("Enemy")]
        public String Type { get; set; }
        public Single CollisionDamage { get; set; }
        [DefaultValue(null)]
        public MoverSpecification Mover { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(MoveTargetSelectType.NoTarget)]
        public MoveTargetSelectType MoveTargetSelectType { get; set; }
        public SpecEffectSpecification DeathEffect { get; set; }

        internal override IEnumerable<SpecEffectSpecification> GetSpecEffects()
        {
            return new SpecEffectSpecification[] { DeathEffect };
        }
    }
}
