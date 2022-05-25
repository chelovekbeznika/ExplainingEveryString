using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class FourthBossPartBlueprint : EnemyBlueprint
    {
        public Vector2 Offset { get; set; }
        [DefaultValue("0.0,0.0")]
        public Vector2 PulsationOffset { get; set; }
        public String PulsationTag { get; set; }
        public SpriteSpecification PhaseSwitchSprite { get; set; }
        public SpriteSpecification SecondPhaseSprite { get; set; }
        public WeaponSpecification WeaponOfRage { get; set; }
    }
}
