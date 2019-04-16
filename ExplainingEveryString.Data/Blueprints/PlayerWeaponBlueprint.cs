﻿using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public class PlayerWeaponBlueprint
    {
        public Single FireRate { get; set; }
        public Single BulletSpeed { get; set; }
        public Single Damage { get; set; }
        public Single WeaponRange { get; set; }

        public SpriteSpecification BulletSprite { get; set; }
        public SpecEffectSpecification ShootingEffect { get; set; }
    }
}
