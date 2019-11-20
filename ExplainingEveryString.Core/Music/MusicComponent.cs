using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Music
{
    internal class MusicComponent : GameComponent
    {
        DynamicSoundEffectInstance sound;
        Byte[] buffer;

        public MusicComponent(Game game) : base(game)
        {

            this.UpdateOrder = ComponentsOrder.Music;
        }

        public override void Initialize()
        {
            this.sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono);
            sound.Volume = 0.2F;
            this.buffer = new PulseChannel().GetMusic();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                if (sound.PendingBufferCount < 1)
                {
                    sound.SubmitBuffer(buffer);
                    sound.Play();
                }
            }
        }
    }
}
