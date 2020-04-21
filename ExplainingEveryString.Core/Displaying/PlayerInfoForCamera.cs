using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class PlayerInfoForCameraExtractor
    {
        private Player player;

        internal PlayerInfoForCameraExtractor(Level level)
        {
            this.player = level.Player;
        }

        internal Vector2 Position => player.Position;
        internal Vector2 FireDirection => player.Input.GetFireDirection();
        internal Vector2 CurrentMoveSpeed => player.Position - player.OldPosition;
        internal Single Focused => player.Input.Focus;
    }
}
