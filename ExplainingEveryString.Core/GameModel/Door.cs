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
        private OneTimeEpicEvent startedToOpen;
        private OneTimeEpicEvent completelyOpened;
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
                case DoorOpeningMode.Down: getPartiallyOpenHitbox = OpenDown; break;
                case DoorOpeningMode.Left: getPartiallyOpenHitbox = OpenLeft; break;
                case DoorOpeningMode.Right: getPartiallyOpenHitbox = OpenRight; break;
                default: throw new ArgumentException("blueprint.OpeningMode");
            }
            this.startedToOpen = new OneTimeEpicEvent(level, blueprint.OpeningStartedEffect, this);
            this.completelyOpened = new OneTimeEpicEvent(level, blueprint.CompletelyOpenedEffect, this);
        }

        protected override void PlaceOnLevel(ActorStartInfo info)
        {
            base.PlaceOnLevel(info);
        }

        internal void Open()
        {
            opened = true;
            startedToOpen.TryHandle();
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            if (OpenCoefficient > 1 - Math.Constants.Epsilon)
            {
                this.Destroy();
                completelyOpened.TryHandle();
            }
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
            Single toCut = (wholeHitbox.Top - wholeHitbox.Bottom) * openingCoefficient;
            return new Hitbox
            {
                Top = wholeHitbox.Top,
                Bottom = wholeHitbox.Bottom + toCut,
                Left = wholeHitbox.Left,
                Right = wholeHitbox.Right
            };
        }

        private Hitbox OpenDown(Single openingCoefficient)
        {
            Hitbox wholeHitbox = base.GetCurrentHitbox();
            Single toCut = (wholeHitbox.Top - wholeHitbox.Bottom) * openingCoefficient;
            return new Hitbox
            {
                Top = wholeHitbox.Top - toCut,
                Bottom = wholeHitbox.Bottom,
                Left = wholeHitbox.Left,
                Right = wholeHitbox.Right,
            };
        }

        private Hitbox OpenRight(Single openingCoefficient)
        {
            Hitbox wholeHitbox = base.GetCurrentHitbox();
            Single toCut = (wholeHitbox.Right - wholeHitbox.Left) * openingCoefficient;
            return new Hitbox
            {
                Top = wholeHitbox.Top,
                Bottom = wholeHitbox.Bottom,
                Left = wholeHitbox.Left + toCut,
                Right = wholeHitbox.Right
            };
        }

        private Hitbox OpenLeft(Single openingCoefficient)
        {
            Hitbox wholeHitbox = base.GetBulletsHitbox();
            Single toCut = (wholeHitbox.Right - wholeHitbox.Left) * openingCoefficient;
            return new Hitbox
            {
                Top = wholeHitbox.Top,
                Bottom = wholeHitbox.Bottom,
                Left = wholeHitbox.Left,
                Right = wholeHitbox.Right - toCut
            };
        }
    }
}
