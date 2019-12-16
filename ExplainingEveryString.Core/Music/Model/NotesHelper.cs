using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Music.Model
{
    internal static class NotesHelper
    {
        private static readonly Dictionary<Note, Single> notesFrequencies = new Dictionary<Note, Single>
        {
            { new Note(Octave.SubContra, NoteType.C), 16.352F },
            { new Note(Octave.SubContra, NoteType.D), 18.354F },
            { new Note(Octave.SubContra, NoteType.E), 20.602F },
            { new Note(Octave.SubContra, NoteType.F), 21.827F },
            { new Note(Octave.SubContra, NoteType.G), 24.5F },
            { new Note(Octave.SubContra, NoteType.A), 27.5F },
            { new Note(Octave.SubContra, NoteType.H), 30.868F },

            { new Note(Octave.Contra, NoteType.C), 32.703F },
            { new Note(Octave.Contra, NoteType.D), 36.708F },
            { new Note(Octave.Contra, NoteType.E), 41.203F },
            { new Note(Octave.Contra, NoteType.F), 43.654F },
            { new Note(Octave.Contra, NoteType.G), 48.999F },
            { new Note(Octave.Contra, NoteType.A), 55F },
            { new Note(Octave.Contra, NoteType.H), 61.735F },

            { new Note(Octave.Great, NoteType.C), 65.406F },
            { new Note(Octave.Great, NoteType.D), 73.416F },
            { new Note(Octave.Great, NoteType.E), 82.407F },
            { new Note(Octave.Great, NoteType.F), 87.307F },
            { new Note(Octave.Great, NoteType.G), 97.999F },
            { new Note(Octave.Great, NoteType.A), 110F },
            { new Note(Octave.Great, NoteType.H), 123.47F },

            { new Note(Octave.Small, NoteType.C), 130.81F },
            { new Note(Octave.Small, NoteType.D), 146.83F },
            { new Note(Octave.Small, NoteType.E), 164.81F },
            { new Note(Octave.Small, NoteType.F), 174.61F },
            { new Note(Octave.Small, NoteType.G), 196F },
            { new Note(Octave.Small, NoteType.A), 220F },
            { new Note(Octave.Small, NoteType.H), 246.94F },

            { new Note(Octave.OneLine, NoteType.C), 261.63F },
            { new Note(Octave.OneLine, NoteType.D), 293.66F },
            { new Note(Octave.OneLine, NoteType.E), 329.63F },
            { new Note(Octave.OneLine, NoteType.F), 349.23F },
            { new Note(Octave.OneLine, NoteType.G), 392F },
            { new Note(Octave.OneLine, NoteType.A), 440F },
            { new Note(Octave.OneLine, NoteType.H), 493.88F },

            { new Note(Octave.TwoLine, NoteType.C), 523.25F },
            { new Note(Octave.TwoLine, NoteType.D), 587.33F },
            { new Note(Octave.TwoLine, NoteType.E), 659.26F },
            { new Note(Octave.TwoLine, NoteType.F), 698.46F },
            { new Note(Octave.TwoLine, NoteType.G), 783.99F },
            { new Note(Octave.TwoLine, NoteType.A), 880F },
            { new Note(Octave.TwoLine, NoteType.H), 987.77F },

            { new Note(Octave.ThreeLine, NoteType.C), 1046.5F },
            { new Note(Octave.ThreeLine, NoteType.D), 1174.7F },
            { new Note(Octave.ThreeLine, NoteType.E), 1318.5F },
            { new Note(Octave.ThreeLine, NoteType.F), 1396.9F },
            { new Note(Octave.ThreeLine, NoteType.G), 1568F },
            { new Note(Octave.ThreeLine, NoteType.A), 1760F },
            { new Note(Octave.ThreeLine, NoteType.H), 1975.5F },

            { new Note(Octave.FourLine, NoteType.C), 2093F },
            { new Note(Octave.FourLine, NoteType.D), 2349.3F },
            { new Note(Octave.FourLine, NoteType.E), 2637F },
            { new Note(Octave.FourLine, NoteType.F), 2793.8F },
            { new Note(Octave.FourLine, NoteType.G), 3136F },
            { new Note(Octave.FourLine, NoteType.A), 3520F },
            { new Note(Octave.FourLine, NoteType.H), 3951.1F },

            { new Note(Octave.FiveLine, NoteType.C), 4186F },
            { new Note(Octave.FiveLine, NoteType.D), 4698.6F },
            { new Note(Octave.FiveLine, NoteType.E), 5274F },
            { new Note(Octave.FiveLine, NoteType.F), 5587.7F },
            { new Note(Octave.FiveLine, NoteType.G), 6271.9F },
            { new Note(Octave.FiveLine, NoteType.A), 7040F },
            { new Note(Octave.FiveLine, NoteType.H), 7902.1F },
        };

        internal static Int32 PulseTimer(Note note, Alteration alteration)
        {
            Single frequency = GetFrequency(note, alteration);
            return (Int32)System.Math.Round(Constants.CpuFrequency / (16 * frequency) - 1);
        }

        internal static Int32 TriangleTimer(Note note, Alteration alteration)
        {
            Single frequency = GetFrequency(note, alteration);
            return (Int32)System.Math.Round(Constants.CpuFrequency / (32 * frequency) - 1);
        }

        internal static Single GetFrequency(Note note, Alteration alteration = Alteration.None)
        {
            switch (alteration)
            {
                case Alteration.None: return notesFrequencies[note];
                case Alteration.Sharp: return (Single)System.Math.Sqrt(notesFrequencies[note] * notesFrequencies[GetNextNote(note)]);
                case Alteration.Flat: return (Single)System.Math.Sqrt(notesFrequencies[note] * notesFrequencies[GetPreviousNote(note)]);
                default: throw new ArgumentException(nameof(alteration));
            }
        }

        internal static Note GetNextNote(Note note)
        {
            NoteType noteType = note.Type == NoteType.H ? NoteType.C : (NoteType)((Int32)note.Type + 1);
            Octave octave = note.Type == NoteType.H ? (Octave)((Int32)note.Octave + 1) : note.Octave;
            return new Note(octave, noteType);
        }

        internal static Note GetPreviousNote(Note note)
        {
            NoteType noteType = note.Type == NoteType.C ? NoteType.H : (NoteType)((Int32)note.Type - 1);
            Octave octave = note.Type == NoteType.C ? (Octave)((Int32)note.Octave - 1) : note.Octave;
            return new Note(octave, noteType);
        }
    }
}
