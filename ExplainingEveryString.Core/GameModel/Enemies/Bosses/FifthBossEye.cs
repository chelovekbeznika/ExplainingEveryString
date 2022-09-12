using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FifthBossEye : IDisplayble, IUpdateable
    {
        private FifthBoss boss;
        private Vector2 offset;

        public FifthBossEye(FifthBoss boss, Vector2 offset, SpriteSpecification spriteSpecification, FifthBossWeaponMovement weaponMovement)
        {
            this.boss = boss;
            this.offset = offset;
            this.SpriteState = new SpriteState(spriteSpecification) { AnimationCycle = weaponMovement.FullCycleTime };
        }

        public SpriteState SpriteState { get; private set; }

        public Vector2 Position => boss.Position + offset;

        public bool IsVisible => boss.IsVisible && !boss.IsInAppearancePhase;

        public IEnumerable<IDisplayble> GetParts()
        {
            return Enumerable.Empty<IDisplayble>();
        }

        public void Update(Single elapsedSeconds)
        {
            SpriteState.Update(elapsedSeconds);
        }
    }
}
