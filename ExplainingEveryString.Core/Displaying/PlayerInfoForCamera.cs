using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        internal Vector2 FireDirection => player.FireDirection;
        internal Vector2 CurrentMoveSpeed => player.Position - player.OldPosition;
    }
}
