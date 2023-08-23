using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Core.Text;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core
{
    internal class TimeAttackResultsComponent : StaticImagesSequenceComponent
    {
        private const Int32 UpperPartHeight = 128;
        private const Int32 BetweenRows = 12;
        private const Int32 BetweenElements = 12;

        private readonly LevelSequenceSpecification levelSequenceSpecification;
        private Texture2D background;
        private Texture2D wholeGameRoute;
        private SoundEffect recordFireworkSound;
        private SpriteData newLevelRecordNotice;
        private SpriteData recordFirework;
        private Dictionary<String, Texture2D> levelButtons;
        private GameProgress gameProgress;
        private Dictionary<String, Single> currentSplits;
        private String newRecordLevel = null;
        private RecordCelebrationGenerator recordCelebrationGenerator = null;

        private CustomFont TimeFont => (Game as EesGame).FontsStorage.LevelTime;

        public TimeAttackResultsComponent(EesGame game, LevelSequenceSpecification levelSequenceSpecification) : 
            base(game, 1.0F, 60F, 1)
        {
            this.levelSequenceSpecification = levelSequenceSpecification;

            this.DrawOrder = ComponentsOrder.Cutscene;
            this.UpdateOrder = ComponentsOrder.Cutscene;
        }

        public override void Update(GameTime gameTime)
        {
            recordCelebrationGenerator?.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        internal void UpdateGameProgress(GameProgress gameProgress, Dictionary<String, Single> currentSplits)
        {
            this.gameProgress = gameProgress;
            this.currentSplits = currentSplits;
        }

        internal void NotifyNewLevelRecord(String levelName)
        {
            newRecordLevel = levelName;
        }

        internal void NotifyNewGameRecord()
        {
            var config = ConfigurationAccess.GetCurrentConfig();
            recordCelebrationGenerator = new RecordCelebrationGenerator(recordFirework, recordFireworkSound, config.PersonalBestCelebration);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            background = Game.Content.Load<Texture2D>(@"Sprites/Menu/TimeAttackBackground");
            wholeGameRoute = Game.Content.Load<Texture2D>(@"Sprites/Menu/WholeGameRouteInTimeTable");
            recordFireworkSound = Game.Content.Load<SoundEffect>(@"Sounds/Record");

            levelButtons = levelSequenceSpecification.Levels.ToDictionary(
                keySelector: l => l.LevelData, 
                elementSelector: l => Game.Content.Load<Texture2D>(l.ButtonSprite));

            var metadataLoader = AssetsMetadataAccess.GetLoader();
            var spriteDataBuilder = new SpriteDataBuilder(Game.Content, metadataLoader);
            var recordSpriteName = @"Sprites/Menu/NewRecordNotice";
            var recordFireworksSpriteName = @"Sprites/Menu/RecordFireworks";
            var sprites = spriteDataBuilder.Build(new[] { recordSpriteName, recordFireworksSpriteName });
            newLevelRecordNotice = sprites[recordSpriteName];
            recordFirework = sprites[recordFireworksSpriteName];
        }

        protected override void DrawImage(SpriteBatch spriteBatch, Int32 frameNumber)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            if (gameProgress.PersonalBest.HasValue)
                DrawWholeGameResult(spriteBatch, gameProgress.PersonalBest.Value);

            var currentResult = gameProgress.LevelRecords;
            var nextButtonPlaceholder = new Vector2(Displaying.Constants.TargetWidth / 2, UpperPartHeight);
            foreach (var level in levelSequenceSpecification.Levels)
            {
                var levelName = level.LevelData;
                if (currentResult.ContainsKey(levelName))
                {
                    var levelResult = currentResult[levelName];
                    DrawLevelResults(spriteBatch, levelName, levelResult, ref nextButtonPlaceholder);
                }
            }

            recordCelebrationGenerator?.Draw(spriteBatch);
        }

        private void DrawWholeGameResult(SpriteBatch spriteBatch, Single wholeGameRecord)
        {
            var timeString = GameTimeHelper.ToTimeString(wholeGameRecord);
            var timeSize = TimeFont.GetSize(timeString);
            var upperPartSize = new Vector2(
                x: wholeGameRoute.Width + BetweenElements + timeSize.X,
                y: System.Math.Max(wholeGameRoute.Height, timeSize.Y));
            var upperPartPosition = new Vector2(
                x: Displaying.Constants.TargetWidth / 2 - upperPartSize.X / 2,
                y: UpperPartHeight / 2 - upperPartSize.Y / 2);
            var picturePosition = new Vector2(
                x: upperPartPosition.X,
                y: upperPartPosition.Y + upperPartSize.Y / 2 - wholeGameRoute.Height / 2);
            var textPosition = new Vector2(
                x: upperPartPosition.X + wholeGameRoute.Width + BetweenElements,
                y: upperPartPosition.Y + upperPartSize.Y / 2 - timeSize.Y / 2);

            spriteBatch.Draw(wholeGameRoute, picturePosition, Color.White);
            TimeFont.Draw(spriteBatch, textPosition, timeString);
        }

        private void DrawLevelResults(SpriteBatch spriteBatch, String levelName, 
            Single levelResult, ref Vector2 nextButtonPlaceholder)
        {
            var isNewRecord = levelName == newRecordLevel;
            var timeString = GameTimeHelper.ToTimeString(levelResult);
            var textSize = TimeFont.GetSize(timeString);
                    
            var currentButton = levelButtons[levelName];
            var wholeWidth = textSize.X + BetweenElements + currentButton.Width;
            var currentButtonPosition = nextButtonPlaceholder + new Vector2(-wholeWidth / 2, 0);
            var currentTextPosition = new Vector2(
                x: currentButtonPosition.X + currentButton.Width + BetweenElements,
                y: currentButtonPosition.Y + currentButton.Height / 2 - textSize.Y / 2);

            spriteBatch.Draw(currentButton, currentButtonPosition, Color.White);
            TimeFont.Draw(spriteBatch, currentTextPosition, timeString);

            if (isNewRecord)
            {
                var recordNoticePosition = new Vector2(
                    x: currentTextPosition.X + BetweenElements + textSize.X + BetweenElements, 
                    y: nextButtonPlaceholder.Y);
                var partToDraw = AnimationHelper.GetDrawPart(newLevelRecordNotice, FrameTime);
                spriteBatch.Draw(newLevelRecordNotice.Sprite, recordNoticePosition, partToDraw, Color.White);
            }

            if (currentSplits?.ContainsKey(levelName) ?? false)
            {
                var currentSplit = currentSplits[levelName];
                var splitText = GameTimeHelper.ToTimeString(currentSplit);
                var splitTextSize = TimeFont.GetSize(splitText);
                var splitTextPosition = new Vector2(
                    x: BetweenElements, 
                    y: currentButtonPosition.Y + currentButton.Height / 2 - splitTextSize.Y / 2);
                TimeFont.Draw(spriteBatch, splitTextPosition, splitText);
            }

            if (gameProgress.PersonalBestSplits?.ContainsKey(levelName) ?? false)
            {
                var recordSplit = gameProgress.PersonalBestSplits[levelName];
                var recordText = GameTimeHelper.ToTimeString(recordSplit);
                var recordTextSize = TimeFont.GetSize(recordText);
                var recordTextPosition = new Vector2(
                    x: Displaying.Constants.TargetWidth - BetweenElements - recordTextSize.X,
                    y: currentButtonPosition.Y + currentButton.Height / 2 - recordTextSize.Y / 2);
                TimeFont.Draw(spriteBatch, recordTextPosition, recordText);
            }

            nextButtonPlaceholder += new Vector2(0, BetweenRows + currentButton.Height);
        }
    }
}
