﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceInfo
    {
        internal Single GameTime { get; set; }
        internal List<EnemyInterfaceInfo> Enemies { get; set; }
        internal List<Vector2> HiddenEnemies { get; set; }
        internal List<EnemyInterfaceInfo> Bosses { get; set; }
        internal PlayerInterfaceInfo Player { get; set; }
    }

    internal class EnemyInterfaceInfo
    {
        private Single health;
        internal Single MaxHealth { get; set; }
        internal Single Health { get => health; set => health = value > 0 ? value : 0; }
        internal Single FromLastHit { get; set; }
        internal Rectangle PositionOnScreen { get; set; }
    }

    internal class PlayerInterfaceInfo
    {
        private Single health;
        internal Single MaxHealth { get; set; }
        internal Single Health { get => health; set => health = value > 0 ? value : 0; }
        internal Single FromLastHit { get; set; }
        internal Single DashCooldown { get; set; }
        internal Single TillDashRecharge { get; set; }
        internal DashState DashState { get; set; }
        internal PlayerWeaponInfo Weapon { get; set; }
    }

    internal class PlayerWeaponInfo
    {
        internal String Name { get; set; }
        internal Boolean AmmoLimited { get; set; }
        internal Int32 CurrentAmmo { get; set; }
        internal Int32 MaxAmmo { get; set; }
    }

    internal enum DashState { Active, Nonavailable, Available }
}
