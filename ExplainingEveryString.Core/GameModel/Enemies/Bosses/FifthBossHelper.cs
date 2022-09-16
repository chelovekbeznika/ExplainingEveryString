using ExplainingEveryString.Data.Blueprints;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FifthBossHelper : Enemy<EnemyBlueprint>
    {
        internal FifthBoss Boss { get; set; }

        public override Single HitPoints { 
            get => Boss?.HitPoints ?? Single.MaxValue; 
            set
            {
                if (Boss != null) 
                    Boss.HitPoints = value; 
            } 
        }

        public override Boolean IsAlive() => Boss.IsAlive();
    }
}
