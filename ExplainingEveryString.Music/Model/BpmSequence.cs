using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ExplainingEveryString.Music.Model
{
    internal class BpmSequence : ISoundDirectingSequence
    {
        public Int32 Seconds { get; set; }
        [DefaultValue(0)]
        public Int32 SamplesOffset { get; set; }
        [DefaultValue(90)]
        public Int32 BeatsPerMinute { get; set; }
        [DefaultValue(null)]
        public Dictionary<NoteType, Accidental> SequenceAccidental { get; set; }
        public IEnumerable<BpmSoundDirectingEvent> CommonPart { get; set; }
        [DefaultValue(1)]
        public Int32 RepeatTimes { get; set; }
        public Single OneRepeatBeats { get; set; }
        [DefaultValue(0)]
        public Single StartingBeat { get; set; }
        [DefaultValue(null)]
        public Dictionary<Int32, IEnumerable<BpmSoundDirectingEvent>> UnderRepeatSign { get; set; }

        public IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            foreach (var timeToRepeat in Enumerable.Range(0, RepeatTimes))
            {
                foreach (var note in CommonPart)
                    foreach (var directingEvent in GetEventsFromNote(note, timeToRepeat))
                        yield return directingEvent;

                if (UnderRepeatSign != null && UnderRepeatSign.ContainsKey(timeToRepeat))
                    foreach (var note in UnderRepeatSign[timeToRepeat])
                        foreach (var directingEvent in GetEventsFromNote(note, timeToRepeat))
                            yield return directingEvent;
            }
            yield break;
        }

        private IEnumerable<RawSoundDirectingEvent> GetEventsFromNote(BpmSoundDirectingEvent note, Int32 timeToRepeat)
        {
            note.BeatsPerMinute = BeatsPerMinute;
            note.StartingBeat = StartingBeat;
            note.OneRepeatBeats = OneRepeatBeats;
            note.TimeToRepeat = timeToRepeat;
            if (SequenceAccidental != null && note is INote)
                ApplySequenceAccidental(note as INote);      

            foreach (var rawSoundDirectingEvent in note.GetEvents())
            {
                rawSoundDirectingEvent.Seconds += Seconds;
                rawSoundDirectingEvent.SamplesOffset += SamplesOffset;
                yield return rawSoundDirectingEvent;
            }
            yield break;
        }

        private void ApplySequenceAccidental(INote note)
        {
            var noteType = note.Note.Type;
            var currentAccidental = SequenceAccidental.ContainsKey(noteType)
                ? SequenceAccidental[noteType] : Accidental.None;
            if (currentAccidental != Accidental.None && note.Accidental != Accidental.Natural)
                note.Accidental = currentAccidental; 
        }
    }
}
