using ExplainingEveryString.Music.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tests.Music
{
    [TestFixture]
    public class NotesHelperTests
    {
        [Test]
        public void GetNextNote_MiddleOfOctave_ReturnsSameOctave()
        {
            Note note = new Note(Octave.OneLine, NoteType.D);
            Note nextNote = NotesHelper.GetNextNote(note);
            Assert.AreEqual(new Note(Octave.OneLine, NoteType.E), nextNote);
        }

        [Test]
        public void GetNextNote_HNote_ReturnsNextOctave()
        {
            Note note = new Note(Octave.OneLine, NoteType.H);
            Note nextNote = NotesHelper.GetNextNote(note);
            Assert.AreEqual(new Note(Octave.TwoLine, NoteType.C), nextNote);
        }

        [Test]
        public void GetPreviousNote_MiddleOfOctave_ReturnsSameOctave()
        {
            Note note = new Note(Octave.OneLine, NoteType.D);
            Note previousNote = NotesHelper.GetPreviousNote(note);
            Assert.AreEqual(new Note(Octave.OneLine, NoteType.C), previousNote);
        }

        [Test]
        public void GetPreviousNote_CNote_ReturnsPreviousOctave()
        {
            Note note = new Note(Octave.OneLine, NoteType.C);
            Note previousNote = NotesHelper.GetPreviousNote(note);
            Assert.AreEqual(new Note(Octave.Small, NoteType.H), previousNote);
        }

        [Test]
        public void PulseTimer_ANoteOneLineOctave_Returns253()
        {
            Note note = new Note(Octave.OneLine, NoteType.A);
            Int32 timer = NotesHelper.PulseTimer(note, Alteration.None);
            Assert.AreEqual(253, timer);
        }
    }
}
