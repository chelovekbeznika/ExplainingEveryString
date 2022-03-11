using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using ExplainingEveryString.Core.GameModel.Movement.TargetSelectors;
using ExplainingEveryString.Core.Collisions;
using System.Collections.Generic;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Core.Tiles;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    internal class MoveTargetSelectorFactory
    {
        private CollisionsController collisionsController;
        private LevelData levelData;
        private Dictionary<String, RoomPointsGraph> roomGraphsCache;
        private TileWrapper map;
        private String roomName;

        public MoveTargetSelectorFactory(CollisionsController collisionsController, LevelData levelData, TileWrapper map)
        {
            this.collisionsController = collisionsController;
            this.levelData = levelData;
            this.roomGraphsCache = new Dictionary<String, RoomPointsGraph>();
            this.map = map;
            this.roomName = null;
        }

        internal IMoveTargetSelector Get(MoveTargetSelectType type, Vector2[] parameters, 
            Player player, IMovableCollidable actor)
        {
            Vector2 actorLocator() => actor.Position;
            switch (type)
            {
                case MoveTargetSelectType.NoTarget:
                    return new NotATargetSelector(actorLocator);
                case MoveTargetSelectType.TargetsList:
                    return new LoopingTargetSelector(parameters, actorLocator());
                case MoveTargetSelectType.RandomFromListTargets:
                    return new RandomFromListTargetsSelector(parameters, actorLocator());
                case MoveTargetSelectType.MoveTowardPlayer:
                    return new PlayerHunter(() => player.Position);
                case MoveTargetSelectType.MoveByPlayerTracks:
                    return new PlayerTracker(() => player.Position);
                case MoveTargetSelectType.RandomTargets:
                    return new RandomFlight(actor, parameters[0], parameters[1]);
                case MoveTargetSelectType.MoveTowardPlayerByWaypoints:
                    return GetPlayerHunterThroughWaypoints(player, actor);
                default:
                    throw new ArgumentException("Unknown type of MoveTargetSelectType");
            }
        }

        internal void SwitchRoom(Int32 waveNumber)
        {
            roomName = levelData.EnemyWaves[waveNumber].RoomName;
        }

        private IMoveTargetSelector GetPlayerHunterThroughWaypoints
            (Player player, IMovableCollidable actor)
        {
            if (roomName == null)
                return new PlayerHunter(() => player.Position);

            var roomCacheKey = $"{roomName}:{actor.CollideTag}";
            RoomPointsGraph roomGraph = null;
            if (roomGraphsCache.ContainsKey(roomCacheKey))
            {
                roomGraph = roomGraphsCache[roomCacheKey];
            }
            else
            {
                roomGraph = RoomPointsGraph.BuildRoomGraph(collisionsController, levelData, roomName, map, actor.CollideTag);
            }
            return new PlayerHunterThroughWaypoints(actor, player, collisionsController, roomGraph);
        }
    }
}
