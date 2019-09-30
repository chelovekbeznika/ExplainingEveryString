using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal static class EnemyDeathProcessor
    {
        internal static List<IEnemy> SendDeadToHeaven(List<IEnemy> enemies, List<IEnemy> avengers)
        {
            List<IEnemy> newAvengers = new List<IEnemy>();
            foreach (IEnemy dead in enemies.Where(e => !e.IsAlive()))
            {
                if (dead.Avengers != null)
                    newAvengers.AddRange(dead.Avengers);
            }
            avengers.AddRange(newAvengers);
            return enemies.Where(e => e.IsAlive()).ToList();
        }
    }
}
