using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

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
            var itemsContainer = new MenuItemsContainer(items, config.SaveProfile);
            itemsContainer.ContainerAppearedOnScreen += (sender, e) =>
            {
                var currentProfile = config.SaveProfile;
                var newLevelButton = GetMaxLevelButton(currentProfile);
                (items[currentProfile].Displayble as TwoSpritesDisplayer).ChangeableSprite = newLevelButton;
            };
            return itemsContainer;
        }

        private MenuItem BuildSaveProfileButton(Int32 profileNumber, MenuVisiblePart visiblePart)
        {
            var levelButton = GetMaxLevelButton(profileNumber);
            var borderButton = game.Content.Load<Texture2D>($@"Sprites\Menu\Saves\{profileNumber}");
            var displayer = new TwoSpritesDisplayer(borderButton, new Vector2(24, 8), levelButton);
            var itemButton = new MenuItemButton(displayer);
            itemButton.ItemCommandExecuteRequested += (sender, e) =>
            {
                game.GameState.SwitchSaveProfile(profileNumber);
                visiblePart.TryToGetBack();
            };
            return itemButton;
        }

        private Texture2D GetMaxLevelButton(Int32 profileNumber)
        {
            var saveProfile = GameProgressAccess.Load(profileNumber);
            var level = levelSequenceSpecification.Levels.FirstOrDefault(l => l.LevelData == saveProfile?.MaxAchievedLevelName)
                ?? levelSequenceSpecification.Levels.First();
            return game.Content.Load<Texture2D>(level.ButtonSprite);
        }
    }
}
