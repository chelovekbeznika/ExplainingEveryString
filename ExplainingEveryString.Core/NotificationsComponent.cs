using ExplainingEveryString.Core.Notifications;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Notifications;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core
{
    internal class NotificationsComponent : DrawableGameComponent
    {
        private NotificationsProcessor processor;
        private SpriteBatch spriteBatch;
        private EesGame game;
        private Dictionary<String, NotificationSpecification> specs;
        private Boolean wasGamePadConnected;

        internal NotificationsComponent(EesGame game) : base(game)
        {
            this.DrawOrder = ComponentsOrder.Notifications;
            this.UpdateOrder = ComponentsOrder.Notifications;
            this.game = game;
            this.specs = NotificationsSpecificationsAccess.Load()
                .ToDictionary(n => n.Type, n => n);
            this.processor = new NotificationsProcessor(specs);
            this.wasGamePadConnected = GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            var spriteDataBuilder = new SpriteDataBuilder(game.Content, AssetsMetadataAccess.GetLoader());
            var assetsStorage = new NotificationsAssetsStorage();
            assetsStorage.FillStorage(specs, spriteDataBuilder, game.Content);
            this.processor.Initialize(assetsStorage);
        }

        public void SendNotification(String type)
        {
            processor.ReceiveNotification(type);
        }

        public override void Update(GameTime gameTime)
        {
            processor.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
            CheckGamePadConnection();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            processor.DrawCurrentNotification(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void CheckGamePadConnection()
        {
            if (ConfigurationAccess.GetCurrentConfig().Input.PreferredControlDevice != ControlDevice.GamePad)
                return;

            if (wasGamePadConnected && !GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
                processor.ReceiveNotification(NotificationType.GamepadDisconnected);
            wasGamePadConnected = GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
        }
    }
}
