using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal interface IFourthBossBrain
    {
        Vector2 Position { get; set; }
        Single Angle { get; }
        Single PulsationCoefficient(String tag);
        Boolean IsAlive();
    }
}
