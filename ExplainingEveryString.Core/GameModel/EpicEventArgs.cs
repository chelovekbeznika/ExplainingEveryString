using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class EpicEventArgs : EventArgs
    {
        internal Vector2 Position { get; set; }
        internal SpecEffectSpecification SpecEffectSpecification { get; set; }
    }
}
