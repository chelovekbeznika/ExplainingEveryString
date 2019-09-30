using Microsoft.Xna.Framework;
using System;

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
        Vector2 Position { get; set; }
        CollidableMode CollidableMode { get; }
    }

    internal interface IMovableCollidable : ICollidable
    {
        Hitbox GetOldHitbox();
        Vector2 OldPosition { get; }
        String CollideTag { get; }
    }

    internal interface ICrashable : IMovableCollidable
    {
        Single CollisionDamage { get; }
        void Crash();
    }

    internal enum CollidableMode { Solid, Pit, Shadow, Ghost }
}
