using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class CompositePlayerInput : IPlayerInput
    {
        private readonly KeyBoardMousePlayerInput keyboard;
        private readonly GamePadPlayerInput gamePad;

        private IPlayerInput current;

        internal CompositePlayerInput(KeyBoardMousePlayerInput keyboard, GamePadPlayerInput gamePad)
        {
            this.keyboard = keyboard;
            this.gamePad = gamePad;
            Update(0);
        }

        public Single Focus => current.Focus;

        public Vector2 GetFireDirection(Vector2 currentMuzzlePosition) => current.GetFireDirection(currentMuzzlePosition);

        public Vector2 GetMoveDirection() => current.GetMoveDirection();

        public Vector2 GetCursorPosition() => current.GetCursorPosition();

        public Boolean IsFiring() => current.IsFiring();

        public Boolean IsTryingToDash() => current.IsTryingToDash();

        public Boolean IsTryingToReload() => current.IsTryingToReload();

        public Int32 WeaponSwitchMeasure() => current.WeaponSwitchMeasure();

        public string DirectlySelectedWeapon() => current.DirectlySelectedWeapon();

        public void Update(Single elapsedSeconds)
        {
            var inputDevice = ConfigurationAccess.GetCurrentConfig().Input.PreferredControlDevice;
            if (inputDevice == ControlDevice.GamePad && !GamepadAccessible)
                inputDevice = ControlDevice.Keyboard;
            current = inputDevice switch
            {
                ControlDevice.GamePad => gamePad,
                ControlDevice.Keyboard => keyboard,
                _ => throw new Exception("Badly configured input"),
            };
            current.Update(elapsedSeconds);
        }

        private Boolean GamepadAccessible => GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
    }
}
