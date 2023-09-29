using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Specifications;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Tests
{
    public class ReloadTests
    {
        private protected TestAimer aimer = new TestAimer();
        protected ReloaderSpecification specification = new ReloaderSpecification()
        {
            FireRate = 1,
            Ammo = 1,
            ReloadTime = 0
        };
        protected Int32 shots = 0;
        protected Int32 reloadsFinished = 0;
        protected List<Single> bulletUpdateTimes = new List<Single>();
        private protected Reloader reloader;

        [SetUp]
        public virtual void SetUp()
        {
            shots = 0;
            reloadsFinished = 0;
            bulletUpdateTimes = new List<Single>();
            reloader = new Reloader(specification, () => aimer.IsFiring(), (fut) =>
            {
                shots += 1;
                bulletUpdateTimes.Add(fut);
            });
            reloader.ReloadFinished += (sender, e) => reloadsFinished += 1;
        }
    }
}
