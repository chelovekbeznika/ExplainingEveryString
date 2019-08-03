using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal interface ITouchableByBullets : ICollidable
    {
        void TakeDamage(Single damage);
        Hitbox GetBulletsHitbox();
    }

    internal interface ICollidable
    {
        Hitbox GetCurrentHitbox();
        Hitbox GetOldHitbox();
        Vector2 Position { get; set; }
        Vector2 OldPosition { get; }
        CollidableMode Mode { get; }
    }

    internal interface ICrashable : ICollidable
    {
        Single CollisionDamage { get; }
        void Destroy();
    }

    internal enum CollidableMode { Solid, Pit, Shadow, Ghost }
}
