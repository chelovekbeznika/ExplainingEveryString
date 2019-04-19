using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class ShootEventArgs : EventArgs
    {
        internal Bullet Bullet { get; set; }
        internal Single FirstUpdateTime { get; set; }
    }
}
