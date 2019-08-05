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
        [DefaultValue(null)]
        public WeaponSpecification Weapon { get; set; }
        [DefaultValue(null)]
        public SpawnerSpecification Spawner { get; set; }
        [DefaultValue(null)]
        public PostMortemSurpriseSpecification PostMortemSurprise { get; set; }
        public SpecEffectSpecification DeathEffect { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification BeforeAppearanceEffect { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification AfterAppearanceEffect { get; set; }
        public SpriteSpecification AppearancePhaseSprite { get; set; }
        public Single DefaultAppearancePhaseDuration { get; set; }

        internal override IEnumerable<SpecEffectSpecification> GetSpecEffects()
        {
            List<SpecEffectSpecification> specEffects = new List<SpecEffectSpecification>
            {
                DeathEffect, BeforeAppearanceEffect, AfterAppearanceEffect
            };
            if (Weapon != null)
                specEffects.Add(Weapon.ShootingEffect);
            return specEffects;
        }

        internal override IEnumerable<SpriteSpecification> GetSprites()
        {
            IEnumerable<SpriteSpecification> sprites = 
                base.GetSprites().Concat( new SpriteSpecification[] { AppearancePhaseSprite});
            return Weapon != null ? sprites.Concat(Weapon.GetSprites()) : sprites;
        }
    }
}
