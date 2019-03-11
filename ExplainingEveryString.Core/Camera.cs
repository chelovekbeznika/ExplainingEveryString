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
        private Player player;
        private Vector2 screenHalf;

        private Vector2 CameraCenter { get => player.Position; }

        internal Camera(Player player, GraphicsDevice graphicsDevice, Dictionary<String, Texture2D> spritesStorage)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.screenHalf = new Vector2
            {
                X = spriteBatch.GraphicsDevice.Viewport.Width / 2,
                Y = spriteBatch.GraphicsDevice.Viewport.Height / 2
            };
            this.player = player;
            this.spritesStorage = spritesStorage;
        }

        internal void Begin() => spriteBatch.Begin();

        internal void End() => spriteBatch.End();

        internal void Draw(GameObject gameObject)
        {
            Texture2D sprite = spritesStorage[gameObject.SpriteName];

            Vector2 position = gameObject.Position;
            Vector2 worldPosition = new Vector2() { X = position.X - sprite.Width / 2, Y = position.Y - sprite.Height / 2 };
            Vector2 cameraOffset = CameraCenter - screenHalf;
            Vector2 drawPosition = worldPosition - cameraOffset;
            spriteBatch.Draw(sprite, drawPosition, Color.White);
        }
    }
}
