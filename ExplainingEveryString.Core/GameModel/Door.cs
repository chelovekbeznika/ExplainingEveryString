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

        public override SpriteState SpriteState => opened ? openingSprite : base.SpriteState;

        protected override void Construct(DoorBlueprint blueprint, ActorStartInfo info, Level level)
        {
            base.Construct(blueprint, info, level);
            this.openingSprite = new SpriteState(blueprint.OpeningSprite);
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
            if (opened && SpriteState.ElapsedTime > SpriteState.AnimationCycle - Math.Constants.Epsilon)
                this.Destroy();
        }
    }
}
