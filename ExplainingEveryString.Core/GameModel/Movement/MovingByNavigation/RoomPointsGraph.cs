using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExplainingEveryString.Core.Collisions;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement.MovingByNavigation
{
    internal class RoomPointsGraph
    {
        private const Single FarAway = 1_000_000_000;
        private Vector2[] vertices;
        private Single[,] edges;
        private Int32[,] next;

        private Int32 WaypointsAmount => vertices.Length;

        internal RoomPointsGraph(Vector2[] vertices, Single[,] edges)
        {
            this.vertices = vertices;
            this.edges = edges;
        }

        internal List<Vector2> GetWayInLevel(Vector2 enemyPosition, Vector2 playerPosition)
        {
            var closestToEnemyDist = Vector2.Distance(enemyPosition, vertices[0]);
            var closestToEnemyPoint = 0;
            var closestToPlayerDist = Vector2.Distance(playerPosition, vertices[0]);
            var closestToPlayerPoint = 0;
            for (var index = 1; index <= vertices.Length; index += 1)
            {
                if (Vector2.Distance(playerPosition, vertices[index]) < closestToPlayerDist)
                {
                    closestToPlayerDist = Vector2.Distance(playerPosition, vertices[index]);
                    closestToPlayerPoint = index;
                }
                if (Vector2.Distance(enemyPosition, vertices[index]) < closestToEnemyDist)
                {
                    closestToEnemyDist = Vector2.Distance(enemyPosition, vertices[index]);
                    closestToEnemyPoint = index;
                }
            }

            return GetWayInGraph(closestToEnemyPoint, closestToPlayerPoint);
        }

        internal List<Vector2> GetWayInGraph(Int32 a, Int32 b)
        {
            if (next[a, b] == 0)
                return null;

            var result = new List<Vector2>() { vertices[a] };
            var current = a;
            while (current != b)
            {
                current = next[current, b];
                result.Add(vertices[current]);
            }
            return result;
        }

        internal void BuildPaths()
        {
            //Floyd–Warshall algorithm
            this.next = new Int32[WaypointsAmount, WaypointsAmount];
            var distances = new Single[WaypointsAmount, WaypointsAmount];
            foreach (var a in Enumerable.Range(0, WaypointsAmount))
                foreach (var b in Enumerable.Range(0, WaypointsAmount))
                {
                    distances[a, b] = a != b ? edges[a, b] : 0;
                    next[a, b] = distances[a, b] < FarAway ? b : 0;
                }

            foreach (var interPoint in Enumerable.Range(0, WaypointsAmount))
                foreach (var a in Enumerable.Range(0, WaypointsAmount))
                    foreach (var b in Enumerable.Range(0, WaypointsAmount))
                    {
                        if (distances[a, interPoint] + distances[interPoint, b] < distances[a, b])
                        {
                            distances[a, b] = distances[a, interPoint] + distances[interPoint, b];
                            next[a, b] = next[a, interPoint];
                        }
                    }
        }

        internal static RoomPointsGraph BuildRoomGraph(CollisionsController collisionsController, LevelData levelData, 
            String roomName, TileWrapper tileWrapper, String collideTag)
        {
            var room = levelData.Waypoints[roomName];
            var vertices = room.Waypoints.Select(w => tileWrapper.GetLevelPosition(w)).ToArray();
            var waypointsAmount = vertices.Length;
            var edges = new Single[waypointsAmount, waypointsAmount];

            foreach (var row in Enumerable.Range(0, waypointsAmount))
                foreach (var col in Enumerable.Range(0, waypointsAmount))
                    if (row != col)
                    {
                        var wayBetweenPoints = collisionsController.IsItPossibleToRide(vertices[row], vertices[col], collideTag);
                        edges[row, col] = wayBetweenPoints ? Vector2.Distance(vertices[row], vertices[col]) : FarAway;
                    }



            return new RoomPointsGraph(vertices, edges);
        }
    }
}
