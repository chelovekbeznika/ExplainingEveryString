using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.GameState;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Timers
{
    internal class TimersComponent : GameComponent
    {
        internal static TimersComponent Instance { get; private set; }

        internal static void Init(Game game)
        {
            Instance = new TimersComponent(game);
            game.Components.Add(Instance);
        }

        private List<Timer> timers = new List<Timer>();
        private List<Timer> scheduledTimers = new List<Timer>();

        private TimersComponent(Game game) : base(game)
        {
            UpdateOrder = ComponentsOrder.Timers;
        }

        public Timer ScheduleEvent(float seconds, Action atTimerElapsed, IActor eventProducer)
        {
            return ScheduleEvent(seconds, atTimerElapsed, () => eventProducer.IsAlive());
        }

        public Timer ScheduleEvent(float seconds, Action atTimerElapsed)
        {
            return ScheduleEvent(seconds, atTimerElapsed, () => true);
        }

        private Timer ScheduleEvent(float seconds, Action atTimerElapsed, Func<bool> isActive)
        {
            var timer = new Timer(seconds, atTimerElapsed, isActive);
            scheduledTimers.Add(timer);
            return timer;
        }

        public override void Update(GameTime gameTime)
        {
            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timers.AddRange(scheduledTimers);
            scheduledTimers.Clear();
            foreach (var timer in timers)
            {
                timer.Update(elapsedSeconds);
            }
            timers = timers.Where(t => !t.Happened).ToList();
        }
    }
}
