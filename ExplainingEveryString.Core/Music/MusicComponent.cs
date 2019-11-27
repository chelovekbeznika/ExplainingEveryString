using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal class MusicComponent : GameComponent
    {
        private DynamicSoundEffectInstance sound;
        private Byte[] buffer;
        private Byte[] triangleBuffer;
        private Byte[] noiseBuffer;

        public MusicComponent(Game game) : base(game)
        {

            this.UpdateOrder = ComponentsOrder.Music;
        }

        public override void Initialize()
        {
            this.sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono);
            sound.Volume = 0.2F;
            PulseChannel pulse = new PulseChannel();
            TriangleChannel triangle = new TriangleChannel();
            NoiseChannel noise = new NoiseChannel();
            UInt16[] notes = new UInt16[] { 427, 380, 338, 319, 284, 253, 225 };
            List<SoundDirectingEvent> events = notes.Select((note, index) => new SoundDirectingEvent
            {
                Position = index * (Constants.SampleRate / 2),
                Value = note,
                Parameter = SoundChannelParameter.Timer
            }).ToList();
            this.triangleBuffer = triangle.GetMusic(events, Constants.SampleRate * 7 / 2);

            events.Insert(0, new SoundDirectingEvent
            {
                Position = 0,
                Value = 3,
                Parameter = SoundChannelParameter.Duty
            });
            events.Insert(0, new SoundDirectingEvent
            {
                Position = 0,
                Value = 7,
                Parameter = SoundChannelParameter.Volume
            });
            this.buffer = pulse.GetMusic(events, Constants.SampleRate * 7 / 2);

            List<SoundDirectingEvent> noiseEvents = new List<SoundDirectingEvent>
            {
                new SoundDirectingEvent { Parameter = SoundChannelParameter.Timer, Value = 0, Position = 0 },
                new SoundDirectingEvent { Parameter = SoundChannelParameter.Mode, Value = 1, Position = Constants.SampleRate / 2 }
            };
            this.noiseBuffer = noise.GetMusic(noiseEvents, Constants.SampleRate);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                if (sound.PendingBufferCount < 1)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                        sound.SubmitBuffer(noiseBuffer);
                    else if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        sound.SubmitBuffer(triangleBuffer);
                    else
                        sound.SubmitBuffer(buffer);
                    sound.Play();
                }
            }
        }
    }
}
