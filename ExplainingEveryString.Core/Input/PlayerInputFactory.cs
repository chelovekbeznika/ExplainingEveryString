using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExplainingEveryString.Core.Input
{
    internal static class PlayerInputFactory
    {
        internal static IPlayerInput Create()
        {
            return new GamePadPlayerInput();
        }
    }
}
