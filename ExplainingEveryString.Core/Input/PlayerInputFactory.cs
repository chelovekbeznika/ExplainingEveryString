using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;

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
            Vector2 playerScreenPosition() => gameplayComponent.Camera?.PlayerPositionOnScreen 
                ?? new Vector2(Constants.TargetWidth / 2, Constants.TargetHeight / 2);
            var gamePad = new GamePadPlayerInput(playerScreenPosition, config.TimeToFocusOnGamepad, 
                config.BetweenPlayerAndCursor, config.GamepadCameraSpeed);
            var keyboard = new KeyBoardMousePlayerInput(playerScreenPosition, config.TimeToFocusOnKeyboard);
            return new CompositePlayerInput(keyboard, gamePad);
        }
    }
}
