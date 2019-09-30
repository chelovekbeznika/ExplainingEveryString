using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class EpicEventArgs : EventArgs
    {
        internal Vector2 Position { get; set; }
        internal SpecEffectSpecification SpecEffectSpecification { get; set; }
        internal Single Angle { get; set; }
    }
}
