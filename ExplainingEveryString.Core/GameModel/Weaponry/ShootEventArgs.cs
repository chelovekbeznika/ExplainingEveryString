using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class ShootEventArgs : EventArgs
    {
        internal Bullet Bullet { get; set; }
        internal Single FirstUpdateTime { get; set; }
    }
}
