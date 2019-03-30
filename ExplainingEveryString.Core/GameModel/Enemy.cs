using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal abstract class Enemy<TBlueprint> : GameObject<TBlueprint> where TBlueprint : EnemyBlueprint
    {
        public Single CollisionDamage { get; set; }
        internal Single MaxSpeed { get; set; }
        private Func<Vector2> playerLocator;
        protected Vector2 PlayerPosition => playerLocator();

        public override abstract void Update(Single elapsedSeconds);

        protected override void Construct(TBlueprint blueprint, Level level)
        {
            base.Construct(blueprint, level);
            this.CollisionDamage = blueprint.CollisionDamage;
            this.MaxSpeed = blueprint.MaxSpeed;
            this.playerLocator = () => level.PlayerPosition;
        }
    }
}
