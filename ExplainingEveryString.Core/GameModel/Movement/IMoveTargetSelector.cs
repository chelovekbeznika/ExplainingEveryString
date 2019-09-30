using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    internal interface IMoveTargetSelector
    {
        Vector2 GetTarget();
        void SwitchToNextTarget();
    }
}
