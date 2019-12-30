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
            var config = ConfigurationAccess.GetCurrentConfig();
            switch (config.ControlDevice)
            {
                case ControlDevice.GamePad: return new GamePadPlayerInput();
                case ControlDevice.Keyboard:
                    Func<Vector2> playerScreenPosition = () => gameplayComponent.Camera.PlayerPositionOnScreen;
                    return new KeyBoardMousePlayerInput(playerScreenPosition);
                default: throw new Exception("Badly configured input");
            }
        }
    }
}
