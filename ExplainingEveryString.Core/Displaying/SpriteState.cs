﻿using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class SpriteState
    {
        internal String Name { get; private set; }
        internal Single AnimationCycle { get; set; }
        internal Single Angle { get; set; }
        internal Boolean Looping { get; set; } = true;

        private Single elapsedTime;
        internal Single ElapsedTime
        {
            get
            {
                if (Looping)
                    return elapsedTime;
                else
                    return elapsedTime < AnimationCycle ? elapsedTime : AnimationCycle - Math.Constants.Epsilon;
            }
            private set
            {
                elapsedTime = value;
            }
        }

        internal SpriteState(SpriteSpecification spriteSpecification)
        {
            Name = spriteSpecification.Name;
            AnimationCycle = spriteSpecification.AnimationCycle;
            ElapsedTime = 0;
            Angle = 0;
        }

        internal SpriteState(String name, Single animationCycle)
        {
            Name = name;
            AnimationCycle = animationCycle;
        }

        internal void Update(Single elapsedSeconds)
        {
            ElapsedTime += elapsedSeconds;
        }

        internal void StartOver()
        {
            ElapsedTime = 0;
        }
    }
}
