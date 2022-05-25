using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class ThirdBossAimersController : IUpdateable
    {
        private ThirdBoss boss;
        private ThirdBossAimer[] aimers;
        private Int32[] simultaneouslyFiring;
        private Int32[] healthThresholds;
        private Int32 currentThreshold = 0;
        private Single interval;
        private Single tillNextChange;

        internal ThirdBossAimersController(ThirdBoss boss, ThirdBossAimersSpecification specification, ThirdBossAimer[] aimers)
        {
            this.boss = boss;
            this.aimers = aimers;
            this.simultaneouslyFiring = specification.SimultaneouslyFiring.ToArray();
            this.healthThresholds = specification.HealthThresholds.Append(Int32.MinValue).ToArray();
            this.interval = specification.ChangeInterval;
            this.tillNextChange = specification.ChangeInterval;

            SwitchFiringWeapons();
        }

        public void Update(Single elapsedSeconds)
        {
            tillNextChange -= elapsedSeconds;
            if (tillNextChange <= 0)
            {
                SwitchFiringWeapons();
                tillNextChange += interval;
            }
        }

        private void SwitchFiringWeapons()
        {
            while (boss.HitPoints <= healthThresholds[currentThreshold])
                currentThreshold += 1;
            var firingNow = RandomUtility.IntsFromRange(simultaneouslyFiring[currentThreshold], aimers.Length);
            foreach (var aimerIndex in Enumerable.Range(0, aimers.Length))
            {
                aimers[aimerIndex].FireSwitch = firingNow.Contains(aimerIndex);
            }
        }
    }
}
