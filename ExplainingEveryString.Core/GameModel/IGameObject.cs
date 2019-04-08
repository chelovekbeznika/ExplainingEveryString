﻿using Microsoft.Xna.Framework;
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
    }

    internal interface ICollidable
    {
        Hitbox GetCurrentHitbox();
        Hitbox GetOldHitbox();
        Vector2 Position { set; }
    }

    internal interface ICrashable : ICollidable
    {
        Single CollisionDamage { get; }
        void Destroy();
    }

    internal interface IGameObject : ICollidable, IDisplayble
    {
        Boolean IsAlive();
    }
}
