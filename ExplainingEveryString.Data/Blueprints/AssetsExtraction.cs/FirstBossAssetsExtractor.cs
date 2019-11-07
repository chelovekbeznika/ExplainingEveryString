using System;
using System.Linq;
using System.Collections.Generic;
using ExplainingEveryString.Data.Specifications;

namespace ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs
{
    class FirstBossAssetsExtractor : EnemyAssetsExtractor, IAssetsExtractor<FirstBossBlueprint>
    {
        public IEnumerable<SpecEffectSpecification> GetSpecEffects(FirstBossBlueprint blueprint)
        {
            IEnumerable<SpecEffectSpecification> baseSpecEffects = base.GetSpecEffects(blueprint);
            IEnumerable<SpecEffectSpecification> phasesSpeceffects = blueprint.Phases
                .Select(phase => phase.Behavior.Weapon)
                .Where(weapon => weapon != null)
                .Select(weapon => weapon.ShootingEffect);
            return baseSpecEffects.Concat(phasesSpeceffects);
        }

        public IEnumerable<SpriteSpecification> GetSprites(FirstBossBlueprint blueprint)
        {
            IEnumerable<SpriteSpecification> baseSprites = base.GetSprites(blueprint);
            IEnumerable<SpriteSpecification> phaseSprites = blueprint.Phases
                .SelectMany(phase => GetPhaseSprites(phase));
            return baseSprites.Concat(phaseSprites);
        }

        private IEnumerable<SpriteSpecification> GetPhaseSprites(FirstBossPhaseSpecification phaseSpecification)
        {
            List<SpriteSpecification> spriteSpecifications = new List<SpriteSpecification>
            {
                phaseSpecification.On,
                phaseSpecification.Off,
                phaseSpecification.Phase
            };
            if (phaseSpecification.Behavior.Weapon != null)
                spriteSpecifications.AddRange(GetSpritesFromWeapon(phaseSpecification.Behavior.Weapon));
            return spriteSpecifications;
        }
    }
}
