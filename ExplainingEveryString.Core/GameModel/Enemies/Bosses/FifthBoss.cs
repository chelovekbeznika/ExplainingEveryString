﻿using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FifthBoss : Enemy<FifthBossBlueprint>
    {
        private FifthBossLimb leftEye;
        private FifthBossLimb rightEye;
        private FifthBossLimb[] tentacles;
        private Single healthTentaclesThreshold;

        protected override void Construct(FifthBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.leftEye = new FifthBossLimb(blueprint.LeftEye, this, level);
            this.rightEye = new FifthBossLimb(blueprint.RightEye, this, level);

            this.healthTentaclesThreshold = blueprint.HealthThresholdToUseTentacles;
            tentacles = Enumerable.Range(0, blueprint.TentaclesOffsets.Length).Select((index) =>
            {
                var specification = new FifthBossLimbSpecification
                {
                    Angle = blueprint.Tentacle.Angle + blueprint.AngleOffsets[index],
                    Offset = blueprint.Tentacle.Offset + blueprint.TentaclesOffsets[index],
                    Sprite = blueprint.Tentacle.Sprite,
                    Weapon = blueprint.Tentacle.Weapon,
                    WeaponMovementCycle = blueprint.Tentacle.WeaponMovementCycle
                        .Select(pair => new Tuple<Single, Vector2>(pair.Item1, pair.Item2 + blueprint.TentaclesOffsets[index]))
                        .ToList()
                };
                return new FifthBossLimb(specification, this, level);
            }).ToArray();
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            if (!IsInAppearancePhase)
            {
                leftEye.Update(elapsedSeconds);
                rightEye.Update(elapsedSeconds);
                if (HitPoints <= healthTentaclesThreshold)
                    foreach (var tentacle in tentacles)
                        tentacle.Update(elapsedSeconds);
            }
        }

        public override IEnumerable<IDisplayble> GetParts()
        {
            return base.GetParts().Concat(tentacles).Concat(new[] { leftEye, rightEye });
        }
    }
}