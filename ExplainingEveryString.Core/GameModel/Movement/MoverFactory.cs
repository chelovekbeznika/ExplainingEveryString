using ExplainingEveryString.Core.GameModel.Movement.Movers;
using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    internal static class MoverFactory
    {
        internal static IMover Get(MoverSpecification specification)
        {
            if (specification == null)
            {
                return new NonMover();
            }

            switch (specification.Type)
            {
                case MoveType.StayingStill:
                    return new NonMover();
                case MoveType.Linear:
                    Single speed = specification.Parameters["speed"];
                    return new LinearMover(speed);
                case MoveType.Acceleration:
                    Single maxSpeed = specification.Parameters["maxSpeed"];
                    Single startSpeed = specification.Parameters["startSpeed"];
                    Single acceleration = specification.Parameters["acceleration"];
                    return new AccelerationMover(acceleration, startSpeed, maxSpeed);
                case MoveType.Teleportation:
                    Single minTillTeleport = specification.Parameters["min"];
                    Single maxTillTeleport = specification.Parameters["max"];
                    return new TeleportationMover(minTillTeleport, maxTillTeleport);
                default:
                    throw new ArgumentException("Unknown MoveType");
            }
        }
    }
}
