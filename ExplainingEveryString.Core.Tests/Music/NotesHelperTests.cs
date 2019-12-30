using ExplainingEveryString.Music.Model;
using NUnit.Framework;
using System;

namespace ExplainingEveryString.Core.Tests.Music
{
    [TestFixture]
    public class NotesHelperTests
    {
        [Test]
        public void PulseTimer_ANoteOneLineOctave_Returns253()
        {
            Note note = new Note(Octave.OneLine, NoteType.A);
            Int32 timer = NotesHelper.PulseTimer(note, Accidental.None);
            Assert.AreEqual(253, timer);
        }
    }
}
