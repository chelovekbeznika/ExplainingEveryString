using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Accidental { None, Sharp, Flat, Natural }

    [JsonConverter(typeof(StringEnumConverter))]
    [Flags]
    public enum NoteLength 
    { 
        Whole = 1, Half = 2, Quarter = 4, Eigth = 8, Sixteenth = 16, ThirtySecond = 32,
        Dotted = 64, DoubleDotted = 128, TripleDotted = 256, Triplet = 512
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum NoteType { C = 1, D = 2, E = 3, F = 4, G = 5, A = 6, H = 7 }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Octave { SubContra, Contra, Great, Small, OneLine, TwoLine, ThreeLine, FourLine, FiveLine }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Articulation { Legato, NonLegato, Stacatto }

    public struct Note
    {
        public NoteType Type { get; set; }
        [DefaultValue(Octave.OneLine)]
        public Octave Octave { get; set; }

        public Note(Octave octave, NoteType note)
        {
            Octave = octave;
            Type = note;
        }
    }
}
