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
            Point visibleSize = new Point
            {
                X = sprite.Width,
                Y = sprite.Height
            };
            Vector2 centerOnScreen = ConvertToScreenPosition(position);
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
            Rectangle displaybleOnScreen = PositionOnScreen(position, sprite);
            Rectangle screen = viewport.Bounds;
            return screen.Intersects(displaybleOnScreen);
        }

        public Vector2 ConvertToScreenPosition(Vector2 position)
        {
            Vector2 cameraOffset = levelCoordinatesMaster.CameraOffset;
            Vector2 centerOfSpriteOnScreen = position - cameraOffset;
            centerOfSpriteOnScreen.X = (Int32)centerOfSpriteOnScreen.X;
            centerOfSpriteOnScreen.Y = (Int32)(viewport.Height - centerOfSpriteOnScreen.Y);
            return centerOfSpriteOnScreen;
        }

        public void Update(Single elapsedSeconds)
        {
            levelCoordinatesMaster.Update(elapsedSeconds);
        }
    }
}
