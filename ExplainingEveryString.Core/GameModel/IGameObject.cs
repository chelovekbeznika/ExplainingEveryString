using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal interface IUpdatable
    {
        void Update(Single elapsedSeconds);
    }
    
    internal interface IMortal
    {
        Boolean IsAlive();
        void TakeDamage(Single damage);
    }

    internal interface ICollidable
    {
        Hitbox GetHitbox();
    }

    internal interface ICrashable : ICollidable
    {
        Single CollisionDamage { get; }
        void Destroy();
    }

    internal interface IGameObject : ICollidable, IUpdatable, IMortal, IDisplayble
    {

    }
}
