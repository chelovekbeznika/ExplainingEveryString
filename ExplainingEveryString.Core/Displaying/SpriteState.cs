using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Displaying
{
    internal class SpriteState
    {
        internal String Name { get; private set; }
        internal Single ElapsedTime { get; private set; }
        internal Single AnimationCycle { get; private set; }
        internal Single Angle { get; set; }

        internal SpriteState(SpriteSpecification spriteSpecification)
        {
            Name = spriteSpecification.Name;
            AnimationCycle = spriteSpecification.AnimationCycle;
            ElapsedTime = 0;
            Angle = 0;
        }

        internal void Update(Single elapsedSeconds)
        {
            ElapsedTime += elapsedSeconds;
        }
    }
}
