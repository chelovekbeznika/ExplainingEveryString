﻿using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuButtonHandler
    {
        internal event EventHandler ButtonPressed;
        private readonly Func<Boolean> isButtonPressed;
        private const Single buttonCooldown = 1.0F / 3;
        private Single cooldownRemained = 0;

        internal MenuButtonHandler(Func<Boolean> isButtonPressed)
        {
            this.isButtonPressed = isButtonPressed;
        }

        internal void Update(Single elapsedSeconds)
        {
            if (cooldownRemained < Math.Constants.Epsilon)
            {
                if (isButtonPressed())
                {
                    ButtonPressed(this, EventArgs.Empty);
                    cooldownRemained = buttonCooldown;
                }
            }
            else
                cooldownRemained -= elapsedSeconds;
        }
    }
}
