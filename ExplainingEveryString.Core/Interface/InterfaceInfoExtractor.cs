using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceInfoExtractor
    {
        internal InterfaceInfo GetInterfaceInfo(Camera camera, ActiveActorsStorage activeActors, LevelProgress levelProgress)
        {
            return new InterfaceInfo
            {
                Player = GetInterfaceInfo(activeActors.Player),
                GameTime = levelProgress.GameTime,
                Enemies = activeActors.Enemies
                            .Where(e => camera.IsVisibleOnScreen(e)).OfType<IInterfaceAccessable>()
                            .Where(e => e.ShowInterfaceInfo).Select(e => GetInterfaceInfo(e, camera)).ToList()
            };
        }

        private PlayerInterfaceInfo GetInterfaceInfo(Player player)
        {
            return new PlayerInterfaceInfo
            {
                Health = player.HitPoints,
                MaxHealth = player.MaxHitPoints,
                DashCooldown = player.DashController.RechargeTime,
                TillDashRecharge = player.DashController.TillRecharge,
                DashState = player.DashController.IsActive 
                    ? DashState.Active 
                    : player.DashController.IsAvailable
                        ? DashState.Available 
                        : DashState.Nonavailable
            };
        }

        private EnemyInterfaceInfo GetInterfaceInfo(IInterfaceAccessable interfaceAccessable, Camera camera)
        {
            return new EnemyInterfaceInfo
            {
                Health = interfaceAccessable.HitPoints,
                MaxHealth = interfaceAccessable.MaxHitPoints,
                PositionOnScreen = camera.PositionOnScreen(interfaceAccessable)
            };
        }
    }
}
