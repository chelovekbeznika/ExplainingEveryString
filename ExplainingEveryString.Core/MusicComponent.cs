using Microsoft.Xna.Framework;
using ExplainingEveryString.Music;
using System;
using ExplainingEveryString.Data.Configuration;

namespace ExplainingEveryString.Core
{
    internal class MusicComponent : GameComponent
    {
        private MusicPlayer musicPlayer;

        public MusicComponent(EesGame game) : base(game)
        {
            this.musicPlayer = new MusicPlayer();
            this.UpdateOrder = ComponentsOrder.Music;
            this.EnabledChanged += (sender, e) => 
            {
                if (Enabled)
                    musicPlayer.TryResume();
                else
                    musicPlayer.TryPause();
            };
        }

        public override void Initialize()
        {
            musicPlayer.Initialize(ConfigurationAccess.GetCurrentConfig().MusicVolume);
            base.Initialize();
        }

        public void PlaySong(String songName)
        {
            musicPlayer.Start(songName);
        }

        public void Stop()
        {
            musicPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            musicPlayer.Update();
            base.Update(gameTime);
        }
    }
}
