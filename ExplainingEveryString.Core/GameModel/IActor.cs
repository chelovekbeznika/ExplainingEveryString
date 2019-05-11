using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal interface IUpdatable
    {
        void Update(Single elapsedSeconds);
    }
    
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
    }

    internal interface ICrashable : ICollidable
    {
        Single CollisionDamage { get; }
        void Destroy();
    }

    internal interface IActor : ICollidable, IDisplayble, IUpdatable
    {
        Boolean IsAlive();
    }
}
