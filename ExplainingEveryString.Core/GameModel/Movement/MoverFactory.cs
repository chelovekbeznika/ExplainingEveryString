using ExplainingEveryString.Core.GameModel.Movement.Movers;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                default:
                    throw new ArgumentException("Unknown MoveType");
            }
        }
    }
}
