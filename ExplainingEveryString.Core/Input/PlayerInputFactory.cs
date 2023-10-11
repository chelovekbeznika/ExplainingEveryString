using ExplainingEveryString.Core.GameModel;
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
            Vector2 playerScreenPosition() => gameplayComponent.Camera.PlayerPositionOnScreen;
            var gamePad = new GamePadPlayerInput(playerScreenPosition, config.TimeToFocusOnGamepad, config.BetweenPlayerAndCursor);
            var keyboard = new KeyBoardMousePlayerInput(playerScreenPosition, config.TimeToFocusOnKeyboard);
            return new CompositePlayerInput(keyboard, gamePad);
        }
    }
}
