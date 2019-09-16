using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class PlayerDashController : IDisplayble
    {
        internal event EventHandler DashActivated;
        internal Boolean IsActive { get; private set; } = false;
        internal Single RechargeTime { get; }
        internal Single TillRecharge => 
            rechargeTimer != null && rechargeTimer.SecondsTillEvent > 0 ? rechargeTimer.SecondsTillEvent : 0;
        public Boolean IsVisible => IsActive;
        public SpriteState SpriteState { get; private set; }
        public Vector2 Position => player.Position;

        private EpicEvent dashActivatedEpicEvent;
        private Boolean recharged = true;
        private readonly Single duration;
        private Player player;
        private Timer rechargeTimer = null;

        internal PlayerDashController(DashSpecification specification, Player player, Level level)
        {
            this.RechargeTime = specification.RechargeTime;
            this.duration = specification.Duration;
            this.SpriteState = new SpriteState(specification.Sprite);
            this.player = player;
            this.dashActivatedEpicEvent = new EpicEvent(level, specification.SpecEffect, false, this, true);
        }

        internal void Update(Single elapsedSeconds)
        {
            if (recharged && player.Input.IsTryingToDash())
            {
                IsActive = true;
                recharged = false;
                DashActivated?.Invoke(this, EventArgs.Empty);
                dashActivatedEpicEvent.TryHandle();
                TimersComponent.Instance.ScheduleEvent(duration, () => IsActive = false);
                rechargeTimer = TimersComponent.Instance.ScheduleEvent(duration + RechargeTime, () => recharged = true);
            }
            SpriteState.Update(elapsedSeconds);
            SpriteState.Angle = AngleConverter.ToRadians(player.Input.GetMoveDirection());
        }
    }
}
