using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossPartBehavior : IEnemyBehavior
    {
        private FourthBossPart bossPart;
        private Vector2 offset;
        private Vector2 pulsationOffset;
        private String pulsationTag;
        private IFourthBossBrain BossBrain => bossPart.BossBrain;

        public EventHandler MoveGoalReached { get ; set; }

        public Weapon Weapon { get; private set; }

        public ISpawnedActorsController SpawnedActors => null;

        public PostMortemSurprise PostMortemSurprise => null;

        public Boolean IsTeleporter => false;

        public Single? EnemyAngle => BossBrain.Angle;

        public FourthBossPartBehavior(FourthBossPart bossPart, FourthBossPartBlueprint blueprint)
        {
            this.bossPart = bossPart;
            this.offset = blueprint.Offset;
            this.pulsationOffset = blueprint.PulsationOffset;
            this.pulsationTag = blueprint.PulsationTag;
        }

        public void Construct(EnemyBehaviorSpecification specification, Level level, ActorsFactory factory)
        {
            UpdatePosition();
            GiveWeapon(specification.Weapon, level);
        }

        public IEnumerable<IDisplayble> GetPartsToDisplay() => new IDisplayble[] { };

        public void Update(float elapsedSeconds)
        {
            UpdatePosition();
            Weapon?.Update(elapsedSeconds);
        }

        internal void UpdatePosition()
        {
            var fullOffset = offset + pulsationOffset * BossBrain.PulsationCoefficient(pulsationTag);
            (bossPart as ICollidable).Position = BossBrain.Position + GeometryHelper.RotateVector(fullOffset, BossBrain.Angle);
        }

        internal void GiveWeapon(WeaponSpecification specification, Level level)
        {
            if (Weapon != null)
                Weapon.Shoot -= level.EnemyShoot;
            if (specification != null)
            {
                IAimer aimer = null;
                if (specification.AimType == AimType.FixedFireDirection)
                    aimer = new FourthBossFixedDirectionAimer(bossPart);
                if (specification.AimType == AimType.AimAtPlayer)
                    aimer = new PlayerAimer(() => level.Player.Position);
                Weapon = new Weapon(specification, aimer, () => bossPart.Position, () => level.Player, level, false);
                Weapon.Shoot += level.EnemyShoot;
            }
        }
    }
}
