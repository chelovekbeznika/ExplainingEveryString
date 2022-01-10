using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal interface IEnemyBehavior
    {
        void Construct(EnemyBehaviorSpecification specification, Level level, ActorsFactory factory);
        void Update(Single elapsedSeconds);
        IEnumerable<IDisplayble> GetPartsToDisplay();
        EventHandler MoveGoalReached { get; set; }
        Weapon Weapon { get; }
        ISpawnedActorsController SpawnedActors { get; }
        PostMortemSurprise PostMortemSurprise { get; }
        Boolean IsTeleporter { get; }
        Single? EnemyAngle { get; }
    }

    internal interface IChangeableEnemyBehavior
    {
        void ChangeWeapon(WeaponSpecification specification, Level level);
        void ChangeMover(MoverSpecification specification);
    }
}
