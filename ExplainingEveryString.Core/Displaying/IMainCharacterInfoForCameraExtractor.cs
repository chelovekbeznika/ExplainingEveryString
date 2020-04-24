using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal interface IMainCharacterInfoForCameraExtractor
    {
        Vector2 Position { get; }
        Vector2 FireDirection { get; }
        Single Focused { get; }
    }
}
