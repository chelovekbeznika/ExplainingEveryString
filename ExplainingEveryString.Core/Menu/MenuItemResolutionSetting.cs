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
        private List<Resolution> fullScreenResolutions;
        private List<Resolution> windowResolutions;
        private Int32 selectedFullscreenIndex = 0;
        private Int32 selectedWindowIndex = 0;

        private Boolean Fullscreen => SettingsAccess.GetCurrentSettings().Fullscreen;

        private Resolution SelectedResolution => Fullscreen
            ? fullScreenResolutions[selectedFullscreenIndex]
            : windowResolutions[selectedWindowIndex];

        internal override BorderType BorderType => BorderType.Setting;

        internal MenuItemResolutionSetting(FontsStorage fontsStorage, GraphicsAdapter adapter)
        {
            this.font = fontsStorage.ScreenResolution;
            this.fullScreenResolutions = adapter.AllowedResolutions(true);
            this.windowResolutions = adapter.AllowedResolutions(false);
            var resolutionSet = SettingsAccess.GetCurrentSettings().Resolution;
            if (Fullscreen)
            {
                this.selectedFullscreenIndex = fullScreenResolutions
                    .FindIndex(r => r.Width == resolutionSet.Width && r.Height == resolutionSet.Height);
                this.selectedWindowIndex = windowResolutions.Count - 1;
            }
            else
            {
                this.selectedWindowIndex = windowResolutions
                    .FindIndex(r => r.Width == resolutionSet.Width && r.Height == resolutionSet.Height);
                this.selectedFullscreenIndex = fullScreenResolutions.Count - 1;
            }
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
            if (Fullscreen)
            {
                selectedFullscreenIndex += 1;
                if (selectedFullscreenIndex >= fullScreenResolutions.Count)
                    selectedFullscreenIndex = 0;
            }
            else
            {
                selectedWindowIndex += 1;
                if (selectedWindowIndex >= windowResolutions.Count)
                    selectedWindowIndex = 0;
            }
            UpdateSettings();
        }

        internal override void Decrement()
        {
            if (Fullscreen)
            {
                selectedFullscreenIndex -= 1;
                if (selectedFullscreenIndex < 0)
                    selectedFullscreenIndex = fullScreenResolutions.Count - 1;
            }
            else
            {
                selectedWindowIndex -= 1;
                if (selectedWindowIndex < 0)
                    selectedWindowIndex = windowResolutions.Count - 1;
            }
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            SettingsAccess.GetCurrentSettings().Resolution = SelectedResolution;
        }
    }
}
