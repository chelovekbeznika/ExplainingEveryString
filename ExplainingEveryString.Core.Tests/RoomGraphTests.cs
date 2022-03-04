using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using ExplainingEveryString.Core.GameModel.Movement.MovingByNavigation;

namespace ExplainingEveryString.Core.Tests
{
    internal class RoomGraphTests
    {
        private const Single FarAway = 1_000_000_000;

        [Test]
        public void BuildPathsTest()
        {
            var vertices = new Vector2[] 
            { 
                new Vector2(0, 0), 
                new Vector2(1, 1), 
                new Vector2(2, 2), 
                new Vector2(3, 3), 
                new Vector2(4, 4) 
            };
            var edges = new Single[,]
            {
                { 0, 5, FarAway, FarAway, FarAway },
                { 5, 0, 6, 3, 8 },
                { FarAway, 6, 0, 2, FarAway },
                { FarAway, 3, 2, 0, FarAway },
                { FarAway, 8, FarAway, FarAway, 0 }
            };
            var roomGraph = new RoomPointsGraph(vertices, edges);
            roomGraph.BuildPaths();

            var expected_0_4 = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 1), new Vector2(4, 4) };
            var actual_0_4 = roomGraph.GetWayInGraph(0, 4);
            CollectionAssert.AreEqual(expected_0_4, actual_0_4);

            var expected_4_2 = new List<Vector2>() { new Vector2(4, 4), new Vector2(1, 1), new Vector2(3, 3), new Vector2(2, 2) };
            var actual_4_2 = roomGraph.GetWayInGraph(4, 2);
            CollectionAssert.AreEqual(expected_4_2, actual_4_2);
        }
    }
}
