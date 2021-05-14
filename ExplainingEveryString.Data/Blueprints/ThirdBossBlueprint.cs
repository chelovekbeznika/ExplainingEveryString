using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Data.Blueprints
{
    public class ThirdBossBlueprint : EnemyBlueprint
    {
        public ThirdBossAimersSpecification Aimers { get; set; }
        public WeaponSpecification SmallWeapon { get; set; }
        public Vector2[] SmallWeaponOffsets { get; set; }
    }
}
