﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal interface ILevelFogOfWarExtractor
    {
        Rectangle[] GetFogOfWarRegions();
    }
}
