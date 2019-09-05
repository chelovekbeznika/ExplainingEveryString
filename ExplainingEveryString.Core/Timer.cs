using ExplainingEveryString.Core.GameModel;
using System;

namespace ExplainingEveryString.Core
{
    internal class Timer : IUpdatable
    {
        private Single secondsTillEvent;
        private Action atTimerElapsed;
        internal Boolean Happened { get; private set; } = false;

        internal Timer(Single secondsTillEvent, Action atTimerElapsed)
        {
            this.secondsTillEvent = secondsTillEvent;
            this.atTimerElapsed = atTimerElapsed;
        }

        public void Update(Single elapsedSeconds)
        {
            if (!Happened)
            {
                secondsTillEvent -= elapsedSeconds;
                if (secondsTillEvent < Math.Constants.Epsilon)
                {
                    atTimerElapsed();
                    Happened = true;
                }
            }
        }
    }
}
