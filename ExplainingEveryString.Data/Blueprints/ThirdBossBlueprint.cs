using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Data.Blueprints
{
    public class ThirdBossBlueprint : EnemyBlueprint
    {
        public ThirdBossAimersSpecification Aimers { get; set; }
        public WeaponSpecification SmallWeapon { get; set; }
        public Vector2[] SmallWeaponOffsets { get; set; }
        public ThirdBossBigGunSpawnSpecification BigGunSpawn { get; set; }
    }
}
