﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal interface IFourthBossBrain
    {
        Vector2 Position { get; }
        Single Angle { get; }
        Boolean IsAlive();
    }
}