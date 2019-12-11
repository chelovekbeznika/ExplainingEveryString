namespace ExplainingEveryString.Core.Music.Model
{
    internal enum Alteration { None, Sharp, Flat }

    internal enum NoteLength { Whole = 1, Half = 2, Quarter = 4, Eigth = 8, Sixteenth = 16 }

    internal enum NoteType { C = 1, D = 2, E = 3, F = 4, G = 5, A = 6, H = 7 }

    internal enum Octave { SubContra, Contra, Great, Small, OneLine, TwoLine, ThreeLine, FourLine, FiveLine }

    internal struct Note
    {
        internal NoteType Type { get; set; }
        internal Octave Octave { get; set; }

        internal Note(Octave octave, NoteType note)
        {
            Octave = octave;
            Type = note;
        }

    }
}
