using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class PostMortemSurprise
    {
        private Barrel[] barrels;
        private Boolean triggered = false;

        internal PostMortemSurprise(PostMortemSurpriseSpecification specification, 
            Func<Vector2> currentPositionLocator, Func<Vector2> playerLocator, Level level)
        {
            IAimer aimer = AimersFactory.Get(specification.AimType, 0, currentPositionLocator, playerLocator);
            barrels = specification.Barrels
                .Select(bs => new Barrel(aimer, currentPositionLocator, playerLocator, bs)).ToArray();
            foreach (Barrel barrel in barrels)
                barrel.Shoot += level.EnemyShoot;
        }

        internal void TryTrigger()
        {
            if (!triggered)
            {
                Trigger();
                triggered = true;
            }
        }

        internal void Cancel()
        {
            triggered = true;
        }

        private void Trigger()
        {
            if (barrels != null)
            {
                foreach (Barrel barrel in barrels)
                    barrel.OnShoot(0);
            }
        }
    }
}
