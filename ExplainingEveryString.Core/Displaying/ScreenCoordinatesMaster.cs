using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class ScreenCoordinatesMaster : IScreenCoordinatesMaster
    {
        private readonly Viewport viewport;
        private readonly ILevelCoordinatesMaster levelCoordinatesMaster;

        public Vector2 PlayerPosition => ConvertToScreenPosition(levelCoordinatesMaster.PlayerPosition);

        public Rectangle ScreenCovers => viewport.Bounds;

        public ScreenCoordinatesMaster(Viewport viewport, ILevelCoordinatesMaster levelCoordinatesMaster)
        {
            this.viewport = viewport;
            this.levelCoordinatesMaster = levelCoordinatesMaster;
        }

        public Rectangle PositionOnScreen(Vector2 position, SpriteData sprite)
        {
            var visibleSize = new Point
            {
                X = sprite.Width,
                Y = sprite.Height
            };
            var centerOnScreen = ConvertToScreenPosition(position);
            return new Rectangle
            {
                X = (Int32)(centerOnScreen.X - visibleSize.X / 2),
                Y = (Int32)(centerOnScreen.Y - visibleSize.Y / 2),
                Width = visibleSize.X,
                Height = visibleSize.Y
            };
        }


        public Boolean IsVisibleOnScreen(Vector2 position, SpriteData sprite)
        {
            var displaybleOnScreen = PositionOnScreen(position, sprite);
            var screen = viewport.Bounds;
            return screen.Intersects(displaybleOnScreen);
        }

        public Vector2 ConvertToScreenPosition(Vector2 position)
        {
            var cameraOffset = levelCoordinatesMaster.CameraOffset;
            var centerOfSpriteOnScreen = position - cameraOffset;
            centerOfSpriteOnScreen.X = (Int32)centerOfSpriteOnScreen.X;
            centerOfSpriteOnScreen.Y = (Int32)(viewport.Height - centerOfSpriteOnScreen.Y);
            return centerOfSpriteOnScreen;
        }

        public Vector2 ConvertToLevelPosition(Vector2 position)
        {
            position.Y = viewport.Height - position.Y;
            var cameraOffset = levelCoordinatesMaster.CameraOffset;
            var centerOfSpriteOnLevel = cameraOffset + position;
            return centerOfSpriteOnLevel;
        }

        public void Update(Single elapsedSeconds)
        {
            levelCoordinatesMaster.Update(elapsedSeconds);
        }
    }
}
