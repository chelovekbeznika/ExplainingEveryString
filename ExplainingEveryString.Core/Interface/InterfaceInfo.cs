using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceInfo
    {
        internal Single GameTime { get; set; }
        internal List<EnemyInterfaceInfo> Enemies { get; set; }
        internal EnemyInterfaceInfo Boss { get; set; }
        internal PlayerInterfaceInfo Player { get; set; }
    }

    internal class EnemyInterfaceInfo
    {
        internal Single MaxHealth { get; set; }
        internal Single Health { get; set; }
        internal Rectangle PositionOnScreen { get; set; }
    }

    internal class PlayerInterfaceInfo
    {
        private Single health;
        internal Single MaxHealth { get; set; }
        internal Single Health { get => health; set => health = value > 0 ? value : 0; }
        internal Single DashCooldown { get; set; }
        internal Single TillDashRecharge { get; set; }
        internal DashState DashState { get; set; }
    }

    internal enum DashState { Active, Nonavailable, Available }
}
