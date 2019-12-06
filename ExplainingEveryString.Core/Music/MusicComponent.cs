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
        private Byte[] lengthTest;
        private Byte[] envelopeTest;
        private Byte[] sweepTest;
        private Byte[] deltaTest;

        public MusicComponent(Game game) : base(game)
        {
            this.UpdateOrder = ComponentsOrder.Music;
        }

        public override void Initialize()
        {
            this.sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono);
            this.buffer = new Mixer().GetMusic(GetTestSong(), 20);
            this.lengthTest = new Mixer().GetMusic(GetLengthCounterTest(), 16);
            this.envelopeTest = new Mixer().GetMusic(GetEnvelopeTestLength(), 16);
            this.sweepTest = new Mixer().GetMusic(GetSweepTest(), 12);
            this.deltaTest = new Mixer().GetMusic(GetDeltaTest(), 2);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                if (sound.PendingBufferCount < 1)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.RightAlt))
                        sound.SubmitBuffer(sweepTest);
                    else if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                        sound.SubmitBuffer(envelopeTest);
                    else if (Keyboard.GetState().IsKeyDown(Keys.RightControl))
                        sound.SubmitBuffer(lengthTest);
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        sound.SubmitBuffer(deltaTest);
                    else
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
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.Pulse1Enabled,
                    Value = 1
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.TriangleEnabled,
                    Value = 1
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.NoiseEnabled,
                    Value = 1
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Duty,
                    Value = 3
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Volume,
                    Value = 15
                }
            };
            events.AddRange(notes.Select((note, index) => new SoundDirectingEvent
            {
                Seconds = index / 2,
                SamplesOffset = index % 2 == 1 ? Constants.SampleRate / 2 : 0,
                SoundComponent = SoundComponentType.Pulse1,
                Parameter = SoundChannelParameter.Timer,
                Value = note,
            }));
            events.Add(new SoundDirectingEvent
            {
                Seconds = 3,
                SamplesOffset = Constants.SampleRate / 2,
                SoundComponent = SoundComponentType.Pulse1,
                Parameter = SoundChannelParameter.Volume,
                Value = 0
            });
            events.AddRange(notes.Skip(2).Select((note, index) => new SoundDirectingEvent
            {
                Seconds = index / 2 + 1,
                SamplesOffset = index % 2 == 1 ? Constants.SampleRate / 2 : 0,
                SoundComponent = SoundComponentType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = note
            }));
            events.Add(new SoundDirectingEvent
            {
                Seconds = 3,
                SamplesOffset = Constants.SampleRate / 2,
                SoundComponent = SoundComponentType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = 0
            });
            events.Add(new SoundDirectingEvent
            {
                Seconds = 4,
                SoundComponent = SoundComponentType.Noise,
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
                        SoundComponent = SoundComponentType.Noise,
                        Parameter = SoundChannelParameter.NoiseMode,
                        Value = 0
                    },
                    new SoundDirectingEvent
                    {
                        Value = timer,
                        Seconds = seconds,
                        SoundComponent = SoundComponentType.Noise,
                        Parameter = SoundChannelParameter.Timer
                    },
                    new SoundDirectingEvent
                    {
                        Seconds = seconds,
                        SamplesOffset = Constants.SampleRate / 2,
                        SoundComponent = SoundComponentType.Noise,
                        Parameter = SoundChannelParameter.NoiseMode,
                        Value = 1             
                    }
                };
            }));
            events.Sort((x, y) => x.Position - y.Position);
            return events;
        }

        private List<SoundDirectingEvent> GetLengthCounterTest()
        {
            Int32[] testLengthsBase10 = new Int32[] { 192, 96, 48, 24, 12, 72, 32, 16 };
            return new SoundDirectingEvent[]
            {
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.Pulse1Enabled,
                    Value = 1
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.FrameCounter,
                    Parameter = SoundChannelParameter.FrameCounterMode,
                    Value = 1
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Volume,
                    Value = 15
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Duty,
                    Value = 2
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Timer,
                    Value = 253
                },
                new SoundDirectingEvent
                {
                    Seconds = 0, 
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.HaltLoopFlag,
                    Value = 0
                }
            }
            .Concat(testLengthsBase10.SelectMany((length, index) => new SoundDirectingEvent[]
            {
                new SoundDirectingEvent
                {
                    Seconds = index * 2,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.LengthCounter,
                    Value = length
                }
            }))
            .ToList();
        }

        private List<SoundDirectingEvent> GetEnvelopeTestLength()
        {
            List<SoundDirectingEvent> result = new List<SoundDirectingEvent>
            {
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.Pulse1Enabled,
                    Value = 1
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.FrameCounter,
                    Parameter = SoundChannelParameter.FrameCounterMode,
                    Value = 0
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.HaltLoopFlag,
                    Value = 0
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Duty,
                    Value = 2
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.EnvelopeConstant,
                    Value = 0
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Timer,
                    Value = 427
                }
            };
            result.AddRange(Enumerable.Range(0, 15).Reverse().SelectMany((volume, index) => new SoundDirectingEvent[]
            {
                new SoundDirectingEvent
                {
                    Seconds = index,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Volume,
                    Value = volume
                },
                new SoundDirectingEvent
                {
                    Seconds = index,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Timer,
                    Value = 427
                },
                new SoundDirectingEvent
                {
                    Seconds = index,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.LengthCounter,
                    Value = 120,
                }
            }));
            return result;
        }

        private List<SoundDirectingEvent> GetSweepTest()
        {
            return new List<SoundDirectingEvent>()
            {
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.Pulse2Enabled,
                    Value = 1
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.Timer,
                    Value = 8
                },
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.Volume,
                    Value = 15
                },
                new SoundDirectingEvent
                {
                    Seconds = 1,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.SweepPeriod,
                    Value = 7
                },
                new SoundDirectingEvent
                {
                    Seconds = 1,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.SweepAmount,
                    Value = 3
                },
                new SoundDirectingEvent
                {
                    Seconds = 1,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.SweepEnabled,
                    Value = 1
                },
                new SoundDirectingEvent
                {
                    Seconds = 6,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.SweepNegate,
                    Value = 1
                }
            };
        }

        private List<SoundDirectingEvent> GetDeltaTest()
        {
            return new List<SoundDirectingEvent>
            {
                new SoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.DeltaEnabled,
                    Value = 1
                }
            };
        }
    }
}
