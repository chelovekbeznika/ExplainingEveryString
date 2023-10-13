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
        private IMenuBuilder storyLevelSelectBuilder;
        private IMenuBuilder timeAttackBuilder;
        private IMenuBuilder musicTestBuilder;
        private IMenuBuilder settingsBuilder;
        private IMenuBuilder saveProfilesBuilder;

        internal MenuBuilder(EesGame game, LevelSequenceSpecification levelSequenceSpecification, 
            IMenuBuilder storyLevelSelectBuilder,
            IMenuBuilder timeAttackBuilder, IMenuBuilder musicTestBuilder, 
            IMenuBuilder settingsBuilder, IMenuBuilder saveProfilesMenuBuilder)
        {
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
            this.storyLevelSelectBuilder = storyLevelSelectBuilder;
            this.timeAttackBuilder = timeAttackBuilder;
            this.musicTestBuilder = musicTestBuilder;
            this.settingsBuilder = settingsBuilder;
            this.saveProfilesBuilder = saveProfilesMenuBuilder;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var content = game.Content;
            var saveProfileNumber = ConfigurationAccess.GetCurrentConfig().SaveProfile;
            var saveProfile = GameProgressAccess.Load(saveProfileNumber);
            var saveFileIcon = content.Load<Texture2D>(@"Sprites/Menu/CurrentSaveIcon");

            var unpauseButtonDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Unpause"));
            var unpause = new MenuItemButton(unpauseButtonDisplayer)
            {
                Text = "CONTINUE",
                IsVisible = () => game.GameState.IsPaused
            };
            unpause.ItemCommandExecuteRequested += (sender, e) => game.GameState.TryPauseSwitch();

            var continueButtonDisplayer = new TwoSpritesDisplayer(
                baseSprite: content.Load<Texture2D>(@"Sprites/Menu/Continue"),
                offset: new Vector2(16, 20),
                changeableSprite: GetCurrentLevelButton(saveProfile));
            var continueStory = new MenuItemButton(continueButtonDisplayer) 
            { 
                Text = "LOAD LAST" 
            };
            continueStory.ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueCurrentGame();

            var newGameButtonDisplayer = new TwoSpritesDisplayer(
                baseSprite: content.Load<Texture2D>(@"Sprites/Menu/NewGame"),
                offset: new Vector2(16, 20),
                changeableSprite: content.Load<Texture2D>(levelSequenceSpecification.Levels.First().ButtonSprite));
            var newGame = new MenuItemButton(newGameButtonDisplayer) 
            { 
                Text = "NEW GAME" 
            };
            newGame.ItemCommandExecuteRequested += (sender, e) => game.GameState.StartNewGame();

            var levelSelectButtonDisplayer = new TwoSpritesDisplayer(
                baseSprite: content.Load<Texture2D>(@"Sprites/Menu/LevelSelect"),
                offset: new Vector2(16, 21),
                changeableSprite: GetMaxLevelButton(saveProfile));
            var levelSelect = new MenuItemWithContainer(levelSelectButtonDisplayer,
                storyLevelSelectBuilder.BuildMenu(menuVisiblePart), menuVisiblePart)
            { 
                Text = "SELECT LEVEL" 
            };

            var saveProfilesButtonDisplayer = new TwoSpritesDisplayer(
                baseSprite: content.Load<Texture2D>(@"Sprites/Menu/SaveProfiles"),
                offset: CalculateCurrentSaveIconOffset(saveProfileNumber, saveFileIcon.Width),
                changeableSprite: saveFileIcon);
            var selectSave = new MenuItemWithContainer(saveProfilesButtonDisplayer,
                saveProfilesBuilder.BuildMenu(menuVisiblePart), menuVisiblePart)
            { 
                Text = "SELECT SAVE" 
            };

            var timeAttackButtonDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/TimeAttack"));
            var timeAttack = new MenuItemWithContainer(timeAttackButtonDisplayer,
                timeAttackBuilder.BuildMenu(menuVisiblePart), menuVisiblePart)
            {
                Text = "TIME ATTACK",
                IsVisible = () =>
                {
                    var saveProfile = ConfigurationAccess.GetCurrentConfig().SaveProfile;
                    var gameProgress = GameProgressAccess.Load(saveProfile);
                    return gameProgress?.TimeAttackModeOpened ?? false;
                }
            };

            var settingsButtonDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Settings/Submenu"));
            var settings = new MenuItemWithContainer(settingsButtonDisplayer,
                settingsBuilder.BuildMenu(menuVisiblePart), menuVisiblePart)
            {
                Text = "SETTINGS"
            };

            var musicTestButtonDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/MusicTest"));
            var musicTest = new MenuItemWithContainer(musicTestButtonDisplayer,
                musicTestBuilder.BuildMenu(menuVisiblePart), menuVisiblePart)
            { 
                Text = "MUSIC TEST" 
            };

            var tutorialButtonDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Tutorial"));
            var tutorial = new MenuItemButton(tutorialButtonDisplayer)
            {
                Text = "TUTORIAL"
            };
            tutorial.ItemCommandExecuteRequested += (sender, args) => game.GameState.ShowTutorial();

            var exitButtonDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Exit"));
            var exit = new MenuItemButton(exitButtonDisplayer) 
            { 
                Text = "EXIT" 
            };
            exit.ItemCommandExecuteRequested += (sender, e) => game.Exit();

            var items = new MenuItemButton[]
            {
                unpause, continueStory, newGame, levelSelect, selectSave, timeAttack, settings, musicTest, tutorial, exit
            };

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
