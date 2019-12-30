using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class TrajectoryFactory
    {
        private Dictionary<String, ConstructorInfo> constructorsCache = new Dictionary<String, ConstructorInfo>();

        internal BulletTrajectory GetTrajectory
            (String type, Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters)
        {
            var constructor = GetTrajectoryConstructor(type);
            return constructor.Invoke(new Object[] { startPosition, fireDirection, parameters }) as BulletTrajectory;
        }

        private ConstructorInfo GetTrajectoryConstructor(String type)
        {
            var trajectoryClassName = $"ExplainingEveryString.Core.GameModel.Weaponry.Trajectories.{type}Trajectory";
            var trajectoryClass = Type.GetType(trajectoryClassName);
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            var constructor = trajectoryClass.GetConstructor(bindingFlags, null, new Type[]
            {
                typeof(Vector2),
                typeof(Vector2),
                typeof(Dictionary<String, Single>)
            }, null);
            return constructor;
        }
    }
}
