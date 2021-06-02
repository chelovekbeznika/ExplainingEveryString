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
            var gamePad = new GamePadPlayerInput(config.TimeToFocusOnGamepad);
            Func<Vector2> playerScreenPosition = () => gameplayComponent.Camera.PlayerPositionOnScreen;
            var keyboard = new KeyBoardMousePlayerInput(playerScreenPosition, config.TimeToFocusOnKeyboard);
            return new CompositePlayerInput(keyboard, gamePad);
        }
    }
}
