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
        private List<SpecEffect> activeSpecEffects = new List<SpecEffect>();

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

        internal void Update(Single elapsedSeconds)
        {
            foreach(SpecEffect specEffect in activeSpecEffects)
            {
                specEffect.Update(elapsedSeconds);
            }
            activeSpecEffects = activeSpecEffects.Where(se => se.IsAlive()).ToList();
        }

        internal IEnumerable<IDisplayble> GetSpecEffectsToDraw()
        {
            return activeSpecEffects;
        }

        private void ProcessEpicEvent(EpicEventArgs epicEvent)
        {
            String soundName = epicEvent.SpecEffectSpecification.Sound;
            Single volume = epicEvent.SpecEffectSpecification.Volume;
            assetsStorage.GetSound(soundName).Play(volume, 0, 0);

            if (epicEvent.SpecEffectSpecification.Sprite != null)
            {
                activeSpecEffects.Add(new SpecEffect(epicEvent.Position, epicEvent.SpecEffectSpecification.Sprite));
            }
        }
    }
}
