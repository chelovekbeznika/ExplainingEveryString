using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal static class TrajectoryFactory
    {
        private static readonly Dictionary<String, ConstructorInfo> constructorsCache = new Dictionary<String, ConstructorInfo>();

        internal static BulletTrajectory GetTrajectory
            (String type, Vector2 center, Vector2 fireDirection, Dictionary<String, Single> parameters)
        {
            var constructor = GetTrajectoryConstructor(type);
            return constructor.Invoke(new Object[] { center, fireDirection, parameters }) as BulletTrajectory;
        }

        private static ConstructorInfo GetTrajectoryConstructor(String type)
        {
            if (!constructorsCache.ContainsKey(type))
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
                constructorsCache.Add(type, constructor);
            }
            
            return constructorsCache[type];
        }
    }
}
