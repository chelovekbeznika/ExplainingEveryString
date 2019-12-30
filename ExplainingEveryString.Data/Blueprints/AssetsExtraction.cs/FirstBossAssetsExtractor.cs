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
            var baseSpecEffects = base.GetSpecEffects(blueprint);
            var phasesSpeceffects = blueprint.Phases
                .Select(phase => phase.Behavior.Weapon)
                .Where(weapon => weapon != null)
                .Select(weapon => weapon.ShootingEffect);
            return baseSpecEffects.Concat(phasesSpeceffects)
                .Concat(new SpecEffectSpecification[] { blueprint.PhaseOnEffect, blueprint.PhaseOffEffect });
        }

        public IEnumerable<SpriteSpecification> GetSprites(FirstBossBlueprint blueprint)
        {
            var baseSprites = base.GetSprites(blueprint);
            var phaseSprites = blueprint.Phases
                .SelectMany(phase => GetPhaseSprites(phase));
            return baseSprites.Concat(phaseSprites);
        }

        private IEnumerable<SpriteSpecification> GetPhaseSprites(FirstBossPhaseSpecification phaseSpecification)
        {
            var spriteSpecifications = new List<SpriteSpecification>
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
