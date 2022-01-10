using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Core.GameModel.Weaponry;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel
{
    public interface IUpdateable
    {
        void Update(Single elapsedSeconds);
    }

    internal interface IActor : ICollidable, IDisplayble, IUpdateable
    {
        Boolean IsAlive();
    }

    internal interface IEnemy : IActor, IInterfaceAccessable, IMovableCollidable, ICrashable, ITouchableByBullets, IDisplayble
    {
        ISpawnedActorsController SpawnedActorsController { get; }
        List<IEnemy> Avengers { get; }
        void ProcessDeath();
        event EventHandler<EnemyBehaviorChangedEventArgs> EnemyBehaviorChanged;
        event EventHandler Died;
    }

    internal interface IChangeableActor : IUpdateable
    {
        void ReactToChangingEvent(String enemyChangingEvent);
        event EventHandler<ChangingEventArgs> ChangingEventOccured;
    }
}
