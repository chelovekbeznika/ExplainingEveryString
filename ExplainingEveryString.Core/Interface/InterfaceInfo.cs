using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceInfo
    {
        internal List<EnemyInterfaceInfo> Enemies { get; set; }
        internal List<Vector2> HiddenEnemies { get; set; }
        internal List<EnemyInterfaceInfo> Bosses { get; set; }
        internal PlayerInterfaceInfo Player { get; set; }
        internal List<Vector2> EnemiesLevelPositions { get; set; }
        internal List<Vector2> BossesLevelPositions { get; set; }
    }

    internal class InterfaceGameTimeInfo
    {
        internal Single CurrentTime { get; set; }
        internal Single? CurrentLevelRecord { get; set; }
    }

    internal class EnemyInterfaceInfo
    {
        private Single health;
        internal Vector2 LevelPosition { get; set; }
        internal Single MaxHealth { get; set; }
        internal Single Health { get => health; set => health = value > 0 ? value : 0; }
        internal Single FromLastHit { get; set; }
        internal Rectangle PositionOnScreen { get; set; }
    }

    internal class PlayerInterfaceInfo
    {
        private Single health;
        internal Vector2 LevelPosition { get; set; }
        internal Single MaxHealth { get; set; }
        internal Single Health { get => health; set => health = value > 0 ? value : 0; }
        internal Single FromLastHit { get; set; }
        internal Single FromLastCheckpoint { get; set; }
        internal Single DashCooldown { get; set; }
        internal Single TillDashRecharge { get; set; }
        internal DashState DashState { get; set; }
        internal PlayerWeaponInterfaceInfo Weapon { get; set; }
        internal EnemyInterfaceInfo HomingTarget { get; set; }
    }

    internal class PlayerWeaponInterfaceInfo
    {
        internal String SelectedWeapon { get; set; }
        internal List<String> AvailableWeapons { get; set; }
        internal Int32 CurrentAmmo { get; set; }
        internal Int32 MaxAmmo { get; set; }
        internal Int32? AmmoStock { get; set; }
        internal Single? ReloadRemained { get; set; }
    }

    internal enum DashState { Active, Nonavailable, Available }
}
