﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class PlayerBlueprint : Blueprint
    {
        public Single MaxSpeed { get; set; }
        public Single MaxAcceleration { get; set; }
        public PlayerWeaponBlueprint Weapon { get; set; }
        public SpecEffectSpecification DamageEffect { get; set; }

        internal override IEnumerable<string> GetSprites()
        {
            return new String[] { DefaultSpriteName, Weapon.BulletSpriteName };
        }
        internal override IEnumerable<SpecEffectSpecification> GetSpecEffects()
        {
            return new SpecEffectSpecification[] { Weapon.ShootingEffect, DamageEffect };
        }
    }
}