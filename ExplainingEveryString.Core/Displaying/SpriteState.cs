using ExplainingEveryString.Data.Blueprints;
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

        internal SpriteState(SpriteSpecification spriteSpecification)
        {
            Name = spriteSpecification.Name;
            AnimationCycle = spriteSpecification.AnimationCycle;
            ElapsedTime = 0;
        }

        internal void Update(Single elapsedSeconds)
        {
            ElapsedTime += elapsedSeconds;
        }
    }
}
