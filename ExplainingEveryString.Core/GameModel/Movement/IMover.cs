using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    interface IMover
    {
        Vector2 GetPositionChange(Vector2 lineToTarget, Single elapsedSeconds, out Boolean goalReached);
    }
}
