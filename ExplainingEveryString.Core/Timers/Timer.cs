using ExplainingEveryString.Core.GameModel;
using System;

namespace ExplainingEveryString.Core.Timers
{
    internal class Timer : IUpdateable
    {
        private readonly Action atTimerElapsed;
        internal bool Happened { get; private set; } = false;
        internal float SecondsTillEvent { get; private set; }
        private Func<bool> isActive;

        internal Timer(float secondsTillEvent, Action atTimerElapsed, Func<bool> isActive)
        {
            SecondsTillEvent = secondsTillEvent;
            this.atTimerElapsed = atTimerElapsed;
            this.isActive = isActive;
        }

        public void Update(float elapsedSeconds)
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
