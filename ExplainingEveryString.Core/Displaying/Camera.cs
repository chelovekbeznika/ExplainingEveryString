using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Displaying
{
    internal class Camera
    {
        private Dictionary<String, Texture2D> spritesStorage;
        private SpriteBatch spriteBatch;       
        private Level level;
        private Single screenHeight;
        private CameraObjectGlass objectGlass;

        internal Vector2 PlayerPositionOnScreen => objectGlass.PlayerPositionOnScreen;

        internal Camera(Level level, GraphicsDevice graphicsDevice, Dictionary<String, Texture2D> spritesStorage,
            Single playerFramePercentageWidth, Single playerFramePercentageHeight)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.screenHeight = graphicsDevice.Viewport.Height;
            this.spritesStorage = spritesStorage;
            this.level = level;
            this.objectGlass =
                new CameraObjectGlass(level, graphicsDevice, playerFramePercentageWidth, playerFramePercentageHeight);
        }

        internal void Draw()
        {
            spriteBatch.Begin();
            foreach (IDisplayble objectToDraw in level.GetObjectsToDraw())
                Draw(objectToDraw);
            spriteBatch.End();
        }

        private void Draw(IDisplayble objectToDraw)
        {
            Texture2D sprite = spritesStorage[objectToDraw.CurrentSpriteName];
            Vector2 position = objectToDraw.Position;
            Vector2 drawPosition = GetDrawPosition(position, sprite);

            spriteBatch.Draw(sprite, drawPosition, Color.White);
        }

        private Vector2 GetDrawPosition(Vector2 position, Texture2D sprite)
        {
            Vector2 cameraOffset = objectGlass.CameraOffset;
            Vector2 centerOfSpriteOnScreen = position - cameraOffset;
            centerOfSpriteOnScreen.Y = screenHeight - centerOfSpriteOnScreen.Y;

            Vector2 leftUpperCornerPosition = new Vector2()
            {
                X = centerOfSpriteOnScreen.X - sprite.Width / 2,
                Y = centerOfSpriteOnScreen.Y - sprite.Height / 2
            };

            return leftUpperCornerPosition;
        }

        internal void Update()
        {
            objectGlass.Update();
        }
    }
}
