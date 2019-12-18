using ExplainingEveryString.Core.Music.Model;
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
        private List<Byte[]> deltaSamplesLibrary;
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
            this.deltaSamplesLibrary = DeltaSamplesLibraryLoader.Load(@"Content/Data/Music/deltasamples.dat");
            this.sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono);
            this.buffer = new NesSoundChipReplica(deltaSamplesLibrary).GetMusic(GetTestSong(), 20);
            this.lengthTest = new NesSoundChipReplica(deltaSamplesLibrary)
                .GetMusic(GetLengthCounterTest().Cast<ISoundDirectingSequence>().ToList(), 16);
            this.envelopeTest = new NesSoundChipReplica(deltaSamplesLibrary)
                .GetMusic(GetEnvelopeTestLength().Cast<ISoundDirectingSequence>().ToList(), 16);
            this.sweepTest = new NesSoundChipReplica(deltaSamplesLibrary)
                .GetMusic(GetSweepTest().Cast<ISoundDirectingSequence>().ToList(), 12);
            this.deltaTest = new NesSoundChipReplica(deltaSamplesLibrary)
                .GetMusic(GetDeltaTest().Cast<ISoundDirectingSequence>().ToList(), 16);
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

        private List<ISoundDirectingSequence> GetTestSong()
        {
            NoteType[] notes = new NoteType[] { NoteType.C, NoteType.D, NoteType.E, NoteType.F, NoteType.G, NoteType.A, NoteType.H };
            List<ISoundDirectingSequence> result = new List<ISoundDirectingSequence>
            {
                new SwitchChannel
                {
                    Seconds = 0,
                    Channel = SoundComponentType.Pulse1,
                    TurnOn = true
                },
                new SwitchChannel
                {
                    Seconds = 5,
                    Channel = SoundComponentType.Triangle,
                    TurnOn = true
                },
                new SwitchChannel
                {
                    Seconds = 5,
                    Channel = SoundComponentType.Noise,
                    TurnOn = true
                }
            };
            result.Add(new BpmSequence
            {
                BeatsPerMinute = 90,
                BpmNotes = notes.Select((note, index) => new PulseNote
                {
                    BeatNumber = index,
                    Note = new Note(Octave.OneLine, note),
                    Length = NoteLength.Quarter,
                    Volume = 15,
                    Decaying = true
                })
            });
            result.Add(new BpmSequence
            {
                BeatsPerMinute = 140,
                Seconds = 5,
                BpmNotes = notes.Select((note, index) => new TriangleNote
                {
                    BeatNumber = index,
                    Note = new Note(Octave.OneLine, note),
                    Length = NoteLength.Quarter
                })
            });
            result.Add(new BpmSequence
            {
                BeatsPerMinute = 140,
                Seconds = 5,
                BpmNotes = Enumerable.Range(0, 7).Select(index => new NoiseNote
                {
                    NoiseType = 8,
                    LoopedNoise = true,
                    BeatNumber = index,
                    Volume = 7,
                    Length = NoteLength.Sixteenth
                })
            });
            return result;
        }

        private List<RawSoundDirectingEvent> GetLengthCounterTest()
        {
            Int32[] testLengthsBase10 = new Int32[] { 192, 96, 48, 24, 12, 72, 32, 16 };
            return new RawSoundDirectingEvent[]
            {
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.Pulse1Enabled,
                    Value = 1
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.FrameCounter,
                    Parameter = SoundChannelParameter.FrameCounterMode,
                    Value = 1
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Volume,
                    Value = 15
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Duty,
                    Value = 2
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Timer,
                    Value = 253
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0, 
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.HaltLoopFlag,
                    Value = 0
                }
            }
            .Concat(testLengthsBase10.SelectMany((length, index) => new RawSoundDirectingEvent[]
            {
                new RawSoundDirectingEvent
                {
                    Seconds = index * 2,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.LengthCounter,
                    Value = length
                }
            }))
            .ToList();
        }

        private List<RawSoundDirectingEvent> GetEnvelopeTestLength()
        {
            List<RawSoundDirectingEvent> result = new List<RawSoundDirectingEvent>
            {
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.Pulse1Enabled,
                    Value = 1
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.FrameCounter,
                    Parameter = SoundChannelParameter.FrameCounterMode,
                    Value = 0
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.HaltLoopFlag,
                    Value = 0
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Duty,
                    Value = 2
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.EnvelopeConstant,
                    Value = 0
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Timer,
                    Value = 427
                }
            };
            result.AddRange(Enumerable.Range(0, 15).Reverse().SelectMany((volume, index) => new RawSoundDirectingEvent[]
            {
                new RawSoundDirectingEvent
                {
                    Seconds = index,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Volume,
                    Value = volume
                },
                new RawSoundDirectingEvent
                {
                    Seconds = index,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.Timer,
                    Value = 427
                },
                new RawSoundDirectingEvent
                {
                    Seconds = index,
                    SoundComponent = SoundComponentType.Pulse1,
                    Parameter = SoundChannelParameter.LengthCounter,
                    Value = 120,
                }
            }));
            return result;
        }

        private List<RawSoundDirectingEvent> GetSweepTest()
        {
            return new List<RawSoundDirectingEvent>()
            {
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.Pulse2Enabled,
                    Value = 1
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.Timer,
                    Value = 8
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.Volume,
                    Value = 15
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 1,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.SweepPeriod,
                    Value = 7
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 1,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.SweepAmount,
                    Value = 3
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 1,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.SweepEnabled,
                    Value = 1
                },
                new RawSoundDirectingEvent
                {
                    Seconds = 6,
                    SoundComponent = SoundComponentType.Pulse2,
                    Parameter = SoundChannelParameter.SweepNegate,
                    Value = 1
                }
            };
        }

        private List<RawSoundDirectingEvent> GetDeltaTest()
        {
            return new List<RawSoundDirectingEvent>
            {
                new RawSoundDirectingEvent
                {
                    Seconds = 0,
                    SoundComponent = SoundComponentType.Status,
                    Parameter = SoundChannelParameter.DeltaEnabled,
                    Value = 1
                }
            }.Concat(Enumerable.Range(0, 15).SelectMany((timer, index) => new RawSoundDirectingEvent[]
            {
                new RawSoundDirectingEvent
                {
                    Seconds = index,
                    SoundComponent = SoundComponentType.DeltaModulation,
                    Parameter = SoundChannelParameter.CurrentSample,
                    Value = 0
                },
                new RawSoundDirectingEvent
                {
                    Seconds = index,
                    SoundComponent = SoundComponentType.DeltaModulation,
                    Parameter = SoundChannelParameter.Timer,
                    Value = timer
                }
            })).ToList();
        }
    }
}
