using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class RectangleTests
    {
        [Test]
        public void Test()
        {
            Rectangle rectangle = new Rectangle(100, 100, 150, 150);
            Assert.That(rectangle.Top == 100);
            Assert.That(rectangle.Bottom == 250);
        }
    }
}
