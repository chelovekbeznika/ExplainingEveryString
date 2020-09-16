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
                    var speed = specification.Parameters["speed"];
                    return new LinearMover(speed);
                case MoveType.Acceleration:
                    var maxSpeed = specification.Parameters["maxSpeed"];
                    var startSpeed = specification.Parameters["startSpeed"];
                    var acceleration = specification.Parameters["acceleration"];
                    return new AccelerationMover(acceleration, startSpeed, maxSpeed);
                case MoveType.Axis:
                    var scalarSpeed = specification.Parameters["speed"];
                    var minTimeTillAxeSwitch = specification.Parameters["minTimeTillAxeSwitch"];
                    var maxTimeTillAxeSwitch = specification.Parameters["maxTimeTillAxeSwitch"];
                    return new AxisMover(scalarSpeed, minTimeTillAxeSwitch, maxTimeTillAxeSwitch);
                case MoveType.Teleportation:
                    var minTillTeleport = specification.Parameters["min"];
                    var maxTillTeleport = specification.Parameters["max"];
                    return new TeleportationMover(minTillTeleport, maxTillTeleport);
                default:
                    throw new ArgumentException("Unknown MoveType");
            }
        }
    }
}
