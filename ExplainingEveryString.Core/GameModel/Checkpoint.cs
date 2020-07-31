using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Checkpoint
    {
        internal String Name { get; set; }
        internal Vector2 StartPosition { get; set; }
        internal Int32 StartWave { get; set; }
        internal ArsenalSpecification PlayerArsenal { get; set; }
    }
}
