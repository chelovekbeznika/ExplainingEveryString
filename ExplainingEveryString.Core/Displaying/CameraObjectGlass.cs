using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class CameraObjectGlass
    {
        private readonly Vector2 playerFrame;
        private Level level;
        private Vector2 screenHalf;
        private Single screenHeight;
        private Vector2 cameraCenter;
        private Vector2 playerPositionOnScreen { get; }
      
        internal Vector2 CameraOffset => cameraCenter - screenHalf;
        internal Vector2 PlayerPositionOnScreen
        {
            get
            {
                Vector2 result = level.PlayerPosition - cameraCenter + screenHalf;
                result.Y = screenHeight - PlayerPositionOnScreen.Y;
                return result;
            }
        }

        internal CameraObjectGlass(Level level, GraphicsDevice graphicsDevice, 
            Single playerFramePercentageWidth, Single playerFramePercentageHeight)
        {
            Viewport viewport = graphicsDevice.Viewport;
            this.screenHalf = new Vector2 { X = viewport.Width / 2, Y = viewport.Height / 2 };
            this.screenHeight = viewport.Height;
            this.level = level;
            this.cameraCenter = level.PlayerPosition;
            this.playerFrame = new Vector2()
            {
                X = screenHalf.X * playerFramePercentageWidth / 100,
                Y = screenHalf.Y * playerFramePercentageHeight / 100
            };
        }

        internal void Update()
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
