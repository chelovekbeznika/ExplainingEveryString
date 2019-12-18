using Microsoft.Xna.Framework;
using ExplainingEveryString.Music;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core
{
    internal class MusicComponent : GameComponent
    {
        private MusicPlayer musicPlayer;
        private Double betweenPresses = 0;

        public MusicComponent(Game game) : base(game)
        {
            this.musicPlayer = new MusicPlayer();
            this.UpdateOrder = ComponentsOrder.Music;
        }

        public override void Initialize()
        {
            musicPlayer.Initialize();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.M) && betweenPresses > 1)
            {
                if (state.IsKeyDown(Keys.Enter))
                    musicPlayer.Stop();
                else if (state.IsKeyDown(Keys.RightShift))
                    musicPlayer.PauseSwitch();
                else
                    musicPlayer.Start("gamma");
                betweenPresses = 0;
            }
            musicPlayer.Update();
            betweenPresses += gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }
    }
}
