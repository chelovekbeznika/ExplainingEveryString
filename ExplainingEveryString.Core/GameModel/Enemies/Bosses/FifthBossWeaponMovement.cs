using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FifthBossWeaponMovement : IUpdateable
    {
        //List<Tuple<TimeInPosition, Position>>
        private List<Tuple<Single, Vector2>> movementSpecification;
        private Int32 currentIndex = 0;
        private Single timeInCurrentPosition = 0;

        public Vector2 CurrentOffset => movementSpecification[currentIndex].Item2;

        public Single FullCycleTime => movementSpecification.Sum(pair => pair.Item1);

        public FifthBossWeaponMovement(List<Tuple<Single, Vector2>> specification)
        {
            this.movementSpecification = specification;
        }

        public void Update(Single elapsedSeconds)
        {
            timeInCurrentPosition += elapsedSeconds;
            while (timeInCurrentPosition >= movementSpecification[currentIndex].Item1)
            {
                timeInCurrentPosition -= movementSpecification[currentIndex].Item1;
                currentIndex += 1;
                if (currentIndex >= movementSpecification.Count)
                    currentIndex -= movementSpecification.Count;
            }
        }
    }
}
