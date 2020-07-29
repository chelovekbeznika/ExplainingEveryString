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
        private readonly Viewport viewport;

        private IMainCharacterInfoForCameraExtractor playerInfo;
        private Vector2 screenHalf;
        private Vector2 cameraCenter;
        private Single focusAngle;
        private Single desiredFocusAngle;
        private Single timeToReverseFocus;

        public Vector2 CameraOffset => cameraCenter - screenHalf;
        public Rectangle ScreenCovers => new Rectangle
        {
            X = (Int32)CameraOffset.X,
            Y = (Int32)CameraOffset.Y,
            Width = viewport.Width,
            Height = viewport.Height
        };
        public Vector2 PlayerPosition => playerInfo.Position;

        internal CameraObjectGlass(IMainCharacterInfoForCameraExtractor playerInfo, Viewport viewport, CameraConfiguration config)
        {
            this.viewport = viewport;
            this.screenHalf = new Vector2 { X = viewport.Width / 2, Y = viewport.Height / 2 };
            this.playerInfo = playerInfo;
            this.cameraCenter = playerInfo.Position;
            this.playerWindowEllips = new Vector2()
            {
                X = screenHalf.X * config.PlayerFramePercentageWidth / 100,
                Y = screenHalf.Y * config.PlayerFramePercentageHeight / 100
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
