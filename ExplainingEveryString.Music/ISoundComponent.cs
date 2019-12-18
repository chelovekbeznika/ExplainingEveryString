using ExplainingEveryString.Music.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Music
{
    internal interface ISoundComponent
    {
        void ProcessSoundDirectingEvent(RawSoundDirectingEvent soundEvent);
        void MoveEmulationTowardNextSample();
    }
}
