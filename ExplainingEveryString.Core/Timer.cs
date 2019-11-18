using ExplainingEveryString.Core.GameModel;
using System;

namespace ExplainingEveryString.Core
{
    internal class Timer : IUpdateable
    {
        private readonly Action atTimerElapsed;
        internal Boolean Happened { get; private set; } = false;
        internal Single SecondsTillEvent { get; private set; }
        private Func<Boolean> isActive;

        internal Timer(Single secondsTillEvent, Action atTimerElapsed, Func<Boolean> isActive)
        {
            this.SecondsTillEvent = secondsTillEvent;
            this.atTimerElapsed = atTimerElapsed;
            this.isActive = isActive;
        }

        public void Update(Single elapsedSeconds)
        {
            if (!isActive())
                Cancel();
            if (!Happened)
            {
                SecondsTillEvent -= elapsedSeconds;
                if (SecondsTillEvent < Math.Constants.Epsilon)
                {
                    atTimerElapsed();
                    Happened = true;
                }
            }
        }

        private void Cancel()
        {
            Happened = true;
        }
    }
}
