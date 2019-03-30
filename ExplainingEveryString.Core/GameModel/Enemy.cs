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
        internal Single CollisionDamage { get; set; }
        internal Single MaxSpeed { get; set; }
        protected Func<Vector2> PlayerLocator { get; private set; }

        public override abstract void Update(Single elapsedSeconds);

        protected override void Construct(TBlueprint blueprint, Level level)
        {
            base.Construct(blueprint, level);
            this.CollisionDamage = blueprint.CollisionDamage;
            this.MaxSpeed = blueprint.MaxSpeed;
            this.PlayerLocator = () => level.PlayerPosition;
        }

        internal void Destroy()
        {
            Hitpoints = 0;
        }
    }
}
