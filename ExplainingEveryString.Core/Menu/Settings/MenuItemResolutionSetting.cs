using ExplainingEveryString.Core.Extensions;
using ExplainingEveryString.Core.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal class MenuItemResolutionSetting : MenuItem, IMenuItemDisplayble
    {
        private readonly CustomFont font;
        private readonly List<Resolution> fullScreenResolutions;
        private readonly List<Resolution> windowResolutions;
        private int selectedFullscreenIndex = 0;
        private int selectedWindowIndex = 0;

        private bool Fullscreen => SettingsAccess.GetCurrentSettings().Fullscreen;

        private Resolution SelectedResolution => Fullscreen
            ? fullScreenResolutions[selectedFullscreenIndex]
            : windowResolutions[selectedWindowIndex];

        internal override BorderType BorderType => BorderType.Setting;

        internal override IMenuItemDisplayble Displayble => this;

        internal MenuItemResolutionSetting(FontsStorage fontsStorage, GraphicsAdapter adapter)
        {
            font = fontsStorage.ScreenResolution;
            fullScreenResolutions = adapter.AllowedResolutions(true);
            windowResolutions = adapter.AllowedResolutions(false);
            var resolutionSet = SettingsAccess.GetCurrentSettings().Resolution;
            if (Fullscreen)
            {
                selectedFullscreenIndex = fullScreenResolutions
                    .FindIndex(r => r.Width == resolutionSet.Width && r.Height == resolutionSet.Height);
                selectedWindowIndex = windowResolutions.Count - 1;
            }
            else
            {
                selectedWindowIndex = windowResolutions
                    .FindIndex(r => r.Width == resolutionSet.Width && r.Height == resolutionSet.Height);
                selectedFullscreenIndex = fullScreenResolutions.Count - 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            font.Draw(spriteBatch, position, SelectedResolution.ToString());
        }

        public Point GetSize() => font.GetSize(SelectedResolution.ToString());

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
