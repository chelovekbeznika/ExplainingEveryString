﻿using ExplainingEveryString.Core.GameModel;
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
            var desiredCenter = CalculateDesiredCenter();
            var maxFrameCameraMove = cameraMoveSpeed * elapsedSeconds;
            var cameraMoveDirection = desiredCenter - cameraCenter;
            if (cameraMoveDirection.Length() < maxFrameCameraMove)
                cameraCenter = desiredCenter;
            else
                cameraCenter += cameraMoveDirection / cameraMoveDirection.Length() * maxFrameCameraMove;
            cameraCenter = AdjustToPlayerFrame(cameraCenter);
        }

        private Vector2 CalculateDesiredCenter()
        {
            var screenFrameArrow = -playerInfo.FireDirection;
            var targetXPosition = screenFrameArrow.X < 0 ? -playerFrame.X : playerFrame.X;
            var targetYPosition = screenFrameArrow.Y < 0 ? -playerFrame.Y : playerFrame.Y;
            var distanceToFrameX = targetYPosition / screenFrameArrow.Y;
            var distanceToFrameY = targetXPosition / screenFrameArrow.X;
            var desiredPlayerPositionRelativeToCenter =
                (!Single.IsInfinity(distanceToFrameX) && distanceToFrameX < distanceToFrameY) || Single.IsInfinity(distanceToFrameY)
                    ? screenFrameArrow * distanceToFrameX
                    : screenFrameArrow * distanceToFrameY;
            return playerInfo.Position - desiredPlayerPositionRelativeToCenter;
        }

        private Vector2 AdjustToPlayerFrame(Vector2 cameraCenter)
        {
            if (cameraCenter.X < playerInfo.Position.X - playerFrame.X)
                cameraCenter.X = playerInfo.Position.X - playerFrame.X;
            if (cameraCenter.X > playerInfo.Position.X + playerFrame.X)
                cameraCenter.X = playerInfo.Position.X + playerFrame.X;
            if (cameraCenter.Y < playerInfo.Position.Y - playerFrame.Y)
                cameraCenter.Y = playerInfo.Position.Y - playerFrame.Y;
            if (cameraCenter.Y > playerInfo.Position.Y + playerFrame.Y)
                cameraCenter.Y = playerInfo.Position.Y + playerFrame.Y;
            return cameraCenter;
        }

        private Single CalculateCameraMoveSpeed(Single timeFromAngleToAngle)
        {
            var distanceFromAngleToAngle = playerFrame.Length() * 2;
            return distanceFromAngleToAngle / timeFromAngleToAngle;
        }
    }
}
