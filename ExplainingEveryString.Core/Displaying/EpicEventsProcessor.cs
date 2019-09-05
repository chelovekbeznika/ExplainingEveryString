using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Displaying
{
    internal class EpicEventsProcessor
    {
        private AssetsStorage assetsStorage;
        private Level level;
        private List<SpecEffect> activeSpecEffects = new List<SpecEffect>();
        private readonly Single fadingOutDistance;

        internal EpicEventsProcessor(AssetsStorage assetsStorage, Level level, Configuration config)
        {
            this.assetsStorage = assetsStorage;
            this.level = level;
            this.fadingOutDistance = config.SoundFadingOut;
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
            ProcessSound(epicEvent);
            ProcessAnimation(epicEvent);
        }

        private void ProcessSound(EpicEventArgs epicEvent)
        {
            SoundSpecification sound = epicEvent.SpecEffectSpecification.Sound;
            if (sound != null)
            {
                Single distance = (level.Player.Position - epicEvent.Position).Length();
                Single currentFadingOutDistance = fadingOutDistance * sound.FadingCoeff;
                if (distance <= currentFadingOutDistance)
                {
                    Single nearEpicenterVolume = epicEvent.SpecEffectSpecification.Sound.Volume;
                    Single fadingCoeff = 1 - (distance / currentFadingOutDistance);
                    Single volume = nearEpicenterVolume * fadingCoeff;
                    assetsStorage.GetSound(sound.Name).Play(volume, 0, 0);
                }
            }
        }

        private void ProcessAnimation(EpicEventArgs epicEvent)
        {
            SpriteSpecification sprite = epicEvent.SpecEffectSpecification.Sprite;
            if (sprite != null)
            {
                SpecEffect specEffect = new SpecEffect(epicEvent.Position, epicEvent.Angle, sprite);
                activeSpecEffects.Add(specEffect);
            }
        }
    }
}
