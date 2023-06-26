using ExplainingEveryString.Core.Menu.Settings;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuBuilder : IMenuBuilder
    {
        private const Int32 BetweenSaveIconsPixels = 3;
        private const Int32 FirstIconOffset = 37;

        private EesGame game;
        private LevelSequenceSpecification levelSequenceSpecification;
        private IMenuBuilder levelSelectBuilder;
        private IMenuBuilder musicTestBuilder;
        private IMenuBuilder settingsMenuBuilder;
        private IMenuBuilder saveProfilesMenuBuilder;

        internal MenuBuilder(EesGame game, LevelSequenceSpecification levelSequenceSpecification, 
            IMenuBuilder levelSelectBuilder, IMenuBuilder musicTestBuilder, 
            IMenuBuilder settingsMenuBuilder, IMenuBuilder saveProfilesMenuBuilder)
        {
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
            this.levelSelectBuilder = levelSelectBuilder;
            this.musicTestBuilder = musicTestBuilder;
            this.settingsMenuBuilder = settingsMenuBuilder;
            this.saveProfilesMenuBuilder = saveProfilesMenuBuilder;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var content = game.Content;
            var saveProfileNumber = ConfigurationAccess.GetCurrentConfig().SaveProfile;
            var saveProfile = GameProgressAccess.Load(saveProfileNumber);
            var saveFileIcon = content.Load<Texture2D>(@"Sprites/Menu/CurrentSaveIcon");

            var newGameButtonDisplayer = new TwoSpritesDisplayer(
                content.Load<Texture2D>(@"Sprites/Menu/NewGame"),
                new Vector2(16, 20),
                content.Load<Texture2D>(levelSequenceSpecification.Levels.First().ButtonSprite));
            var continueButtonDisplayer = new TwoSpritesDisplayer(
                content.Load<Texture2D>(@"Sprites/Menu/Continue"),
                new Vector2(16, 20),
                GetCurrentLevelButton(saveProfile));
            var levelSelectButtonDisplayer = new TwoSpritesDisplayer(
                content.Load<Texture2D>(@"Sprites/Menu/LevelSelect"),
                new Vector2(16, 21),
                GetMaxLevelButton(saveProfile));

            var saveProfilesButtonDisplayer = new TwoSpritesDisplayer(
                content.Load<Texture2D>(@"Sprites/Menu/SaveProfiles"),
                CalculateCurrentSaveIconOffset(saveProfileNumber, saveFileIcon.Width),
                saveFileIcon);

            var items = new MenuItemButton[]
            {
                new MenuItemButton(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Unpause"))) { Text = "CONTINUE" },
                new MenuItemButton(continueButtonDisplayer) { Text = "LOAD LAST" },
                new MenuItemButton(newGameButtonDisplayer) { Text = "NEW GAME" },
                new MenuItemWithContainer(levelSelectButtonDisplayer,
                    levelSelectBuilder.BuildMenu(menuVisiblePart), menuVisiblePart) { Text = "SELECT LEVEL" },
                new MenuItemWithContainer(saveProfilesButtonDisplayer,
                    saveProfilesMenuBuilder.BuildMenu(menuVisiblePart), menuVisiblePart) { Text = "SELECT SAVE" },
                new MenuItemWithContainer(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Settings/Submenu")),
                    settingsMenuBuilder.BuildMenu(menuVisiblePart), menuVisiblePart) { Text = "SETTINGS" },
                new MenuItemWithContainer(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/MusicTest")),
                    musicTestBuilder.BuildMenu(menuVisiblePart), menuVisiblePart) { Text = "MUSIC TEST" },
                new MenuItemButton(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Exit"))) { Text = "EXIT" }
            };

            items[0].ItemCommandExecuteRequested += (sender, e) => game.GameState.TryPauseSwitch();
            items[1].ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueCurrentGame();
            items[2].ItemCommandExecuteRequested += (sender, e) => game.GameState.StartNewGame();

            items[^1].ItemCommandExecuteRequested += (sender, e) => game.Exit();

            items[0].IsVisible = () => game.GameState.IsPaused;
            var container = new MenuItemsContainer(items);
            container.ContainerAppearedOnScreen += (sender, e) =>
            {
                var saveProfile = ConfigurationAccess.GetCurrentConfig().SaveProfile;
                var gameProgress = GameProgressAccess.Load(saveProfile);

                continueButtonDisplayer.ChangeableSprite = GetCurrentLevelButton(gameProgress);
                levelSelectButtonDisplayer.ChangeableSprite = GetMaxLevelButton(gameProgress);
                saveProfilesButtonDisplayer.Offset = CalculateCurrentSaveIconOffset(saveProfile, saveFileIcon.Width);
            };
            return container;
        }

        private Texture2D GetMaxLevelButton(GameProgress saveProfile)
        {
            var level = levelSequenceSpecification.Levels.FirstOrDefault(l => l.LevelData == saveProfile?.MaxAchievedLevelName)
                ?? levelSequenceSpecification.Levels.First();
            return game.Content.Load<Texture2D>(level.ButtonSprite);
        }

        private Texture2D GetCurrentLevelButton(GameProgress saveProfile)
        {
            var level = levelSequenceSpecification.Levels.FirstOrDefault(l => l.LevelData == saveProfile?.CurrentLevelFileName)
                ?? levelSequenceSpecification.Levels.First();
            return game.Content.Load<Texture2D>(level.ButtonSprite);
        }

        private Vector2 CalculateCurrentSaveIconOffset(Int32 profileNumber, Int32 width)
        {
            var xOffset = FirstIconOffset + (width + BetweenSaveIconsPixels) * profileNumber;
            return new Vector2(xOffset, 3);
        }
    }
}
