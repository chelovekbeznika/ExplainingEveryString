using ExplainingEveryString.Data.Blueprints;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Mine : Enemy<EnemyBlueprint>, ICrashable
    {
        public override void Update(Single elapsedSeconds)
        {
        }
    }
}
