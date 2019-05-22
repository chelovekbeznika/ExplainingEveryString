﻿using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal interface IAimer
    {
        Vector2 GetFireDirection();
        Boolean IsFiring();
    }
}
