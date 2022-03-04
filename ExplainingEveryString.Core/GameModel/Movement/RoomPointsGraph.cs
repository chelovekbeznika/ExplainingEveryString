using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExplainingEveryString.Core.Collisions;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    internal class RoomPointsGraph
    {
        private const Single FarAway = 1_000_000_000;

        private CollisionsController collisionController;
        private Vector2[] vertices;
        private Single[,] edges;
        private Int32[,] next;

        private const Int32 EnemyIndex = 0;
        private Int32 PlayerIndex => VerticesAmount - 1;
        
        public String CollideTag { get; private set; }

        private Int32 VerticesAmount => vertices.Length;

        internal RoomPointsGraph(CollisionsController collisionsController, Vector2[] vertices, Single[,] edges, String collideTag)
        {
            this.collisionController = collisionsController;
            this.vertices = vertices;
            this.edges = edges;
            this.CollideTag = collideTag;
        }

        internal List<Vector2> GetWayInLevel(Vector2 enemyPosition, Vector2 playerPosition)
        {
            RebuildPlayerAndEnemyPositionsInGraph(enemyPosition, playerPosition);
            return GetWayInGraph(EnemyIndex, PlayerIndex);
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
            this.next = new Int32[VerticesAmount, VerticesAmount];
            var distances = new Single[VerticesAmount, VerticesAmount];
            foreach (var a in Enumerable.Range(0, VerticesAmount))
                foreach (var b in Enumerable.Range(0, VerticesAmount))
                {
                    distances[a, b] = a != b ? edges[a, b] : 0;
                    next[a, b] = distances[a, b] < FarAway ? b : 0;
                }

            foreach (var interPoint in Enumerable.Range(0, VerticesAmount))
                foreach (var a in Enumerable.Range(0, VerticesAmount))
                    foreach (var b in Enumerable.Range(0, VerticesAmount))
                    {
                        if (distances[a, interPoint] + distances[interPoint, b] < distances[a, b])
                        {
                            distances[a, b] = distances[a, interPoint] + distances[interPoint, b];
                            next[a, b] = next[a, interPoint];
                        }
                    }
        }

        private void RebuildPlayerAndEnemyPositionsInGraph(Vector2 enemyPosition, Vector2 playerPosition)
        {
            vertices[EnemyIndex] = enemyPosition;
            vertices[PlayerIndex] = playerPosition;

            foreach (var index in Enumerable.Range(0, VerticesAmount))
            {
                InitDistance(collisionController, vertices, edges, EnemyIndex, index, CollideTag);
                InitDistance(collisionController, vertices, edges, index, EnemyIndex, CollideTag);
                InitDistance(collisionController, vertices, edges, PlayerIndex, index, CollideTag);
                InitDistance(collisionController, vertices, edges, index, PlayerIndex, CollideTag);
            }
        }

        internal static RoomPointsGraph BuildRoomGraph(CollisionsController collisionsController, LevelData levelData, 
            String roomName, TileWrapper tileWrapper, String collideTag)
        {
            var room = levelData.Waypoints[roomName];
            var vertices = new Vector2[] { Vector2.Zero }
                .Concat(room.Waypoints.Select(w => tileWrapper.GetLevelPosition(w)))
                .Concat(new Vector2[] { Vector2.Zero }).ToArray();
            var waypointsAmount = room.Waypoints.Count;
            var edges = new Single[waypointsAmount + 2, waypointsAmount + 2];

            foreach (var row in Enumerable.Range(1, waypointsAmount - 1))
                foreach (var col in Enumerable.Range(1, waypointsAmount - 1))
                    InitDistance(collisionsController, vertices, edges, row, col, collideTag);

            return new RoomPointsGraph(collisionsController, vertices, edges, collideTag);
        }

        private static void InitDistance(CollisionsController collisionsController, Vector2[] vertices, 
            Single[,] edges, Int32 row, Int32 col, String collideTag)
        {
            if (row != col)
            {
                var wayBetweenPoints = collisionsController.IsItPossibleToRide(vertices[row], vertices[col], collideTag);
                edges[row, col] = wayBetweenPoints ? Vector2.Distance(vertices[row], vertices[col]) : FarAway;
            }
        }
    }
}
