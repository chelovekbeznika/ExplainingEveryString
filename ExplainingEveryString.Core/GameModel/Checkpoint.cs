using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Checkpoint
    {
        internal String Name { get; set; }
        internal Vector2 StartPosition { get; set; }
        internal Int32 StartWave { get; set; }
    }
}
