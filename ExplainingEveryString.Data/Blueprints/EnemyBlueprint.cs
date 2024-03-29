﻿using ExplainingEveryString.Data.Specifications;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class EnemyBlueprint : Blueprint
    {
        [DefaultValue("Enemy")]
        public String Type { get; set; }
        public Single CollisionDamage { get; set; }
        [DefaultValue(null)]
        public String CollideTag { get; set; }
        [DefaultValue(0)]
        public Single BulletsHeight { get; set; }
        [DefaultValue(0)]
        public Single BulletsWidth { get; set; }
        [DefaultValue(false)]
        public Boolean HideHealthBar { get; set; }
        public EnemyBehaviorSpecification Behavior { get; set; }
        public SpecEffectSpecification DeathEffect { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification BeforeAppearanceEffect { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification AfterAppearanceEffect { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification GoalAchievedEffect { get; set; }
        public SpriteSpecification AppearancePhaseSprite { get; set; }
        public Single DefaultAppearancePhaseDuration { get; set; }
    }
}
