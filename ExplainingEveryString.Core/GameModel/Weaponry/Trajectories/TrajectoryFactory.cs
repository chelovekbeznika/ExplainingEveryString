using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal class TrajectoryFactory
    {
        private Dictionary<String, ConstructorInfo> constructorsCache = new Dictionary<String, ConstructorInfo>();

        internal BulletTrajectory GetTrajectory
            (String type, Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters)
        {
            ConstructorInfo constructor = GetTrajectoryConstructor(type);
            return constructor.Invoke(new Object[] { startPosition, fireDirection, parameters }) as BulletTrajectory;
        }

        private ConstructorInfo GetTrajectoryConstructor(String type)
        {
            String trajectoryClassName = $"ExplainingEveryString.Core.GameModel.Weaponry.Trajectories.{type}Trajectory";
            Type trajectoryClass = Type.GetType(trajectoryClassName);
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            ConstructorInfo constructor = trajectoryClass.GetConstructor(bindingFlags, null, new Type[]
            {
                typeof(Vector2),
                typeof(Vector2),
                typeof(Dictionary<String, Single>)
            }, null);
            return constructor;
        }
    }
}
