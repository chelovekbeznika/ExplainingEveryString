using ExplainingEveryString.Core.Collisions;
#if DEBUG
using ExplainingEveryString.Core.Displaying.Debug;
#endif
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class PlayerHunterThroughWaypoints : IMoveTargetSelector
    {
        private const Single TooClose = 8;

        private Player player;
        private IMovableCollidable hunter;
        private CollisionsController collisionsController;
        private RoomPointsGraph roomGraph;

        internal PlayerHunterThroughWaypoints(IMovableCollidable hunter, Player player, 
            CollisionsController collisionsController, RoomPointsGraph roomGraph)
        {
            this.hunter = hunter;
            this.player = player;
            this.collisionsController = collisionsController;
            this.roomGraph = roomGraph;
        }

        public Vector2 GetTarget()
        {
            if (HunterCanRideToPlayer())
                return player.Position;
            else
            {
                var path = roomGraph.GetWayInLevel(hunter.Position, hunter.GetOldHitbox(), player.Position);
#if DEBUG
                if (path != null)
                {
                    DebugInfoDisplayer.Instance.AddDebugInfo(path);
                }
#endif
                var nextPoint = path?.First(point => Vector2.Distance(hunter.Position, point) > TooClose);
                return nextPoint ?? player.Position;
            }
        }

        private Boolean HunterCanRideToPlayer()
        {
            var hunterHitbox = hunter.GetCurrentHitbox();
            var playerHitbox = player.GetOldHitbox();
            var pathIsClear = collisionsController.IsItPossibleToRide(TopLeft(hunterHitbox), TopLeft(playerHitbox), hunter.CollideTag)
                && collisionsController.IsItPossibleToRide(BottomLeft(hunterHitbox), BottomLeft(playerHitbox), hunter.CollideTag)
                && collisionsController.IsItPossibleToRide(TopRight(hunterHitbox), TopRight(playerHitbox), hunter.CollideTag)
                && collisionsController.IsItPossibleToRide(BottomRight(hunterHitbox), BottomRight(playerHitbox), hunter.CollideTag);
            return pathIsClear;
        }

        private Vector2 TopLeft(Hitbox hitbox) => new Vector2(hitbox.Left + 1, hitbox.Top - 1);
        private Vector2 TopRight(Hitbox hitbox) => new Vector2(hitbox.Right - 1, hitbox.Top - 1);
        private Vector2 BottomLeft(Hitbox hitbox) => new Vector2(hitbox.Left + 1, hitbox.Bottom + 1);
        private Vector2 BottomRight(Hitbox hitbox) => new Vector2(hitbox.Right - 1, hitbox.Bottom + 1);

        public void SwitchToNextTarget()
        {
            GetTarget();
        }
    }
}
