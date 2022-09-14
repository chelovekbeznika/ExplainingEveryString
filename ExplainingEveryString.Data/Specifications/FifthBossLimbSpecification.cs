using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class FifthBossLimbSpecification
    {
        public SpriteSpecification Sprite { get; set; }
        public Vector2 Offset { get; set; }
        public List<Tuple<Single, Vector2>> WeaponMovementCycle { get; set; }
        public WeaponSpecification Weapon { get; set; }
        [DefaultValue(null)]
        public Single? Angle { get; set; }
    }
}
