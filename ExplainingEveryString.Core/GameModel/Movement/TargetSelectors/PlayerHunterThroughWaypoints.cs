using ExplainingEveryString.Core.Collisions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class PlayerHunterThroughWaypoints : IMoveTargetSelector
    {
        private const Single TooClose = 2;

        private Func<Vector2> playerLocator;
        private Func<Vector2> currentPosition;
        private CollisionsController collisionsController;
        private RoomPointsGraph roomGraph;

        private Vector2 nextPoint;

        internal PlayerHunterThroughWaypoints(Func<Vector2> currentPosition, Func<Vector2> playerLocator, 
            CollisionsController collisionsController, RoomPointsGraph roomGraph)
        {
            this.currentPosition = currentPosition;
            this.playerLocator = playerLocator;
            this.collisionsController = collisionsController;
            this.roomGraph = roomGraph;
        }

        public Vector2 GetTarget()
        {
            return nextPoint;
        }

        public void SwitchToNextTarget()
        {
            if (collisionsController.IsItPossibleToRide(currentPosition(), playerLocator(), roomGraph.CollideTag))
            {
                nextPoint = playerLocator();
            }
            else
            {
                var path = roomGraph.GetWayInLevel(currentPosition(), playerLocator());
                nextPoint = path.First(point => Vector2.Distance(currentPosition(), point) > TooClose);
            }
        }
    }
}
