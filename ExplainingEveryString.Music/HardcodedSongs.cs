using ExplainingEveryString.Music.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Music
{
    internal static class HardcodedSongs
    {
        internal static List<ISoundDirectingSequence> GetTestSong()
        {
            var notes = new NoteType[] { NoteType.C, NoteType.D, NoteType.E, NoteType.F, NoteType.G, NoteType.A, NoteType.H };
            var result = new List<ISoundDirectingSequence>
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
                CommonPart = notes.Select((note, index) => new PulseNote
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
                CommonPart = notes.Select((note, index) => new TriangleNote
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
                CommonPart = Enumerable.Range(0, 7).Select(index => new NoiseNote
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

        internal static List<RawSoundDirectingEvent> GetLengthCounterTest()
        {
            var testLengthsBase10 = new Int32[] { 192, 96, 48, 24, 12, 72, 32, 16 };
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

        internal static List<RawSoundDirectingEvent> GetEnvelopeTestLength()
        {
            var result = new List<RawSoundDirectingEvent>
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

        internal static List<RawSoundDirectingEvent> GetSweepTest()
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

        public static List<RawSoundDirectingEvent> GetDeltaTest()
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
