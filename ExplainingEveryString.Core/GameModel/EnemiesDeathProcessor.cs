using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal static class EnemiesDeathProcessor
    {
        internal static List<IEnemy> DivideAliveAndDead(List<IEnemy> enemies, List<IEnemy> avengers)
        {
            if (enemies == null)
                return null;
            var newAvengers = new List<IEnemy>();
            foreach (var dead in enemies.Where(e => !e.IsAlive()))
            {
                dead.ProcessDeath();
                if (dead.Avengers != null)
                    newAvengers.AddRange(dead.Avengers);
            }
            avengers.AddRange(newAvengers);
            return enemies.Where(e => e.IsAlive()).ToList();
        }
    }
}
