﻿using ExplainingEveryString.Core.GameModel;
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
            foreach (GameObject go in level.GameObjects)
                Draw(go);
            spriteBatch.End();
        }

        private void Draw(GameObject gameObject)
        {
            Texture2D sprite = spritesStorage[gameObject.SpriteName];
            Vector2 position = gameObject.Position;
            Vector2 worldPosition = new Vector2() { X = position.X - sprite.Width / 2, Y = position.Y - sprite.Height / 2 };

            Vector2 drawPosition = GetDrawPosition(worldPosition);
            spriteBatch.Draw(sprite, drawPosition, Color.White);
        }

        private Vector2 GetDrawPosition(Vector2 worldPosition)
        {
            RecalculateCameraCenter();
            Vector2 cameraOffset = cameraCenter - screenHalf;
            Vector2 drawPosition = worldPosition - cameraOffset;
            Single screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
            drawPosition.Y = screenHeight - drawPosition.Y;
            return drawPosition;
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