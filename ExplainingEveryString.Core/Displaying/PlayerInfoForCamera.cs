using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class PlayerInfoForCameraExtractor : IMainCharacterInfoForCameraExtractor
    {
        private Player player;

        internal PlayerInfoForCameraExtractor(Level level)
        {
            this.player = level.Player;
        }

        public Vector2 Position => player.Position;
        public Vector2 FireDirection => player.Input.GetFireDirection(Position);
        public Vector2 CursorPosition => player.Input.GetCursorPosition();
        public Single Focused => player.Input.Focus;
    }
}
