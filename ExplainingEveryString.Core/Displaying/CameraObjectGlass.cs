using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class CameraObjectGlass : ILevelCoordinatesMaster
    {
        private readonly IMainCharacterInfoForCameraExtractor playerInfo;
        private readonly Vector2 screenHalf = new Vector2 { X = Constants.TargetWidth / 2, Y = Constants.TargetHeight / 2 };
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
        }

        public void Update(Single elapsedSeconds)
        {
            var cursorPosition = playerInfo.CursorPosition;
            if (cursorPosition.Y < 0)
                cursorPosition.Y = 0;
            if (cursorPosition.Y > Constants.TargetHeight)
                cursorPosition.Y = Constants.TargetHeight;
            if (cursorPosition.X < 0)
                cursorPosition.X = 0;
            if (cursorPosition.X > Constants.TargetWidth)
                cursorPosition.X = Constants.TargetWidth;
            var cursorOffset = cursorPosition - screenHalf;
            cursorOffset.Y *= -1;
            cameraCenter = playerInfo.Position + cursorOffset * playerInfo.Focused;
        }
    }
}
