using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core
{
    internal class TimersComponent : GameComponent
    {
        internal static TimersComponent Instance { get; private set; }

        internal static void Init(Game game)
        {
            Instance = new TimersComponent(game);
            game.Components.Insert(ComponentsOrder.Timers, Instance);
        }

        private List<Timer> timers = new List<Timer>();

        private TimersComponent(Game game) : base(game)
        {
            UpdateOrder = ComponentsOrder.Timers;
        }

        public void ScheduleEvent(Single seconds, Action atTimerElapsed)
        {
            timers.Add(new Timer(seconds, atTimerElapsed));
        }

        public override void Update(GameTime gameTime)
        {
            Single elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Timer timer in timers)
            {
                timer.Update(elapsedSeconds);
            }
            timers = timers.Where(t => !t.Happened).ToList();
        }
    }
}
