using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Level;
using System.Linq;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceInfoExtractor
    {
        internal InterfaceInfo GetInterfaceInfo(Camera camera, ActiveActorsStorage activeActors, LevelProgress levelProgress)
        {
            var homingTarget = activeActors.Player.CurrentTarget;
            return new InterfaceInfo
            {
                Player = GetInterfaceInfo(activeActors.Player, camera),
                GameTime = levelProgress.GameTime,
                Enemies = activeActors.Enemies
                            .Where(e => camera.IsVisibleOnScreen(e)).OfType<IInterfaceAccessable>()
                            .Where(e => e.ShowInterfaceInfo && !(activeActors.ShowAsBossesInInterface?.Contains(e) ?? false))
                            .Select(e => GetInterfaceInfo(e, camera)).ToList(),
                HiddenEnemies = activeActors.Enemies
                            .Where(e => e.IsVisible && !camera.IsVisibleOnScreen(e))
                            .Select(e => camera.GetScreenBorderDangerDirection(e)).ToList(),
                Bosses = activeActors.ShowAsBossesInInterface?.Select(boss => GetInterfaceInfo(boss,  camera)).ToList(),
                EnemiesLevelPositions = activeActors.Enemies
                            .Where(e => e.ShowInterfaceInfo && !(activeActors.ShowAsBossesInInterface?.Contains(e) ?? false))
                            .Select(e => (e as ICollidable).Position).ToList(),
                BossesLevelPositions = activeActors.ShowAsBossesInInterface?
                            .Select(b => (b as ICollidable).Position).ToList()
            };
        }

        private PlayerInterfaceInfo GetInterfaceInfo(Player player, Camera camera)
        {
            return new PlayerInterfaceInfo
            {
                LevelPosition = player.Position,
                Health = player.HitPoints > 0 ? player.HitPoints : 0,
                MaxHealth = player.MaxHitPoints,
                FromLastHit = player.FromLastHit,
                FromLastCheckpoint = player.FromLastCheckpoint,
                DashCooldown = player.DashController.RechargeTime,
                TillDashRecharge = player.DashController.TillRecharge,
                DashState = player.DashController.IsActive
                    ? DashState.Active
                    : player.DashController.IsAvailable
                        ? DashState.Available
                        : DashState.Nonavailable,
                Weapon = new PlayerWeaponInterfaceInfo
                {
                    SelectedWeapon = player.Weapon.Name,
                    AvailableWeapons = player.AvailableWeapons.ToList(),
                    CurrentAmmo = player.Weapon.Reloader.CurrentAmmo,
                    MaxAmmo = player.Weapon.Reloader.MaxAmmo,
                    AmmoStock = player.Weapon.Reloader.AmmoStock,
                    ReloadRemained = player.Weapon.Reloader.ReloadRemained
                },
                HomingTarget = player.CurrentTarget != null ? GetInterfaceInfo(player.CurrentTarget, camera) : null
            };
        }

        private EnemyInterfaceInfo GetInterfaceInfo(
            IInterfaceAccessable interfaceAccessable, Camera camera)
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
