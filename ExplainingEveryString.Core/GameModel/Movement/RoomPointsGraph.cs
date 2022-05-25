using System;
using System.Collections.Generic;
using System.Linq;
using ExplainingEveryString.Core.Collisions;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    internal class RoomPointsGraph
    {
        private const Single FarAway = 1_000_000_000;
        private const Int32 NoWay = -1;

        private CollisionsController collisionController;
        private Vector2[] vertices;
        private Single[,] edges;
        private Int32[] previousInPath;

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

        internal List<Vector2> GetWayInLevel(Vector2 enemyPosition, Hitbox enemyHitbox, Vector2 playerPosition)
        {
            RebuildPlayerAndEnemyPositionsInGraph(enemyPosition, enemyHitbox, playerPosition);
            BuildPaths();
            return GetWayInGraph();
        }

        internal List<Vector2> GetWayInGraph()
        {
            if (previousInPath[PlayerIndex] == NoWay)
                return null;

            var currentVertice = PlayerIndex;
            var result = new List<Vector2>();
            while (currentVertice != NoWay)
            {
                result.Add(vertices[currentVertice]);
                currentVertice = previousInPath[currentVertice];
            }
            result.Reverse();
            return result;
        }

        private void RebuildPlayerAndEnemyPositionsInGraph(Vector2 enemyPosition, Hitbox enemyHitbox, Vector2 playerPosition)
        {
            vertices[EnemyIndex] = enemyPosition;
            vertices[PlayerIndex] = playerPosition;

            foreach (var index in Enumerable.Range(0, VerticesAmount))
            {
                if (EnemyCanRideToPoint(enemyHitbox, vertices[index]))
                {
                    edges[index, EnemyIndex] = Vector2.Distance(vertices[index], vertices[EnemyIndex]);
                    edges[EnemyIndex, index] = Vector2.Distance(vertices[EnemyIndex], vertices[index]);
                }
                else
                {
                    edges[index, EnemyIndex] = FarAway;
                    edges[EnemyIndex, index] = FarAway;
                }
                InitDistance(collisionController, vertices, edges, PlayerIndex, index, CollideTag);
                InitDistance(collisionController, vertices, edges, index, PlayerIndex, CollideTag);
            }

            edges[EnemyIndex, PlayerIndex] = FarAway;
            edges[PlayerIndex, EnemyIndex] = FarAway;
        }

        private Boolean EnemyCanRideToPoint(Hitbox enemyHitbox, Vector2 point) =>
            collisionController.IsItPossibleToRide(new Vector2(enemyHitbox.Left, enemyHitbox.Bottom), point, CollideTag)
            && collisionController.IsItPossibleToRide(new Vector2(enemyHitbox.Right, enemyHitbox.Bottom), point, CollideTag)
            && collisionController.IsItPossibleToRide(new Vector2(enemyHitbox.Left, enemyHitbox.Top), point, CollideTag)
            && collisionController.IsItPossibleToRide(new Vector2(enemyHitbox.Right, enemyHitbox.Top), point, CollideTag);

        private void BuildPaths()
        {
            //Dijkstra algorithm
            this.previousInPath = new Int32[VerticesAmount];
            var distances = new Single[VerticesAmount];
            Array.Fill(distances, FarAway);
            distances[EnemyIndex] = 0;
            Array.Fill(previousInPath, NoWay);
            var visited = new Boolean[VerticesAmount];

            while (!visited.All(v => v))
            {
                var visitingNowIndex = distances.Select((dist, index) => new { Distance = dist, Index = index })
                    .Where(pair => !visited[pair.Index])
                    .OrderBy(pair => pair.Distance)
                    .First()
                    .Index;

                visited[visitingNowIndex] = true;

                foreach (var verticeIndex in Enumerable.Range(0, VerticesAmount).Where(verticeIndex => !visited[verticeIndex]))
                {
                    if (distances[visitingNowIndex] + edges[visitingNowIndex, verticeIndex] < distances[verticeIndex])
                    {
                        distances[verticeIndex] = distances[visitingNowIndex] + edges[visitingNowIndex, verticeIndex];
                        previousInPath[verticeIndex] = visitingNowIndex;
                    }
                }
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

            foreach (var row in Enumerable.Range(1, waypointsAmount))
                foreach (var col in Enumerable.Range(1, waypointsAmount))
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
