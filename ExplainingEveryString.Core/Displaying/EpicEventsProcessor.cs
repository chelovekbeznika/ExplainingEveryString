using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Displaying
{
    internal class EpicEventsProcessor
    {
        private Dictionary<String, SoundEffect> soundEffectsStorage;
        private Level level;

        internal EpicEventsProcessor(Dictionary<String, SoundEffect> soundEffectsStorage, Level level)
        {
            this.soundEffectsStorage = soundEffectsStorage;
            this.level = level;
        }

        internal void ProcessEpicEvents()
        {
            foreach (EpicEventArgs epicEvent in level.CollectEpicEvents())
            {
                ProcessEpicEvent(epicEvent);
            }
        }

        private void ProcessEpicEvent(EpicEventArgs epicEvent)
        {
            String sound = epicEvent.SpecEffectSpecification.Sound;
            Single volume = epicEvent.SpecEffectSpecification.Volume;
            soundEffectsStorage[sound].Play(volume, 0, 0);
        }
    }
}
