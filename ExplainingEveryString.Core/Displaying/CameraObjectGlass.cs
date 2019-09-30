using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class CameraObjectGlass : ILevelCoordinatesMaster
    {
        private readonly Vector2 playerFrame;
        private readonly Single cameraMoveSpeed;
        private readonly Single screenHeight;
        private readonly Viewport viewport;

        private PlayerInfoForCameraExtractor playerInfo;
        private Vector2 screenHalf;
        private Vector2 cameraCenter;
      
        public Vector2 CameraOffset => cameraCenter - screenHalf;
        public Rectangle ScreenCovers => new Rectangle
        {
            X = (Int32)CameraOffset.X,
            Y = (Int32)CameraOffset.Y,
            Width = viewport.Width,
            Height = viewport.Height
        };
        public Vector2 PlayerPosition => playerInfo.Position;

        internal CameraObjectGlass(Level level, Viewport viewport, CameraConfiguration config)
        {
            this.viewport = viewport;
            this.screenHalf = new Vector2 { X = viewport.Width / 2, Y = viewport.Height / 2 };
            this.screenHeight = viewport.Height;
            this.playerInfo = new PlayerInfoForCameraExtractor(level);
            this.cameraCenter = playerInfo.Position;
            this.playerFrame = new Vector2()
            {
                X = screenHalf.X * config.PlayerFramePercentageWidth / 100,
                Y = screenHalf.Y * config.PlayerFramePercentageHeight / 100
            };
            this.cameraMoveSpeed = CalculateCameraMoveSpeed(config.CameraMoveTimeFromAngleToAngle);
        }

        public void Update(Single elapsedSeconds)
        {
            Vector2 desiredCenter = CalculateDesiredCenter();
            Single maxFrameCameraMove = cameraMoveSpeed * elapsedSeconds;
            if ((desiredCenter - cameraCenter).Length() < maxFrameCameraMove)
                cameraCenter = desiredCenter;
            else
            {
                Vector2 normalizedCameraMoveDirection = (desiredCenter - cameraCenter);
                normalizedCameraMoveDirection.Normalize();
                cameraCenter += normalizedCameraMoveDirection * maxFrameCameraMove + 0.5F * playerInfo.CurrentMoveSpeed;
            }
        }

        private Vector2 CalculateDesiredCenter()
        {
            Vector2 screenFrameArrow = -playerInfo.FireDirection;
            Single targetXPosition = screenFrameArrow.X < 0 ? -playerFrame.X : playerFrame.X;
            Single targetYPosition = screenFrameArrow.Y < 0 ? -playerFrame.Y : playerFrame.Y;
            Single distanceToFrameX = targetYPosition / screenFrameArrow.Y;
            Single distanceToFrameY = targetXPosition / screenFrameArrow.X;
            Vector2 desiredPlayerPositionRelativeToCenter =
                (!Single.IsInfinity(distanceToFrameX) && distanceToFrameX < distanceToFrameY) || Single.IsInfinity(distanceToFrameY)
                    ? screenFrameArrow * distanceToFrameX
                    : screenFrameArrow * distanceToFrameY;
            return playerInfo.Position - desiredPlayerPositionRelativeToCenter;
        }

        private Single CalculateCameraMoveSpeed(Single timeFromAngleToAngle)
        {
            Single distanceFromAngleToAngle = playerFrame.Length() * 2;
            return distanceFromAngleToAngle / timeFromAngleToAngle;
        }
    }
}
