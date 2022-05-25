using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Notifications;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Notifications
{
    internal class NotificationsComponent : DrawableGameComponent
    {
        private NotificationsProcessor processor;
        private SpriteBatch spriteBatch;
        private EesGame game;
        private Dictionary<string, NotificationSpecification> specs;
        private bool wasGamePadConnected;

        internal NotificationsComponent(EesGame game) : base(game)
        {
            DrawOrder = ComponentsOrder.Notifications;
            UpdateOrder = ComponentsOrder.Notifications;
            this.game = game;
            specs = NotificationsSpecificationsAccess.Load()
                .ToDictionary(n => n.Type, n => n);
            processor = new NotificationsProcessor(specs);
            wasGamePadConnected = GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var spriteDataBuilder = new SpriteDataBuilder(game.Content, AssetsMetadataAccess.GetLoader());
            var assetsStorage = new NotificationsAssetsStorage();
            assetsStorage.FillStorage(specs, spriteDataBuilder, game.Content);
            processor.Initialize(assetsStorage);
        }

        public void SendNotification(string type)
        {
            processor.ReceiveNotification(type);
        }

        public override void Update(GameTime gameTime)
        {
            processor.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
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

            var gamepadConnectedNow = GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
            if (wasGamePadConnected && !gamepadConnectedNow)
                ProcessGamepadDisconnect();
            wasGamePadConnected = gamepadConnectedNow;
        }

        private void ProcessGamepadDisconnect()
        {
            processor.ReceiveNotification(NotificationType.GamepadDisconnected);
            game.GameState.TryPause();
        }
    }
}
