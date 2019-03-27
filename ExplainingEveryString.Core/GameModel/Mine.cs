using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Mine : GameObject<MineBlueprint>
    {
        internal Single Damage { get; private set; }

        public override void Update(Single elapsedSeconds)
        {
        }

        protected override void Construct(MineBlueprint blueprint)
        {
            base.Construct(blueprint);
            this.Damage = blueprint.Damage;
        }

        internal void Destroy()
        {
            Hitpoints = 0;
        }
    }
}
