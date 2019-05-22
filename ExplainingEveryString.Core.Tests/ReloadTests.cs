using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tests
{
    public class ReloadTests
    {
        private protected TestAimer aimer = new TestAimer();
        protected WeaponSpecification specification = new WeaponSpecification()
        {
            Barrels = new BarrelSpecification[]
            {
                new BarrelSpecification
                {
                    Bullet = new BulletSpecification
                    {
                        TrajectoryParameters = new Dictionary<String, Single> { { "speed", 100 } },
                        Sprite = new SpriteSpecification { Name = "FooBar" },
                        TimeToLive = 50,
                        Damage = 1
                    },
                    Length = 14,
                    AngleCorrection = 0
                }
            },
            FireRate = 1,
            Ammo = 1,
            ReloadTime = 0
        };
        protected Int32 shots = 0;
        protected List<Single> bulletUpdateTimes = new List<Single>();
        private protected WeaponReloader reloader;

        [SetUp]
        public virtual void SetUp()
        {
            shots = 0;
            bulletUpdateTimes = new List<Single>();
            reloader = new WeaponReloader(specification, aimer, (fut) =>
            {
                shots += 1;
                bulletUpdateTimes.Add(fut);
            });
        }
    }
}
