using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal interface IScreenCoordinatesMaster : ICoordinatesMaster
    {
        Rectangle PositionOnScreen(Vector2 position, SpriteData spriteData);
        Boolean IsVisibleOnScreen(Vector2 position, SpriteData spriteData);
        Vector2 ConvertToScreenPosition(Vector2 position);
    }

    internal interface ILevelCoordinatesMaster : ICoordinatesMaster
    {
        Vector2 CameraOffset { get; }
    }

    internal interface ICoordinatesMaster : GameModel.IUpdateable
    {
        Rectangle ScreenCovers { get; }
        Vector2 PlayerPosition { get; }
    }
}
