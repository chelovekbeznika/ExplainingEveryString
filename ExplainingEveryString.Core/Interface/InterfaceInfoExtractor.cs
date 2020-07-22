﻿using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Level;
using System.Linq;

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
                            .Where(e => e.ShowInterfaceInfo && !(activeActors.Bosses?.Contains(e) ?? false))
                            .Select(e => GetInterfaceInfo(e, camera)).ToList(),
                HiddenEnemies = activeActors.Enemies
                            .Where(e => e.IsVisible && !camera.IsVisibleOnScreen(e))
                            .Select(e => camera.GetScreenBorderDangerDirection(e)).ToList(),
                Bosses = activeActors.Bosses?.Select(boss => GetInterfaceInfo(boss, camera)).ToList()
            };
        }

        private PlayerInterfaceInfo GetInterfaceInfo(Player player)
        {
            return new PlayerInterfaceInfo
            {
                Health = player.HitPoints > 0 ? player.HitPoints : 0,
                MaxHealth = player.MaxHitPoints,
                FromLastHit = player.FromLastHit,
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
                Health = interfaceAccessable.HitPoints > 0 ? interfaceAccessable.HitPoints : 0,
                MaxHealth = interfaceAccessable.MaxHitPoints,
                FromLastHit = interfaceAccessable.FromLastHit,
                PositionOnScreen = camera.PositionOnScreen(interfaceAccessable)
            };
        }
    }
}
