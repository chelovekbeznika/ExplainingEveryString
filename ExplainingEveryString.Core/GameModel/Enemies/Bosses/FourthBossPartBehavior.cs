using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossPartBehavior : IEnemyBehavior
    {
        private FourthBossPart bossPart;
        private Vector2 bossPartOffset => bossPart.Offset;
        private IFourthBossBrain BossBrain => bossPart.BossBrain;

        public EventHandler MoveGoalReached { get ; set; }

        public Weapon Weapon => null;

        public ISpawnedActorsController SpawnedActors => null;

        public PostMortemSurprise PostMortemSurprise => null;

        public Boolean IsTeleporter => false;

        public Single? EnemyAngle => BossBrain.Angle;

        public FourthBossPartBehavior(FourthBossPart bossPart)
        {
            this.bossPart = bossPart;
        }

        public void Construct(EnemyBehaviorSpecification specification, Level level, ActorsFactory factory)
        {
            UpdatePosition();
        }

        public IEnumerable<IDisplayble> GetPartsToDisplay() => new IDisplayble[] { };

        public void Update(float elapsedSeconds)
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            (bossPart as ICollidable).Position = BossBrain.Position + GeometryHelper.RotateVector(bossPartOffset, BossBrain.Angle);
        }
    }
}
