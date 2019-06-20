using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Data.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Door : Actor<DoorBlueprint>
    {
        private SpriteState openingSprite;
        private Boolean opened = false;
        internal Int32 OpeningWaveNumber { get; set; }
        private Single OpenCoefficient => opened ? SpriteState.ElapsedTime / SpriteState.AnimationCycle : 0;
        private Func<Single, Hitbox> getPartiallyOpenHitbox;

        public override SpriteState SpriteState => opened ? openingSprite : base.SpriteState;

        protected override void Construct(DoorBlueprint blueprint, ActorStartInfo info, Level level)
        {
            base.Construct(blueprint, info, level);
            this.openingSprite = new SpriteState(blueprint.OpeningSprite);
            switch (blueprint.OpeningMode)
            {
                case DoorOpeningMode.Instant: getPartiallyOpenHitbox = InstantOpen; break;
                case DoorOpeningMode.Up: getPartiallyOpenHitbox = OpenUp; break;
            }
        }

        protected override void PlaceOnLevel(ActorStartInfo info)
        {
            base.PlaceOnLevel(info);
        }

        internal void Open()
        {
            opened = true;
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            if (OpenCoefficient > 1 - Math.Constants.Epsilon)
                this.Destroy();
        }

        public override Hitbox GetCurrentHitbox()
        {
            if (!opened)
                return base.GetCurrentHitbox();
            else
                return getPartiallyOpenHitbox(OpenCoefficient);
        }

        private Hitbox InstantOpen(Single openingCoefficient)
        {
            return base.GetCurrentHitbox();
        }

        private Hitbox OpenUp(Single openingCoefficient)
        {
            Hitbox wholeHitbox = base.GetCurrentHitbox();
            Single height = wholeHitbox.Top - wholeHitbox.Bottom;
            Single toCut = height * openingCoefficient;
            return new Hitbox
            {
                Top = wholeHitbox.Top,
                Bottom = wholeHitbox.Bottom + toCut,
                Left = wholeHitbox.Left,
                Right = wholeHitbox.Right
            };
        }
    }
}
