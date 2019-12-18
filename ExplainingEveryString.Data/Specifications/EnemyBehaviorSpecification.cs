using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class EnemyBehaviorSpecification
    {
        [DefaultValue(null)]
        public MoverSpecification Mover { get; set; }
        [DefaultValue(MoveTargetSelectType.NoTarget)]
        public MoveTargetSelectType MoveTargetSelectType { get; set; }
        [DefaultValue(null)]
        public WeaponSpecification Weapon { get; set; }
        [DefaultValue(null)]
        public SpawnerSpecification Spawner { get; set; }
        [DefaultValue(null)]
        public PostMortemSurpriseSpecification PostMortemSurprise { get; set; }
    }
}
