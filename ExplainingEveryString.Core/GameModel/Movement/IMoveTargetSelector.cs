using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    internal interface IMoveTargetSelector
    {
        Vector2 GetTarget();
        void SwitchToNextTarget();
    }
}
