using ExplainingEveryString.Core.Music;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tests.Music
{
    [TestFixture]
    public class FrameCounterTests
    {
        [Test]
        public void NineteenThousandsCyclesModeSet()
        {
            Int32 quarterFrames = 0;
            Int32 halfFrames = 0;
            FrameCounter frameCounter = new FrameCounter { ModeFlag = true };
            frameCounter.QuarterFrame += (sender, e) => quarterFrames += 1;
            frameCounter.HalfFrame += (sender, e) => halfFrames += 1;
            foreach (Int32 sampleNumber in Enumerable.Range(0, 1000))
            {
                frameCounter.MoveEmulationForward();
            }
            Assert.AreEqual(4, quarterFrames);
            Assert.AreEqual(2, halfFrames);
        }

        [Test]
        public void NineteenThousandsCyclesModeClear()
        {
            Int32 quarterFrames = 0;
            Int32 halfFrames = 0;
            FrameCounter frameCounter = new FrameCounter { ModeFlag = false };
            frameCounter.QuarterFrame += (sender, e) => quarterFrames += 1;
            frameCounter.HalfFrame += (sender, e) => halfFrames += 1;
            foreach (Int32 sampleNumber in Enumerable.Range(0, 1000))
            {
                frameCounter.MoveEmulationForward();
            }
            Assert.AreEqual(5, quarterFrames);
            Assert.AreEqual(2, halfFrames);
        }
    }
}
