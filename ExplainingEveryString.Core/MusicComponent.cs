using Microsoft.Xna.Framework;
using ExplainingEveryString.Music;

namespace ExplainingEveryString.Core
{
    internal class MusicComponent : GameComponent
    {
        MusicPlayer musicPlayer;

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
            musicPlayer.Update();
            base.Update(gameTime);
        }
    }
}
