using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Data.Blueprints
{
    public class FifthBossBlueprint : EnemyBlueprint
    {
        public SpriteSpecification EyeSprite { get; set; }
        public Vector2 LeftEyeOffset { get; set; }
        public Vector2 RightEyeOffset { get; set; }
        public List<Tuple<Single, Vector2>> LeftWeaponMovementCycle { get; set; }
        public List<Tuple<Single, Vector2>> RightWeaponMovementCycle { get; set; }
        public WeaponSpecification LeftWeapon { get; set; }
        public WeaponSpecification RightWeapon { get; set; }
        public Single HealthThresholdToSpawnHelper { get; set; }
        public String HelperType { get; set; }
    }
}
