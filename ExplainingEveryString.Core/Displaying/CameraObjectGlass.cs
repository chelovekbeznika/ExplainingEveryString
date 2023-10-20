using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class CameraObjectGlass : ILevelCoordinatesMaster
    {
        private readonly IMainCharacterInfoForCameraExtractor playerInfo;
        private readonly Vector2 screenHalf = new Vector2 { X = Constants.TargetWidth / 2, Y = Constants.TargetHeight / 2 };
        private Rectangle playerFrame;
        private Vector2 cameraCenter;

        public Vector2 CameraOffset => cameraCenter - screenHalf;

        public Rectangle ScreenCovers => new Rectangle
        {
            X = (Int32)CameraOffset.X,
            Y = (Int32)CameraOffset.Y,
            Width = Constants.TargetWidth,
            Height = Constants.TargetHeight
        };
        public Vector2 PlayerPosition => playerInfo.Position;

        internal CameraObjectGlass(IMainCharacterInfoForCameraExtractor playerInfo, CameraConfiguration config)
        {
            this.playerInfo = playerInfo;
            this.cameraCenter = playerInfo.Position;
            playerFrame = new Rectangle(
                x: Constants.TargetWidth * (100 - config.PlayerFramePercentageWidth) / 2 / 100, 
                y: Constants.TargetHeight * (100 - config.PlayerFramePercentageHeight) / 2 / 100, 
                width: Constants.TargetWidth * config.PlayerFramePercentageWidth / 100, 
                height: Constants.TargetHeight * config.PlayerFramePercentageHeight / 100);
        }

        public void Update(Single elapsedSeconds)
        {
            var cursorPosition = playerInfo.CursorPosition;
            if (cursorPosition.Y < playerFrame.Top)
                cursorPosition.Y = playerFrame.Top;
            if (cursorPosition.Y > playerFrame.Bottom)
                cursorPosition.Y = playerFrame.Bottom;
            if (cursorPosition.X < playerFrame.Left)
                cursorPosition.X = playerFrame.Left;
            if (cursorPosition.X > playerFrame.Right)
                cursorPosition.X = playerFrame.Right;
            var cursorOffset = cursorPosition - screenHalf;
            cursorOffset.Y *= -1;
            cameraCenter = playerInfo.Position + cursorOffset * playerInfo.Focused;
        }
    }
}
