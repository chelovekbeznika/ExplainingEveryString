using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal interface IUpdatable
    {
        void Update(Single elapsedSeconds);
        Boolean IsAlive();
    }
}
