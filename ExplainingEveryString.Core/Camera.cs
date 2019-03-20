using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core
{
    internal class Camera
    {
        private Dictionary<String, Texture2D> spritesStorage;
        private SpriteBatch spriteBatch;
        private Level level;
        private Vector2 screenHalf;
        private Vector2 cameraCenter;
        private readonly Vector2 playerFrame;

        internal Camera(Level level, GraphicsDevice graphicsDevice, Dictionary<String, Texture2D> spritesStorage,
            Single playerFramePercentageWidth, Single playerFramePercentageHeight)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
            this.screenHalf = new Vector2 { X = viewport.Width / 2, Y = viewport.Height / 2 };

            this.level = level;
            this.cameraCenter = level.PlayerPosition;
            this.spritesStorage = spritesStorage;
            this.playerFrame = new Vector2()
            {
                X = screenHalf.X * playerFramePercentageWidth / 100,
                Y = screenHalf.Y * playerFramePercentageHeight / 100
            };
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
            RecalculateCameraCenter();

            Vector2 cameraOffset = cameraCenter - screenHalf;
            Vector2 centerOfSpriteOnScreen = position - cameraOffset;
            Single screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
            centerOfSpriteOnScreen.Y = screenHeight - centerOfSpriteOnScreen.Y;

            Vector2 leftUpperCornerPosition = new Vector2()
            {
                X = centerOfSpriteOnScreen.X - sprite.Width / 2,
                Y = centerOfSpriteOnScreen.Y - sprite.Height / 2
            };

            return leftUpperCornerPosition;
        }

        private void RecalculateCameraCenter()
        {
            Vector2 currentDifference = level.PlayerPosition - cameraCenter;
            if (currentDifference.X > playerFrame.X)
                cameraCenter.X += currentDifference.X - playerFrame.X;
            if (currentDifference.X < -playerFrame.X)
                cameraCenter.X += currentDifference.X + playerFrame.X;
            if (currentDifference.Y > playerFrame.Y)
                cameraCenter.Y += currentDifference.Y - playerFrame.Y;
            if (currentDifference.Y < -playerFrame.Y)
                cameraCenter.Y += currentDifference.Y + playerFrame.Y;
        }
    }
}
