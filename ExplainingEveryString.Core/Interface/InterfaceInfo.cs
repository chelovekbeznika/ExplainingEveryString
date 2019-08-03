using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceInfo
    {
        private Single health;
        internal Single MaxHealth { get; set; }
        internal Single Health { get => health; set => health = value > 0 ? value : 0; }
        internal Single GameTime { get; set; }
        internal List<EnemyInterfaceInfo> Enemies { get; set; }
    }

    internal class EnemyInterfaceInfo
    {
        internal Single MaxHealth { get; set; }
        internal Single Health { get; set; }
        internal Rectangle PositionOnScreen { get; set; }
    }
}
