using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal interface IFourthBossBrain
    {
        Vector2 Position { get; set; }
        Single Angle { get; }
        Boolean InAgony { get; }

        Single PulsationCoefficient(String tag);
        Boolean IsAlive();
        void SendAgonySignal();
        void TakeDamage(Single damage);
    }
}
