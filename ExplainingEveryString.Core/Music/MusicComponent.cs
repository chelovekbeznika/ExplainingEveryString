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

        public MusicComponent(Game game) : base(game)
        {
            this.UpdateOrder = ComponentsOrder.Music;
        }

        public override void Initialize()
        {
            this.sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono);
            this.buffer = new Mixer().GetMusic(GetTestSong(), 20);
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

        private List<SoundDirectingEvent> GetTestSong()
        {
            UInt16[] notes = new UInt16[] { 427, 380, 338, 319, 284, 253, 225 };
            List<SoundDirectingEvent> events = new List<SoundDirectingEvent>
            {
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundChannel = SoundChannelType.Pulse1,
                    Parameter = SoundChannelParameter.Duty,
                    Value = 3
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundChannel = SoundChannelType.Pulse1,
                    Parameter = SoundChannelParameter.Volume,
                    Value = 15
                }
            };
            events.AddRange(notes.Select((note, index) => new SoundDirectingEvent
            {
                Seconds = index / 2,
                SamplesOffset = index % 2 == 1 ? Constants.SampleRate / 2 : 0,
                SoundChannel = SoundChannelType.Pulse1,
                Parameter = SoundChannelParameter.Timer,
                Value = note,
            }));
            events.Add(new SoundDirectingEvent
            {
                Seconds = 3,
                SamplesOffset = Constants.SampleRate / 2,
                SoundChannel = SoundChannelType.Pulse1,
                Parameter = SoundChannelParameter.Volume,
                Value = 0
            });
            events.AddRange(notes.Skip(2).Select((note, index) => new SoundDirectingEvent
            {
                Seconds = index / 2 + 1,
                SamplesOffset = index % 2 == 1 ? Constants.SampleRate / 2 : 0,
                SoundChannel = SoundChannelType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = note
            }));
            events.Add(new SoundDirectingEvent
            {
                Seconds = 3,
                SamplesOffset = Constants.SampleRate / 2,
                SoundChannel = SoundChannelType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = 0
            });
            events.Add(new SoundDirectingEvent
            {
                Seconds = 4,
                SoundChannel = SoundChannelType.Noise,
                Parameter = SoundChannelParameter.Volume,
                Value = 15
            });
            events.AddRange(Enumerable.Range(0, 16).SelectMany(timer =>
            {
                Int32 seconds = 4 + timer;
                return new List<SoundDirectingEvent>
                {
                    new SoundDirectingEvent
                    {
                        Seconds = seconds,
                        SoundChannel = SoundChannelType.Noise,
                        Parameter = SoundChannelParameter.Mode,
                        Value = 0
                    },
                    new SoundDirectingEvent
                    {
                        Value = timer,
                        Seconds = seconds,
                        SoundChannel = SoundChannelType.Noise,
                        Parameter = SoundChannelParameter.Timer
                    },
                    new SoundDirectingEvent
                    {
                        Seconds = seconds,
                        SamplesOffset = Constants.SampleRate / 2,
                        SoundChannel = SoundChannelType.Noise,
                        Parameter = SoundChannelParameter.Mode,
                        Value = 1             
                    }
                };
            }));
            events.Sort((x, y) => x.Position - y.Position);
            return events;
        }
    }
}
