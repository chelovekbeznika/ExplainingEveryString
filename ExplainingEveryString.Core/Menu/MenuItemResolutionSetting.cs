using ExplainingEveryString.Core.Extensions;
using ExplainingEveryString.Core.Menu.Settings;
using ExplainingEveryString.Core.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemResolutionSetting : MenuItem
    {
        private CustomFont font;
        private List<Resolution> resolutions;
        private Int32 selectedIndex = 0;

        private Resolution SelectedResolution => resolutions[selectedIndex];

        internal override BorderType BorderType => BorderType.Setting;

        internal MenuItemResolutionSetting(FontsStorage fontsStorage, GraphicsAdapter adapter)
        {
            this.font = fontsStorage.SmallNumbers;
            this.resolutions = adapter.AllowedResolutions();
            var resolutionSet = SettingsAccess.GetCurrentSettings().Resolution;
            this.selectedIndex = resolutions.FindIndex(r => r.Width == resolutionSet.Width && r.Height == resolutionSet.Height);
        }

        internal override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            font.Draw(spriteBatch, position, SelectedResolution.ToString());
        }

        internal override Point GetSize() => font.GetSize(SelectedResolution.ToString());

        internal override void RequestCommandExecution()
        {
            Increment();
        }

        internal override void Increment()
        {
            selectedIndex += 1;
            if (selectedIndex >= resolutions.Count)
                selectedIndex = 0;
            UpdateSettings();
        }

        internal override void Decrement()
        {
            selectedIndex -= 1;
            if (selectedIndex < 0)
                selectedIndex = resolutions.Count - 1;
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            SettingsAccess.GetCurrentSettings().Resolution = SelectedResolution;
        }
    }
}
