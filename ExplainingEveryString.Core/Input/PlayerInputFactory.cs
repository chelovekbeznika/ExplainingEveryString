using ExplainingEveryString.Data;

namespace ExplainingEveryString.Core.Input
{
    internal static class PlayerInputFactory
    {
        internal static IPlayerInput Create()
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            switch (config.ControlDevice)
            {
                case ControlDevice.GamePad: return new GamePadPlayerInput();
                case ControlDevice.Keyboard: return new KeyBoardMousePlayerInput();
                default: throw new System.Exception("Badly configured input");
            }
        }
    }
}
