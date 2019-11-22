using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

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
            PulseChannel pulse = new PulseChannel();
            UInt16[] notes = new UInt16[] { 427, 380, 338, 319, 284, 253, 225 };
            this.buffer = notes.SelectMany(note => pulse.GetNote(15, 0, note, Constants.SampleRate / 4)).ToArray();
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
