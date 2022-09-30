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

        public Vector2 GetFireDirection() => current.GetFireDirection();

        public Vector2 GetMoveDirection() => current.GetMoveDirection();

        public Boolean IsFiring() => current.IsFiring();

        public Boolean IsTryingToDash() => current.IsTryingToDash();

        public Boolean IsTryingToReload() => current.IsTryingToReload();

        public Int32 WeaponSwitchMeasure() => current.WeaponSwitchMeasure();

        public string DirectlySelectedWeapon() => current.DirectlySelectedWeapon();

        public void Update(Single elapsedSeconds)
        {
            var preferredDevice = ConfigurationAccess.GetCurrentConfig().Input.PreferredControlDevice;
            if (preferredDevice == ControlDevice.GamePad && !GamepadAccessible)
                preferredDevice = ControlDevice.Keyboard;
            switch (preferredDevice)
            {
                case ControlDevice.GamePad: current = gamePad; break;
                case ControlDevice.Keyboard: current = keyboard; break;
                default: throw new Exception("Badly configured input");
            }

            current.Update(elapsedSeconds);
        }

        private Boolean GamepadAccessible => GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
    }
}
