using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Notifications;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Notifications
{
    internal class NotificationsProcessor : GameModel.IUpdateable
    {
        private const Single scale = 2;
        private Dictionary<String, NotificationSpecification> specs;
        private IAssetsStorage assetsStorage;
        private List<Notification> activeNotifications = new List<Notification>();
        private Notification Current => activeNotifications.Count > 0 ? activeNotifications[0] : null;

        internal NotificationsProcessor(Dictionary<String, NotificationSpecification> specs)
        {
            this.specs = specs;
        }

        internal void Initialize(IAssetsStorage assetsStorage)
        {
            this.assetsStorage = assetsStorage;
        }

        public void Update(Single elapsedSeconds)
        {
            activeNotifications.ForEach(n => n.Update(elapsedSeconds));
            SortNotifications();
        }

        internal void ReceiveNotification(String type)
        {
            var spec = specs[type];
            var newNotification = new Notification(spec, assetsStorage);
            activeNotifications.Add(newNotification);
            SortNotifications();
            if (newNotification == Current)
            {
                var volume = ConfigurationAccess.GetCurrentConfig().SoundVolume;
                assetsStorage.GetSound(Current.Type).Play(volume, 0, 0); 
            }
                
        }

        internal void DrawCurrentNotification(SpriteBatch spriteBatch)
        {
            if (Current == null)
                return;

            var height = Displaying.Constants.TargetHeight;
            var spriteState = Current.Sprite;
            var spriteData = assetsStorage.GetSprite(Current.Type);
            var drawPart = AnimationHelper.GetDrawPart(spriteData, spriteState.AnimationCycle, spriteState.ElapsedTime);
            var position = new Vector2(32, height - spriteData.Height * scale - 32);
            spriteBatch.Draw(spriteData.Sprite, position, drawPart, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        private void SortNotifications()
        {
            activeNotifications = activeNotifications
                .Where(n => n.IsActive)
                .OrderByDescending(n => n.TimeToLive)
                .OrderByDescending(n => n.Priority)
                .OrderBy(n => n.Type)
                .ToList();
        }
    }
}
