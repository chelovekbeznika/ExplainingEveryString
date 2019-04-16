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
        private AssetsStorage assetsStorage;
        private Level level;

        internal EpicEventsProcessor(AssetsStorage assetsStorage, Level level)
        {
            this.assetsStorage = assetsStorage;
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
            String soundName = epicEvent.SpecEffectSpecification.Sound;
            Single volume = epicEvent.SpecEffectSpecification.Volume;
            assetsStorage.GetSound(soundName).Play(volume, 0, 0);
        }
    }
}
