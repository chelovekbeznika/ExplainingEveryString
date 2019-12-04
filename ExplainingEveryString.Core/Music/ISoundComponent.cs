using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Music
{
    internal interface ISoundComponent
    {
        void ProcessSoundDirectingEvent(SoundDirectingEvent soundEvent);
        void MoveEmulationTowardNextSample();
    }
}
