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
        public void PulseTimer_ANoteOneLineOctave_Returns253()
        {
            Note note = new Note(Octave.OneLine, NoteType.A);
            Int32 timer = NotesHelper.PulseTimer(note, Alteration.None);
            Assert.AreEqual(253, timer);
        }
    }
}
