using ExplainingEveryString.Core.GameModel;
using System;

namespace ExplainingEveryString.Core
{
    internal class Timer : IUpdateable
    {
        private readonly Action atTimerElapsed;
        internal Boolean Happened { get; private set; } = false;
        internal Single SecondsTillEvent { get; private set; }

        internal Timer(Single secondsTillEvent, Action atTimerElapsed)
        {
            this.SecondsTillEvent = secondsTillEvent;
            this.atTimerElapsed = atTimerElapsed;
        }

        public void Update(Single elapsedSeconds)
        {
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
    }
}
