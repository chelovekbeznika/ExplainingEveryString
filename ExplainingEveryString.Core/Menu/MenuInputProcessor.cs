using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal abstract class MenuInputProcessor
    {
        protected abstract MenuButtonHandler[] Buttons { get; set; }

        internal MenuInputProcessor()
        {
            InitButtons();
        }

        internal void Update(GameTime gameTime)
        {
            var elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var button in Buttons)
                button.Update(elapsedSeconds);
        }

        protected abstract void InitButtons();
    }
}
