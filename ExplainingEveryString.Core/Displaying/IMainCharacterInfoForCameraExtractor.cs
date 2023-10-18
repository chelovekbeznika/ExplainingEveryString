using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal interface IMainCharacterInfoForCameraExtractor
    {
        Vector2 Position { get; }
        Vector2 CursorPosition { get; }
        Single Focused { get; }
    }
}
