using ExplainingEveryString.Music.Model;

namespace ExplainingEveryString.Music
{
    internal interface ISoundComponent
    {
        void ProcessSoundDirectingEvent(RawSoundDirectingEvent soundEvent);
        void MoveEmulationTowardNextSample();
    }
}
