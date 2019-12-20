using Microsoft.Xna.Framework;
using ExplainingEveryString.Music;
using System;

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
            musicPlayer.Initialize();
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
