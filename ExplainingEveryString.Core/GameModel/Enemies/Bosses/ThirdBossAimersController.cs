using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class ThirdBossAimersController : IUpdateable
    {
        private ThirdBossAimer[] aimers;
        private Int32 simultaneouslyFiring;
        private Single interval;
        private Single tillNextChange;

        internal ThirdBossAimersController(ThirdBossAimersSpecification specification, ThirdBossAimer[] aimers)
        {
            this.aimers = aimers;
            this.simultaneouslyFiring = specification.SimultaneouslyFiring;
            this.interval = specification.ChangeInterval;
            this.tillNextChange = specification.ChangeInterval;
        }

        public void Update(Single elapsedSeconds)
        {
            tillNextChange -= elapsedSeconds;
            if (tillNextChange <= 0)
            {
                var firingNow = RandomUtility.IntsFromRange(simultaneouslyFiring, aimers.Length);
                foreach (var aimerIndex in Enumerable.Range(0, aimers.Length))
                {
                    aimers[aimerIndex].FireSwitch = firingNow.Contains(aimerIndex);
                }
                tillNextChange += interval;
            }
        }
    }
}
