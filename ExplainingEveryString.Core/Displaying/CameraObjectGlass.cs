using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class CameraObjectGlass : ILevelCoordinatesMaster
    {
        private readonly Vector2 playerWindowEllips;

        private IMainCharacterInfoForCameraExtractor playerInfo;
        private Vector2 ScreenHalf => new Vector2 { X = Constants.TargetWidth / 2, Y = Constants.TargetHeight / 2 };
        private Vector2 cameraCenter;
        private Single focusAngle;
        private Single desiredFocusAngle;
        private Single timeToReverseFocus;

        public Vector2 CameraOffset => cameraCenter - ScreenHalf;
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
            this.playerWindowEllips = new Vector2()
            {
                X = ScreenHalf.X * config.PlayerFramePercentageWidth / 100,
                Y = ScreenHalf.Y * config.PlayerFramePercentageHeight / 100
            };
            this.timeToReverseFocus = config.TimeToReverseFocusDirection;
        }

        public void Update(Single elapsedSeconds)
        {
            UpdateFocusAngle(elapsedSeconds);
            UpdateCameraCenter(elapsedSeconds);
        }

        private void UpdateCameraCenter(Single elapsedSeconds)
        {
            var focusOffset = AngleConverter.ToVector(focusAngle);
            focusOffset.X *= playerWindowEllips.X * playerInfo.Focused;
            focusOffset.Y *= playerWindowEllips.Y * playerInfo.Focused;
            cameraCenter = playerInfo.Position + focusOffset;
        }

        private void UpdateFocusAngle(Single elapsedSeconds)
        {
            desiredFocusAngle = AngleConverter.ToRadians(playerInfo.FireDirection);
            var maxAngleChange = (Single)System.Math.PI / timeToReverseFocus * elapsedSeconds;
            var arcToTarget = AngleConverter.ClosestArc(focusAngle, desiredFocusAngle);
            if (System.Math.Abs(arcToTarget) < maxAngleChange)
                focusAngle = desiredFocusAngle;
            else if (arcToTarget > 0)
                focusAngle += maxAngleChange;
            else
                focusAngle -= maxAngleChange;
        }
    }
}
