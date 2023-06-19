using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class SaveProfilesMenuBuilder : IMenuBuilder
    {
        private const Int32 ProfilesAmount = 5;

        private readonly EesGame game;
        private readonly LevelSequenceSpecification levelSequenceSpecification;

        internal SaveProfilesMenuBuilder(EesGame game, LevelSequenceSpecification levelSequenceSpecification)
        {
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var config = ConfigurationAccess.GetCurrentConfig();
            var items = Enumerable.Range(0, ProfilesAmount)
                .Select(number => BuildSaveProfileButton(number, menuVisiblePart)).ToArray();
            return new MenuItemsContainer(items, config.SaveProfile);
        }

        private DoubleSpriteMenuItemButton BuildSaveProfileButton(Int32 profileNumber, MenuVisiblePart visiblePart)
        {
            var saveProfile = GameProgressAccess.Load(profileNumber);
            var level = levelSequenceSpecification.Levels.FirstOrDefault(l => l.LevelData == saveProfile?.MaxAchievedLevelName)
                ?? levelSequenceSpecification.Levels.First();
            var levelButton = game.Content.Load<Texture2D>(level.ButtonSprite);
            var borderButton = game.Content.Load<Texture2D>($@"Sprites\Menu\Saves\{profileNumber}");
            var itemButton = new DoubleSpriteMenuItemButton(levelButton, new Vector2(24, 8), borderButton);
            itemButton.ItemCommandExecuteRequested += (sender, e) =>
            {
                game.GameState.SwitchSaveProfile(profileNumber);
                visiblePart.TryToGetBack();
            };
            return itemButton;
        }
    }
}
