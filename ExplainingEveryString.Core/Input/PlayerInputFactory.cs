using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class PlayerInputFactory
    {
        private GameplayComponent gameplayComponent;

        internal PlayerInputFactory(GameplayComponent gameplayComponent)
        {
            this.gameplayComponent = gameplayComponent;
        }

        internal IPlayerInput Create()
        {
            var config = ConfigurationAccess.GetCurrentConfig().Input;
            switch (config.ControlDevice)
            {
                case ControlDevice.GamePad: 
                    return new GamePadPlayerInput(config.TimeToFocusOnGamepad);
                case ControlDevice.Keyboard:
                    Func<Vector2> playerScreenPosition = () => gameplayComponent.Camera.PlayerPositionOnScreen;
                    return new KeyBoardMousePlayerInput(playerScreenPosition, config.TimeToFocusOnKeyboard);
                default: 
                    throw new Exception("Badly configured input");
            }
        }
    }
}
